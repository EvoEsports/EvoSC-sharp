using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;
using EvoSC.Common.Util;
using EvoSC.Common.Util.ServerUtils;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote.ChatRouter;

public class RemoteChatRouter : IRemoteChatRouter
{
    private readonly ILogger<RemoteChatRouter> _logger;
    private readonly IServerClient _server;
    private readonly IPlayerManagerService _players;
    private readonly IActionPipelineManager _pipelineManager;

    public RemoteChatRouter(ILogger<RemoteChatRouter> logger, IServerClient server, IEventManager events,
        IPlayerManagerService players, IActionPipelineManager pipelineManager)
    {
        _logger = logger;
        _server = server;
        _players = players;
        _pipelineManager = pipelineManager;

        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerChat)
            .WithInstance(this)
            .WithInstanceClass<ServerCallbackHandler>()
            .WithHandlerMethod<PlayerChatGbxEventArgs>(HandlePlayerChatRoutingAsync)
            .AsAsync()
        );
    }

    private async Task HandlePlayerChatRoutingAsync(object sender, PlayerChatGbxEventArgs e)
    {
        var actorAccountId = PlayerUtils.ConvertLoginToAccountId(e.Login);
        var actor = await _players.GetOnlinePlayerAsync(actorAccountId);
        var onlinePlayers = await _players.GetOnlinePlayersAsync();

        await SendMessageAsync(new ChatRouterPipelineContext
        {
            ForwardMessage = true,
            Author = actor,
            MessageText = e.Text,
            Recipients = onlinePlayers.ToList(),
            IsTeamMessage = e.Options == 3
        });
    }

    public async Task SendMessageAsync(string message, IOnlinePlayer actor)
    {
        var onlinePlayers = await _players.GetOnlinePlayersAsync();
        await SendMessageAsync(new ChatRouterPipelineContext
        {
            ForwardMessage = true,
            Author = actor,
            MessageText = message,
            Recipients = onlinePlayers.ToList(),
            IsTeamMessage = false
        });
    }

    public async Task SendMessageAsync(ChatRouterPipelineContext pipelineContext)
    {
        try
        {
            var player = await _players.GetOnlinePlayerAsync(pipelineContext.Author.AccountId);

            if (player.Flags.IsServer)
            {
                _logger.LogDebug("Chat message is from server, will ignore");
                return;
            }

            var pipelineChain = _pipelineManager.BuildChain(PipelineType.ChatRouter, async (context) =>
            {
                if (context is ChatRouterPipelineContext {ForwardMessage: true} chatContext)
                {
                    Task.Run(async () =>
                    {
                        var formatted = FormattingUtils.FormatPlayerChatMessage(chatContext.Author,

                            chatContext.MessageText, chatContext.IsTeamMessage);
                        await _server.Chat.SendChatMessageAsync(formatted, ((IEnumerable<IPlayer>)chatContext.Recipients).ToArray());
                    });
                }
            });
            
            await pipelineChain(pipelineContext);
            
            _logger.LogInformation("[{Name}]: {Msg}", player.StrippedNickName, pipelineContext.MessageText);
        }
        catch (PlayerNotFoundException ex)
        {
            _logger.LogError(ex, "Failed to get online player: {PlayerId}", ex.AccountId);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to route chat message");
            throw;
        }
    }
}
