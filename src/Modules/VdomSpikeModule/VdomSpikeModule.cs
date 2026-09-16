using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.VdomSpikeModule;

/// <summary>
/// Throwaway lab module for the M0 VDOM transport spike (see
/// /home/hanslb/.claude/plans/create-a-plan-for-keen-pumpkin.md).
///
/// Bypasses ManiaTemplates entirely and sends hand-written manialink XML directly, to verify
/// in-game:
///   1. A "for LocalUser" value written by one manialink page is readable by another page's
///      script loop.
///   2. Re-sending the small patch page does not disturb the state (script vars, scroll, hover)
///      of the separately-mounted view page.
///   3. Round-trip latency and whether rapid patches can be dropped.
///   4. Mount cost (payload size / visible hitch) at 200 / 1,000 / 2,000 pooled controls.
///
/// Safe to delete once the M0 spike concludes and its findings are folded into the real design.
/// </summary>
[Module(IsInternal = true)]
public class VdomSpikeModule : EvoScModule, IToggleable
{
    public Task EnableAsync() => Task.CompletedTask;

    public Task DisableAsync() => Task.CompletedTask;
}
