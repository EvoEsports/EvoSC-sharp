using System.Globalization;
using System.Text;
using System.Text.Json;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Patch;

/// <summary>
/// Serializes <see cref="VPatchOp"/>s to the JSON <c>Runtime/VdomRuntime.ms</c> decodes via
/// ManiaScript's <c>.fromjson()</c>, matching these declared structs exactly (field names are
/// case-sensitive on the ManiaScript side):
///
/// <code>
/// #Struct EvoSC_Vdom_Op    { Integer S; Integer K; Integer P; Text V; }
/// #Struct EvoSC_Vdom_Patch { Integer Seq; EvoSC_Vdom_Op[] Ops; }
/// </code>
///
/// There is deliberately no separate "Free" array (the plan's L4 sketch has one) -- a Free is
/// just a <see cref="VPatchOp.Set"/>-shaped entry with <see cref="FreePropSentinel"/> as its
/// prop code and an empty value, so the client has exactly one homogeneous array to iterate
/// instead of two struct types to keep in sync.
///
/// Every value is serialized to <b>Text</b>, whatever its C# type -- colors as "r g b" reals
/// (see <see cref="VColor.ToString"/>), vectors as "x y" (<see cref="VVec2.ToString"/>), enums as
/// their pinned ordinal, everything else via <see cref="IFormattable"/> or a raw string -- so the
/// client's dispatch is "look up Kind+Prop, parse V accordingly", never a type union on the wire
/// itself.
/// </summary>
public static class PatchSerializer
{
    /// <summary>
    /// The prop code used for a Free entry -- guaranteed to never collide with a real
    /// <see cref="VPropId"/>, whose pinned values are all non-negative (see the wire-contract
    /// note on <see cref="VPropId"/>).
    /// </summary>
    public const int FreePropSentinel = -1;

    /// <param name="seq">
    /// The highest diff sequence number included in this patch -- the client echoes this back in
    /// its ack (see <see cref="IPatchTransport"/>), so it must be monotonic and identify exactly
    /// how much of the backlog this patch covers.
    /// </param>
    /// <param name="ops">
    /// Every op to include, already flattened across however many diffs this patch is catching
    /// the client up on (see the "cumulative since last ack" note in the plan's L4).
    /// </param>
    public static string Serialize(int seq, IEnumerable<VPatchOp> ops)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteNumber("Seq", seq);
            writer.WriteStartArray("Ops");

            foreach (var op in ops)
            {
                WriteOp(writer, op);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void WriteOp(Utf8JsonWriter writer, VPatchOp op)
    {
        switch (op)
        {
            case VPatchOp.Set set:
                writer.WriteStartObject();
                writer.WriteNumber("S", set.Slot);
                writer.WriteNumber("K", (int)set.Kind);
                writer.WriteNumber("P", (int)set.Prop);
                writer.WriteString("V", FormatValue(set.Prop, set.Value));
                writer.WriteEndObject();
                break;

            case VPatchOp.Free free:
                writer.WriteStartObject();
                writer.WriteNumber("S", free.Slot);
                writer.WriteNumber("K", (int)free.Kind);
                writer.WriteNumber("P", FreePropSentinel);
                writer.WriteString("V", "");
                writer.WriteEndObject();
                break;

            case VPatchOp.Alloc:
                // Alloc carries no state of its own -- the Set ops for the newly-claimed slot's
                // full initial props (see VProps.AllSet) are what the client actually needs, and
                // they're already included in the same ops list. See the plan's L4 note on why
                // Alloc has no wire representation.
                break;
        }
    }

    private static string FormatValue(VPropId prop, object value) => prop switch
    {
        VPropId.Position or VPropId.Size => value.ToString()!, // VVec2.ToString() -> "x y"
        VPropId.BgColor or VPropId.TextColor => value.ToString()!, // VColor.ToString() -> "r g b"
        VPropId.Visible => (bool)value ? "1" : "0",
        VPropId.ZIndex or VPropId.RelativeScale or VPropId.RelativeRotation or VPropId.Opacity or VPropId.TextSize =>
            ((double)value).ToString("0.###", CultureInfo.InvariantCulture),
        VPropId.ToolTip or VPropId.ImageUrl or VPropId.Text or VPropId.TextFont => (string)value,
        _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
    };
}
