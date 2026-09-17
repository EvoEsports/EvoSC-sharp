using System.Text.Json;
using EvoSC.Manialinks.Vdom.Patch;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Patch;

public class PatchSerializerTests
{
    [Fact]
    public void Serializes_ops_as_a_plain_json_array_with_field_names_matching_the_ManiaScript_struct()
    {
        var json = PatchSerializer.SerializeOps(new VPatchOp[]
        {
            new VPatchOp.Set(3, VKind.Quad, VPropId.BgColor, VColor.FromHex("FF0000"))
        });

        using var doc = JsonDocument.Parse(json);

        // The root is the array itself -- not an object wrapping "Ops"/"Seq" -- matching the one
        // JSON shape ManiaScript's .fromjson() is documented to support (see PatchSerializer's
        // class docs on why the combined-object shape was abandoned).
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);

        var op = doc.RootElement[0];
        Assert.Equal(3, op.GetProperty("S").GetInt32());
        Assert.Equal((int)VKind.Quad, op.GetProperty("K").GetInt32());
        Assert.Equal((int)VPropId.BgColor, op.GetProperty("P").GetInt32());
        Assert.Equal("1 0 0", op.GetProperty("V").GetString());
    }

    [Fact]
    public void Serializes_a_free_op_as_a_set_shaped_entry_with_the_sentinel_prop()
    {
        var json = PatchSerializer.SerializeOps(new VPatchOp[] { new VPatchOp.Free(5, VKind.Label, false) });

        using var doc = JsonDocument.Parse(json);
        var op = doc.RootElement[0];

        Assert.Equal(5, op.GetProperty("S").GetInt32());
        Assert.Equal((int)VKind.Label, op.GetProperty("K").GetInt32());
        Assert.Equal(PatchSerializer.FreePropSentinel, op.GetProperty("P").GetInt32());
        Assert.Equal("", op.GetProperty("V").GetString());
    }

    [Fact]
    public void Alloc_ops_produce_no_wire_entry_of_their_own()
    {
        var json = PatchSerializer.SerializeOps(new VPatchOp[]
        {
            new VPatchOp.Alloc(0, VKind.Quad, false),
            new VPatchOp.Set(0, VKind.Quad, VPropId.Opacity, 0.5)
        });

        using var doc = JsonDocument.Parse(json);
        Assert.Equal(1, doc.RootElement.GetArrayLength());
    }

    [Theory]
    [InlineData(VPropId.Visible, true, "1")]
    [InlineData(VPropId.Visible, false, "0")]
    [InlineData(VPropId.Opacity, 0.5, "0.5")]
    [InlineData(VPropId.ZIndex, 3.0, "3")]
    public void Formats_non_string_values_as_the_expected_text(VPropId prop, object value, string expected)
    {
        var json = PatchSerializer.SerializeOps(new VPatchOp[] { new VPatchOp.Set(0, VKind.Label, prop, value) });

        using var doc = JsonDocument.Parse(json);
        Assert.Equal(expected, doc.RootElement[0].GetProperty("V").GetString());
    }
}
