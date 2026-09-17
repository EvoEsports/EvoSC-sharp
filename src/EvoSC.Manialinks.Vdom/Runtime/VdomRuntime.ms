<!--
Static, hand-written client runtime for one reactive view's mount page (see the plan's L5 --
this is not generated). Resolves the view's pooled controls once, then applies patches read from
a "for LocalUser" variable written by the small, disposable patch page
(Patch/LocalUserPatchTransport.cs) -- confirmed working cross-page in the M0 spike
(src/Modules/VdomSpikeModule).

Tokens are text-substituted per view before this is embedded (see
Runtime/RuntimeScriptBuilder.cs) -- everything else on this page is identical across every
reactive view:
  {{QuadCount}}, {{LabelCount}}, {{EntryCount}}  -- this view's total pool size per Kind
  {{PatchSeqVariableName}}                       -- "for LocalUser" Integer: latest patch's seq
  {{PatchOpsVariableName}}                       -- "for LocalUser" Text: latest patch's ops JSON
  {{AckRoutePrefix}}                             -- TriggerPageAction route prefix for acks

The seq and ops travel in two separate LocalUser variables, not one combined
"{Seq, Ops}" object -- deliberately: ManiaScript's .fromjson() is documented to support "the JSON
root is an array" when called on an array-typed variable directly, but a struct field that is
itself an array of structs is an unverified extension of that (and M0's spike never exercised
.fromjson() at all, only plain Text/Integer LocalUser variables). Two flat variables of
directly-supported shapes were substituted in after the combined-object version was confirmed
in-game to silently fail (patch computed and sent correctly server-side; client never applied it
or acked -- consistent with .fromjson() failing on the nested shape). See the plan's M1 notes.

Wire format (must match PatchSerializer.cs exactly -- field names are case-sensitive):
  #Struct EvoSC_Vdom_Op { Integer S; Integer K; Integer P; Text V; }
The ops JSON is a plain array of these: [{...},{...}]. A Free is a Set-shaped entry with P == -1
(PatchSerializer.FreePropSentinel) and an empty V, so there is exactly one array to iterate
rather than two struct types to keep in sync.

VKind (K) and VPropId (P) numeric values are pinned in VTypes.cs / VProps.cs -- see the comments
there. If those files are ever renumbered, this file's dispatch below must be updated to match;
WireContractTests.cs exists specifically to catch such a renumbering on the C# side, but nothing
catches a mismatch here, since this file cannot be unit tested from C#.

Vec2/Vec3 values arrive as space-joined Text ("x y" / "r g b"), not native JSON vectors -- decoded
below via TextLib::RegexReplace + TextLib::ToReal rather than an assumed Split() function, since
neither a Split() nor a specific Vec2/Vec3 JSON shape could be confirmed against this ManiaScript
version. RegexReplace and ToReal/ToInteger are both directly proven elsewhere in this codebase
(e.g. Scoreboard.ms, Alert.ms).

Everything below this point is UNVERIFIED beyond matching proven syntax patterns elsewhere in
this repo -- it has not been run in-game. Treat the whole file as the next thing to spike, the
same way src/Modules/VdomSpikeModule verified the transport.

#Const C_QuadCount {{QuadCount}}
#Const C_LabelCount {{LabelCount}}
#Const C_EntryCount {{EntryCount}}

#Struct EvoSC_Vdom_Op {
    Integer S;
    Integer K;
    Integer P;
    Text V;
}

Vec2 __VdomParseVec2(Text V) {
    declare Text XPart = TextLib::RegexReplace(" .*$", V, "", "");
    declare Text YPart = TextLib::RegexReplace("^[^ ]* ", V, "", "");
    return <TextLib::ToReal(XPart), TextLib::ToReal(YPart)>;
}

Vec3 __VdomParseVec3(Text V) {
    declare Text RPart = TextLib::RegexReplace(" .*$", V, "", "");
    declare Text Rest = TextLib::RegexReplace("^[^ ]* ", V, "", "");
    declare Text GPart = TextLib::RegexReplace(" .*$", Rest, "", "");
    declare Text BPart = TextLib::RegexReplace("^[^ ]* ", Rest, "", "");
    return <TextLib::ToReal(RPart), TextLib::ToReal(GPart), TextLib::ToReal(BPart)>;
}

