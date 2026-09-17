using System.Collections.Concurrent;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Patch;
using EvoSC.Manialinks.Vdom.Runtime;
using EvoSC.Manialinks.Vdom.Vdom;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks.Vdom.Views;

public sealed class ManialinkViewManager : IManialinkViewManager
{
    private readonly IServerClient _server;
    private readonly IPatchTransport _patchTransport;
    private readonly ILogger<ManialinkViewManager> _logger;
    private readonly ConcurrentDictionary<string, MountedView> _viewsByName = new();

    public ManialinkViewManager(
        IServerClient server,
        IPatchTransport patchTransport,
        IEventManager events,
        ILogger<ManialinkViewManager> logger)
    {
        _server = server;
        _patchTransport = patchTransport;
        _logger = logger;

        // Mirrors ManialinkManager's own PlayerConnect subscription (ManialinkManager.cs:50) --
        // without this, a disconnected player's never-acked backlog entry would hold the shared
        // backlog open forever for everyone else in the view's audience (see TrimBacklogLocked).
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerDisconnect)
            .WithInstance(this)
            .WithInstanceClass<ManialinkViewManager>()
            .WithHandlerMethod<PlayerDisconnectGbxEventArgs>(HandlePlayerDisconnectAsync)
            .AsAsync()
        );
    }

    public async Task<IManialinkView> MountAsync(IEnumerable<IPlayer> players, IVdomView view, object props)
    {
        var playerList = players.ToList();
        var mountedView = new MountedView(view, _server, _patchTransport, _logger, () => Forget(view.Name));

        await mountedView.MountAsync(playerList, props);

        _viewsByName[view.Name] = mountedView;

        return mountedView;
    }

    public void RecordAck(string viewName, string login, int seq)
    {
        // Confirms the client actually received, parsed and applied a patch -- the only direct
        // evidence of that, short of eyeballing the game. See VdomAckController.
        _logger.LogInformation("Vdom ack for view '{View}' from '{Login}': seq {Seq}.", viewName, login, seq);

        if (!_viewsByName.TryGetValue(viewName, out var view))
        {
            _logger.LogWarning("Vdom ack for unknown view '{View}' (already unmounted?).", viewName);
            return;
        }

        view.RecordAck(login, seq);
    }

    internal void Forget(string viewName) => _viewsByName.TryRemove(viewName, out _);

    private Task HandlePlayerDisconnectAsync(object sender, PlayerDisconnectGbxEventArgs e)
    {
        foreach (var view in _viewsByName.Values)
        {
            view.RemovePlayer(e.Login);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// One mounted view's full lifecycle. See the class docs on the simplification this makes
    /// relative to the plan's L6 sketch (one shared mount/backlog for the whole audience, not
    /// independent per-player mount state) -- correct as long as every player in the audience
    /// started from the same mount, which holds for M1's only view (a data-identical broadcast
    /// widget). Diverging per-player state (e.g. one player needing a remount the rest don't) is
    /// not supported; a remount always targets the whole current audience.
    /// </summary>
    private sealed class MountedView(
        IVdomView view,
        IServerClient server,
        IPatchTransport patchTransport,
        ILogger logger,
        Action onUnmounted) : IManialinkView
    {
        private readonly IReadOnlyList<PoolBucketRange> _layout = PoolLayout.Compute(view.PoolBudget);
        private readonly object _stateLock = new();
        private readonly List<(int Seq, IReadOnlyList<VPatchOp> Ops)> _backlog = new();
        private readonly ConcurrentDictionary<string, IPlayer> _playersByLogin = new();
        private readonly ConcurrentDictionary<string, int> _lastAckedSeqByLogin = new();

        private SlotAllocator _allocator = null!;
        private IReadOnlyList<MountedNode> _mounted = Array.Empty<MountedNode>();
        private int _currentSeq;
        private string? _runtimeScript;

        public string Name => view.Name;

        public async Task MountAsync(IReadOnlyList<IPlayer> players, object props)
        {
            string xml;

            lock (_stateLock)
            {
                _allocator = new SlotAllocator(_layout);

                var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), view.Render(props), _allocator);
                _mounted = mounted;
                _currentSeq = 0;
                _backlog.Clear();

                xml = BuildMountPageXml();
            }

            // Deliberately LogInformation, not LogDebug: the whole L4-L6 pipeline is unverified
            // in-game (see the plan's M1 progress notes), so the mount XML needs to be visible
            // in default-level logs the first several times this runs, not hidden behind a log
            // level someone has to remember to enable.
            logger.LogInformation("Vdom mount XML for view '{View}':\n{Xml}", Name, xml);

            foreach (var player in players)
            {
                _playersByLogin[player.GetLogin()] = player;
                _lastAckedSeqByLogin[player.GetLogin()] = 0;
            }

            await SendToLoginsAsync(players.Select(p => p.GetLogin()), xml);
        }

        public async Task UpdateAsync(object props)
        {
            IReadOnlyList<VPatchOp>? opsToSend = null;
            int seqToSend;
            string? remountXml = null;

            lock (_stateLock)
            {
                try
                {
                    var (diffOps, newMounted) = Reconciler.Diff(_mounted, view.Render(props), _allocator);

                    if (diffOps.Count == 0)
                    {
                        logger.LogInformation("Vdom update for view '{View}': no-op diff, nothing to send.", Name);
                        return;
                    }

                    _mounted = newMounted;
                    _currentSeq++;
                    _backlog.Add((_currentSeq, diffOps));
                    TrimBacklogLocked();

                    opsToSend = _backlog.SelectMany(entry => entry.Ops).ToList();
                    seqToSend = _currentSeq;
                }
                catch (SlotPoolExhaustedException ex)
                {
                    logger.LogWarning(ex,
                        "Pool exhausted for view '{View}' ({Kind}, interactive: {Interactive}) -- remounting.",
                        Name, ex.Kind, ex.Interactive);

                    _allocator = new SlotAllocator(_layout);
                    var (_, freshMounted) = Reconciler.Diff(Array.Empty<MountedNode>(), view.Render(props),
                        _allocator);
                    _mounted = freshMounted;
                    _currentSeq = 0;
                    _backlog.Clear();

                    remountXml = BuildMountPageXml();
                    seqToSend = 0;
                }
            }

            if (remountXml != null)
            {
                foreach (var login in _lastAckedSeqByLogin.Keys)
                {
                    _lastAckedSeqByLogin[login] = 0;
                }

                await SendToLoginsAsync(_playersByLogin.Keys, remountXml);
                return;
            }

            if (opsToSend == null)
            {
                return;
            }

            var opsJson = PatchSerializer.SerializeOps(opsToSend);

            // Same reasoning as the mount log: this whole path is unverified in-game, so it
            // needs to be visible by default while that's true.
            logger.LogInformation(
                "Vdom patch for view '{View}': seq {Seq}, {OpCount} ops, {PlayerCount} players.\n{Json}",
                Name, seqToSend, opsToSend.Count, _playersByLogin.Count, opsJson);

            await patchTransport.SendAsync(_playersByLogin.Values, Name, seqToSend, opsJson);
        }

        public async Task AddPlayersAsync(IEnumerable<IPlayer> players)
        {
            var newPlayers = players.Where(p => !_playersByLogin.ContainsKey(p.GetLogin())).ToList();

            if (newPlayers.Count == 0)
            {
                return;
            }

            string xml;
            int seq;

            lock (_stateLock)
            {
                xml = BuildMountPageXml();
                seq = _currentSeq;
            }

            foreach (var player in newPlayers)
            {
                _playersByLogin[player.GetLogin()] = player;
                // Their fresh mount already reflects everything up to the current seq, so they
                // don't need the accumulated backlog replayed on top of it.
                _lastAckedSeqByLogin[player.GetLogin()] = seq;
            }

            await SendToLoginsAsync(newPlayers.Select(p => p.GetLogin()), xml);
        }

        public async Task UnmountAsync()
        {
            var hideXml = $"""<?xml version="1.0" encoding="utf-8" standalone="yes" ?><manialink version="3" id="{VdomNaming.ViewPageId(Name)}"></manialink>""";

            await SendToLoginsAsync(_playersByLogin.Keys, hideXml);

            _playersByLogin.Clear();
            _lastAckedSeqByLogin.Clear();

            lock (_stateLock)
            {
                _backlog.Clear();
            }

            onUnmounted();
        }

        public void RecordAck(string login, int seq)
        {
            if (!_playersByLogin.ContainsKey(login))
            {
                return;
            }

            _lastAckedSeqByLogin.AddOrUpdate(login, seq, (_, existing) => Math.Max(existing, seq));

            lock (_stateLock)
            {
                TrimBacklogLocked();
            }
        }

        /// <summary>Removes a disconnected player from this view's audience and ack tracking.</summary>
        public void RemovePlayer(string login)
        {
            _playersByLogin.TryRemove(login, out _);
            _lastAckedSeqByLogin.TryRemove(login, out _);

            lock (_stateLock)
            {
                TrimBacklogLocked();
            }
        }

        private void TrimBacklogLocked()
        {
            if (_lastAckedSeqByLogin.IsEmpty)
            {
                return;
            }

            var minAcked = _lastAckedSeqByLogin.Values.Min();
            _backlog.RemoveAll(entry => entry.Seq <= minAcked);
        }

        /// <summary>Must be called while holding <see cref="_stateLock"/> -- reads <see cref="_mounted"/>.</summary>
        private string BuildMountPageXml()
        {
            var body = MountXmlWriter.WriteBody(_layout, _mounted);
            _runtimeScript ??= RuntimeScriptBuilder.Build(Name, _layout);

            // _runtimeScript already carries its own <!-- ... --> wrapper (see VdomRuntime.ms's
            // header comment -- matching the existing codebase's resource-script convention,
            // e.g. Templates/Scripts/Button.ms), so it is not added again here.
            return $"""
                <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
                <manialink version="3" id="{VdomNaming.ViewPageId(Name)}" name="EvoSC#-Vdom-{Name}">
                {body}<script>{_runtimeScript}</script>
                </manialink>
                """;
        }

        private Task SendToLoginsAsync(IEnumerable<string> logins, string xml)
        {
            var multiCall = new MultiCall();

            foreach (var login in logins)
            {
                multiCall.Add(nameof(server.Remote.SendDisplayManialinkPageToLoginAsync), login, xml, 0, false);
            }

            return server.Remote.MultiCallAsync(multiCall);
        }
    }
}
