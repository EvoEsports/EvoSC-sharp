using System.Diagnostics;
using System.Text;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Util;

namespace EvoSC.Modules.Official.VdomSpikeModule.Controllers;

/// <summary>
/// Chat commands driving the M0 VDOM transport spike. See <see cref="VdomSpikeManialinks"/> for
/// what each page proves. Everything targets the invoking player only.
/// </summary>
[Controller]
public class VdomSpikeCommandsController(IVdomSpikeState state) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("vdomspikemount", "VDOM spike: mount the view page (step 1 of the spike).")]
    public async Task MountAsync()
    {
        var login = Context.Player.GetLogin();
        var xml = VdomSpikeManialinks.BuildViewPage();

        await Context.Server.Remote.SendDisplayManialinkPageToLoginAsync(login, xml, 0, false);
        await Context.Chat.SuccessMessageAsync(
            "View page mounted. Its 'Ticks' counter should now be climbing. Try "
            + "'//vdomspikepatch hello' next, then hover the gray box and patch again -- "
            + "the tick count and hover count should both keep climbing through every patch.");
    }

    [ChatCommand("vdomspikepatch", "VDOM spike: send a single patch with the given text value.")]
    public async Task PatchAsync(string value)
    {
        var login = Context.Player.GetLogin();
        var seq = state.NextSeq();
        var xml = VdomSpikeManialinks.BuildPatchPage(value, seq);

        var sw = Stopwatch.StartNew();
        await Context.Server.Remote.SendDisplayManialinkPageToLoginAsync(login, xml, 0, false);
        sw.Stop();

        await Context.Chat.SuccessMessageAsync(
            $"Sent patch seq {seq} = \"{value}\" ({xml.Length} bytes, "
            + $"send call took {sw.Elapsed.TotalMilliseconds:0.0}ms). Check the view page's "
            + "'Patch:' label and log.");
    }

    [ChatCommand("vdomspikeburst", "VDOM spike: send N patches back-to-back with no delay, to probe drops.")]
    public async Task BurstAsync(int count)
    {
        var login = Context.Player.GetLogin();
        var firstSeq = state.NextSeq();
        var sentSeqs = new List<int> { firstSeq };

        var sw = Stopwatch.StartNew();
        await Context.Server.Remote.SendDisplayManialinkPageToLoginAsync(login,
            VdomSpikeManialinks.BuildPatchPage("burst-0", firstSeq), 0, false);

        for (var i = 1; i < count; i++)
        {
            var seq = state.NextSeq();
            sentSeqs.Add(seq);
            await Context.Server.Remote.SendDisplayManialinkPageToLoginAsync(login,
                VdomSpikeManialinks.BuildPatchPage($"burst-{i}", seq), 0, false);
        }

        sw.Stop();

        await Context.Chat.SuccessMessageAsync(
            $"Sent {count} patches back-to-back (seqs {sentSeqs[0]}-{sentSeqs[^1]}) in "
            + $"{sw.Elapsed.TotalMilliseconds:0.0}ms total. Check the view page's log: does it "
            + "show every seq, or only the last one it happened to read on a tick? Either is "
            + "fine for the design (patches are meant to be cumulative-since-ack) -- we're just "
            + "confirming the LAST seq sent always eventually shows up.");
    }

    [ChatCommand("vdomspikepool", "VDOM spike: mount N pooled controls (scriptevents on) to measure mount cost.")]
    public Task PoolAsync(int count) => SendPoolAsync(count, scriptEvents: true);

    [ChatCommand("vdomspikepoolflat",
        "VDOM spike: mount N pooled controls with scriptevents OFF, to isolate mount/render cost from event-volume cost.")]
    public Task PoolFlatAsync(int count) => SendPoolAsync(count, scriptEvents: false);

    private async Task SendPoolAsync(int count, bool scriptEvents)
    {
        var login = Context.Player.GetLogin();

        var buildSw = Stopwatch.StartNew();
        var xml = VdomSpikeManialinks.BuildPoolPage(count, scriptEvents);
        buildSw.Stop();

        var sendSw = Stopwatch.StartNew();
        await Context.Server.Remote.SendDisplayManialinkPageToLoginAsync(login, xml, 0, false);
        sendSw.Stop();

        var bytes = Encoding.UTF8.GetByteCount(xml);
        var variant = scriptEvents ? "scriptevents ON" : "scriptevents OFF";

        await Context.Chat.SuccessMessageAsync(
            $"Pool of {count} controls ({variant}) mounted: {bytes:N0} bytes, "
            + $"build {buildSw.Elapsed.TotalMilliseconds:0.0}ms, "
            + $"send call {sendSw.Elapsed.TotalMilliseconds:0.0}ms. "
            + "Move your mouse across the grid and watch for visible hitching, and check the "
            + "on-page event counter.");
    }

    [ChatCommand("vdomspikeclear", "VDOM spike: hide all spike test pages.")]
    public async Task ClearAsync()
    {
        var login = Context.Player.GetLogin();

        foreach (var name in new[]
                 {
                     VdomSpikeManialinks.ViewPageName, VdomSpikeManialinks.PatchPageName,
                     VdomSpikeManialinks.PoolPageName
                 })
        {
            await Context.Server.Remote.SendDisplayManialinkPageToLoginAsync(login,
                ManialinkUtils.CreateHideManialink(name), 3, true);
        }

        await Context.Chat.SuccessMessageAsync("VDOM spike pages cleared.");
    }
}
