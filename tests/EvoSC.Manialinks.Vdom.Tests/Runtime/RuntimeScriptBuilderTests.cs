using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Runtime;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Runtime;

public class RuntimeScriptBuilderTests
{
    [Fact]
    public void Loads_the_embedded_template_and_substitutes_every_token()
    {
        var layout = PoolLayout.Compute(new Dictionary<(VKind, bool), int>
        {
            [(VKind.Quad, false)] = 3,
            [(VKind.Quad, true)] = 2,
            [(VKind.Label, false)] = 4
        });

        var script = RuntimeScriptBuilder.Build("LiveRanking", layout);

        Assert.Contains("#Const C_QuadCount 5", script);
        Assert.Contains("#Const C_LabelCount 4", script);
        Assert.Contains("#Const C_EntryCount 0", script);
        Assert.Contains("declare Integer EvoSC_Vdom_Seq_LiveRanking for LocalUser;", script);
        Assert.Contains("declare Text EvoSC_Vdom_Ops_LiveRanking for LocalUser;", script);
        Assert.Contains("TriggerPageAction(\"VdomAck/Ack/LiveRanking/\" ^ __VdomLastAppliedSeq);", script);

        // No unsubstituted tokens should remain.
        Assert.DoesNotContain("{{", script);
        Assert.DoesNotContain("}}", script);
    }
}
