using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util;
using GbxRemoteNet;

namespace EvoSC.Manialinks.Vdom.Patch;

/// <summary>
/// Sends a patch by writing its seq and ops into two "for LocalUser" ManiaScript variables from
/// a small, disposable page -- the exact mechanism M0's spike (<c>src/Modules/VdomSpikeModule</c>)
/// verified in-game: re-sending this page does not disturb the separately-mounted view page's
/// control tree or running script. Two variables, not one combined object -- see
/// <see cref="VdomNaming.PatchOpsVariableName"/> for why.
/// </summary>
public sealed class LocalUserPatchTransport(IServerClient server) : IPatchTransport
{
    public Task SendAsync(IEnumerable<IPlayer> players, string viewName, int seq, string opsJson)
    {
        var xml = BuildPatchPageXml(viewName, seq, opsJson);
        var multiCall = new MultiCall();

        foreach (var player in players)
        {
            multiCall.Add(nameof(server.Remote.SendDisplayManialinkPageToLoginAsync), player.GetLogin(), xml, 0,
                false);
        }

        return server.Remote.MultiCallAsync(multiCall);
    }

    private static string BuildPatchPageXml(string viewName, int seq, string opsJson)
    {
        var pageId = VdomNaming.PatchPageId(viewName);
        var seqVariableName = VdomNaming.PatchSeqVariableName(viewName);
        var opsVariableName = VdomNaming.PatchOpsVariableName(viewName);
        var escapedOpsJson = EscapeManiaScriptString(opsJson);

        // Double-dollar raw string: interpolation holes need double braces ({{expr}}), so
        // ManiaScript's own single braces pass through as literal text unescaped -- see
        // VdomSpikeManialinks.cs in the M0 spike, where this same approach was proven out.
        return $$"""
            <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
            <manialink version="3" id="{{pageId}}">
            <script><!--
            main() {
                declare Integer {{seqVariableName}} for LocalUser;
                declare Text {{opsVariableName}} for LocalUser;
                {{opsVariableName}} = "{{escapedOpsJson}}";
                {{seqVariableName}} = {{seq}};
            }
            --></script>
            </manialink>
            """;
    }

    private static string EscapeManiaScriptString(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");
}
