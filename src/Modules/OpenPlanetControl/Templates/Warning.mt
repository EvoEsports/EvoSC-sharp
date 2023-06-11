<component>
    <property type="string" name="Mode"/>
    <property type="string" name="AllowedModeText"/>
    <property type="int" name="KickTimeout"/>
    
    <template>
        <frame pos="0 30">
            <frame>
                <label pos="0 0.3" z-index="0" size="30 30" text="" textsize="30" valign="center2" halign="center" rot="-90" textcolor="fff"/>
                <label pos="0 0" z-index="1" size="30 30" text="" textsize="28" valign="center2" halign="center" rot="-90" textcolor="f05"/>
                <label pos="-0.5 0" z-index="2" size="20 20" halign="center" valign="center" textsize="20"  textcolor="fff"  text="!" textfont="GameFontSemiBold"/>
            </frame>
            <frame pos="0 -20">
                <label pos="0 0" z-index="0" size="170 8" text="$tIncompatible signature mode detected!" textfont="GameFontBlack" textemboss="1" halign="center" valign="top" textsize="4" />
                <label pos="0 -7" z-index="0" size="320 5.28" text="Your Openplanet uses $9df{{Mode}} $fffSignature mode, which currently is $f00$o$tnot allowed$t$o$fff with this server." textfont="GameFontRegular" textemboss="1" halign="center" valign="top" textsize="3" /><label pos="0 -13" z-index="0" size="320 5.28" text="Currently $0d0allowed $fffmodes: $9df{{AllowedModeText}}" textfont="GameFontRegular" textemboss="1" halign="center" valign="top" textsize="3" />
            </frame>
            <frame pos="0 -40">
                <label textsize="2" halign="center" pos="0 -4" textprefix="$fff" text="How to switch signature mode?" />
                <label textsize="1" halign="center" pos="0 -12" textprefix="$fff" text="In the Openplanet menu (if not shown, press f3)" />
                <label textsize="1" halign="center" pos="0 -16" textprefix="$fff" text="click $9dfDeveloper$g then $9dfSignature Mode$g" />
                <quad bgcolor="fff" size="125 22" pos="0 -11.5" opacity="1" halign="center" valign="center" z-index="-2" />
                <quad bgcolor="7E002AFF" size="124.3 21.5" pos="0 -11.5" opacity="1" halign="center" valign="center" z-index="-1" />
            </frame>
            <frame pos="0 -70">
                <label pos="0 0" z-index="0" size="200 5" text="Infotext" halign="center" valign="center" id="countdowntext" textfont="GameFontSemiBold"/><label pos="0 -5" z-index="0" size="200 5" text="Change signature mode now to cancel kick." halign="center" valign="center" id="countdowntext" textfont="GameFontRegular"/>
            </frame>
            <quad pos="0 15" z-index="-10" size="240 104" opacity="0.7" halign="center" style="UICommon64_1" substyle="BgFrame1" colorize="000"/>
        </frame>
        <script><!--
    #Include "MathLib" as ML
    #Include "TextLib" as TL

    Void updateCountdownText(Integer value) {
        declare countdownText <=> (Page.MainFrame.GetFirstChild("countdowntext") as CMlLabel);
        declare plural = "s";
        if (value == 1) {
            plural = "";
        }

        if (value < 1) {
            countdownText.SetText("You are about to be kicked.");
            TriggerPageAction("OpenPlanetControl/Kick");
        } else {
            countdownText.SetText("You will be automatically kicked in " ^ value ^ " second" ^ plural ^ ".");
        }
    }

    main() {
        declare Integer countdown = 30;
        declare lastUpdate = Now;

        updateCountdownText(countdown);

        while(True) {
            yield;

            if (Now - lastUpdate > 1000 && countdown > 0) {
                countdown -= 1;
                lastUpdate = Now;
                updateCountdownText(countdown);
            }
        }
    }
--></script>
    </template>
</component>