// Props common to every CMlControl (see VTypes.cs's VPropId 0-5). Returns True if P was one of
// these, so callers know whether to fall through to their own kind-specific props.
Boolean __VdomApplyCommon(CMlControl Ctrl, Integer P, Text V) {
    if (P == 0) {
        Ctrl.RelativePosition_V3 = __VdomParseVec2(V);
        return True;
    }
    if (P == 1) {
        Ctrl.Size = __VdomParseVec2(V);
        return True;
    }
    if (P == 2) {
        Ctrl.ZIndex = TextLib::ToReal(V);
        return True;
    }
    if (P == 3) {
        if (V == "1") {
            Ctrl.Show();
        } else {
            Ctrl.Hide();
        }
        return True;
    }
    if (P == 4) {
        Ctrl.RelativeScale = TextLib::ToReal(V);
        return True;
    }
    if (P == 5) {
        Ctrl.RelativeRotation = TextLib::ToReal(V);
        return True;
    }
    // 6 (HAlign) and 7 (VAlign) are deliberately unhandled -- mount-only, see VProps.cs.
    // 8 (ToolTip) is deliberately unhandled -- not yet wired up, see VProps.cs.
    return False;
}

Void __VdomApplyQuad(CMlQuad Ctrl, Integer P, Text V) {
    if (__VdomApplyCommon(Ctrl, P, V)) {
        return;
    }

    if (P == 9) {
        Ctrl.BgColor = __VdomParseVec3(V);
    } else if (P == 10) {
        Ctrl.Opacity = TextLib::ToReal(V);
    } else if (P == 11) {
        Ctrl.ImageUrl = V;
    }
}

Void __VdomApplyLabel(CMlLabel Ctrl, Integer P, Text V) {
    if (__VdomApplyCommon(Ctrl, P, V)) {
        return;
    }

    if (P == 10) {
        Ctrl.Opacity = TextLib::ToReal(V);
    } else if (P == 12) {
        Ctrl.Value = V;
    } else if (P == 13) {
        Ctrl.TextColor = __VdomParseVec3(V);
    } else if (P == 14) {
        Ctrl.TextSizeReal = TextLib::ToReal(V);
    } else if (P == 15) {
        Ctrl.TextFont = V;
    }
}

// Untested -- no M1 view uses an Entry. Mirrors Label; CMlEntry's exact field set beyond Value
// was not independently verified.
Void __VdomApplyEntry(CMlEntry Ctrl, Integer P, Text V) {
    if (__VdomApplyCommon(Ctrl, P, V)) {
        return;
    }

    if (P == 12) {
        Ctrl.Value = V;
    }
}

main() {
    declare CMlQuad[] __VdomQuads = [];
    declare CMlLabel[] __VdomLabels = [];
    declare CMlEntry[] __VdomEntries = [];

    for (I, 0, C_QuadCount - 1) {
        __VdomQuads.add((Page.GetFirstChild("q" ^ I) as CMlQuad));
    }
    for (I, 0, C_LabelCount - 1) {
        __VdomLabels.add((Page.GetFirstChild("l" ^ I) as CMlLabel));
    }
    for (I, 0, C_EntryCount - 1) {
        __VdomEntries.add((Page.GetFirstChild("e" ^ I) as CMlEntry));
    }

    declare Integer __VdomLastAppliedSeq = 0;
    declare Integer {{PatchSeqVariableName}} for LocalUser;
    declare Text {{PatchOpsVariableName}} for LocalUser;

    while (True) {
        yield;

        if ({{PatchSeqVariableName}} > __VdomLastAppliedSeq) {
            declare EvoSC_Vdom_Op[] __VdomOps;
            declare Boolean __VdomOk = __VdomOps.fromjson({{PatchOpsVariableName}});

            if (__VdomOk) {
                foreach (__VdomOp in __VdomOps) {
                    if (__VdomOp.K == 0) {
                        if (__VdomOp.P == -1) {
                            __VdomQuads[__VdomOp.S].Hide();
                        } else {
                            __VdomApplyQuad(__VdomQuads[__VdomOp.S], __VdomOp.P, __VdomOp.V);
                        }
                    } else if (__VdomOp.K == 1) {
                        if (__VdomOp.P == -1) {
                            __VdomLabels[__VdomOp.S].Hide();
                        } else {
                            __VdomApplyLabel(__VdomLabels[__VdomOp.S], __VdomOp.P, __VdomOp.V);
                        }
                    } else if (__VdomOp.K == 2) {
                        if (__VdomOp.P == -1) {
                            __VdomEntries[__VdomOp.S].Hide();
                        } else {
                            __VdomApplyEntry(__VdomEntries[__VdomOp.S], __VdomOp.P, __VdomOp.V);
                        }
                    }
                }

                __VdomLastAppliedSeq = {{PatchSeqVariableName}};
                TriggerPageAction("{{AckRoutePrefix}}" ^ __VdomLastAppliedSeq);
            }
        }
    }
}
-->
