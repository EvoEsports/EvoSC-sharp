namespace EvoSC.Manialinks.Vdom;

/// <summary>
/// The page-id / ManiaScript-variable naming conventions shared by <c>Mount/</c> (which writes
/// the view page), <c>Patch/</c> (which writes and sends the patch page) and
/// <c>Runtime/VdomRuntime.ms</c> (which reads the LocalUser variables) -- centralized so the
/// three can never drift out of sync with each other.
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
    /// <see cref="PatchSeqVariableName"/>/<see cref="PatchOpsVariableName"/> "for LocalUser" and
    /// exit -- see <c>Patch/LocalUserPatchTransport</c>.
    /// </summary>
    public static string PatchPageId(string viewName) => $"EvoSC_View_{viewName}__patch";

    /// <summary>
    /// The ManiaScript "for LocalUser" Integer variable carrying the latest patch's sequence
    /// number -- confirmed working cross-page in M0 (as a plain Integer, exactly this shape).
    /// Split from the ops payload deliberately: see <see cref="PatchOpsVariableName"/> for why.
    /// </summary>
    public static string PatchSeqVariableName(string viewName) => $"EvoSC_Vdom_Seq_{viewName}";

    /// <summary>
    /// The ManiaScript "for LocalUser" Text variable carrying the latest patch's ops, as a JSON
    /// array (parsed via <c>Ops.fromjson(...)</c> on an array-typed variable directly). Kept
    /// separate from <see cref="PatchSeqVariableName"/> rather than one combined
    /// <c>{Seq, Ops}</c> object: ManiaScript's documented <c>.fromjson()</c> behaviour is "the
    /// JSON root is an array" when called on an array-typed variable -- a struct field that is
    /// itself an array of structs is an unverified extension of that, and M0 never exercised
    /// <c>.fromjson()</c> at all (only plain Text/Integer LocalUser variables). Two flat
    /// variables of directly-supported shapes are safer than one nested one.
    /// </summary>
    public static string PatchOpsVariableName(string viewName) => $"EvoSC_Vdom_Ops_{viewName}";
}
