namespace EvoSC.Manialinks.Vdom;

/// <summary>
/// The page-id / ManiaScript-variable naming conventions shared by <c>Mount/</c> (which writes
/// the view page), <c>Patch/</c> (which writes and sends the patch page) and
/// <c>Runtime/VdomRuntime.ms</c> (which reads the LocalUser variable) -- centralized so the three
/// can never drift out of sync with each other.
/// </summary>
public static class VdomNaming
{
    /// <summary>
    /// The mounted view's own manialink page id. Sent once at mount and never re-sent -- see the
    /// plan's M0 findings on why re-sending it would destroy its control tree and script state.
    /// </summary>
    public static string ViewPageId(string viewName) => $"EvoSC_View_{viewName}";

    /// <summary>
    /// The small, disposable page re-sent on every patch. Its only job is to write
    /// <see cref="PatchVariableName"/> "for LocalUser" and exit -- see
    /// <c>Patch/LocalUserPatchTransport</c>.
    /// </summary>
    public static string PatchPageId(string viewName) => $"EvoSC_View_{viewName}__patch";

    /// <summary>
    /// The ManiaScript "for LocalUser" variable the patch page writes and the view page's
    /// running script loop reads every tick -- confirmed working cross-page in M0.
    /// </summary>
    public static string PatchVariableName(string viewName) => $"EvoSC_Vdom_Patch_{viewName}";
}
