using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util;
using GbxRemoteNet;

namespace EvoSC.Manialinks.Vdom.Patch;

/// <summary>
/// Sends a patch by writing its JSON into a "for LocalUser" ManiaScript variable from a small,
/// disposable page -- the exact mechanism M0's spike (<c>src/Modules/VdomSpikeModule</c>)
/// verified in-game: re-sending this page does not disturb the separately-mounted view page's
/// control tree or running script.
/// </summary>
public sealed class LocalUserPatchTransport(IServerClient server) : IPatchTransport
{
    public Task SendAsync(IEnumerable<IPlayer> players, string viewName, string patchJson)
    {
        var xml = BuildPatchPageXml(viewName, patchJson);
        var multiCall = new MultiCall();

        foreach (var player in players)
        {
            multiCall.Add(nameof(server.Remote.SendDisplayManialinkPageToLoginAsync), player.GetLogin(), xml, 0,
                false);
        }

        return server.Remote.MultiCallAsync(multiCall);
    }

    private static string BuildPatchPageXml(string viewName, string patchJson)
    {
        var pageId = VdomNaming.PatchPageId(viewName);
        var variableName = VdomNaming.PatchVariableName(viewName);
        var escapedJson = EscapeManiaScriptString(patchJson);

        // Double-dollar raw string: interpolation holes need double braces ({{expr}}), so
        // ManiaScript's own single braces pass through as literal text unescaped -- see
        // VdomSpikeManialinks.cs in the M0 spike, where this same approach was proven out.
        return $$"""
            <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
            <manialink version="3" id="{{pageId}}">
            <script><!--
            main() {
                declare Text {{variableName}} for LocalUser;
                {{variableName}} = "{{escapedJson}}";
            }
            --></script>
            </manialink>
            """;
    }

    private static string EscapeManiaScriptString(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");
}
