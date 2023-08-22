<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <property type="MatchInfo" name="data"/>
    <template>
        <frame id="match_ranking" pos="85 82">
            <frame size="75.3 6" pos="-1 0">
                <label pos="10 -3" z-index="5" size="20 5" text="Match Ranking" valign="center2" textfont="GameFontExtraBold" textprefix="$i$t" textsize="2"/>
                <quad z-index="1" pos="37.7 -3" size="74.5 6" valign="center"  halign="center" style="UICommon64_1" substyle="BgFrame1" colorize="4357ea"/>
                <quad z-index="2" size="76.2 6" image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga" opacity="1" modulatecolor="161a35" pos="38.6 -3" halign="center" valign="center" rot="180"/>
            </frame>

            <frame id="players">
                <frameinstance modelid="player" pos="1.5 -8 "/>
                <frameinstance modelid="player" pos="1.5 -14 "/>
                <frameinstance modelid="player" pos="1.5 -20 "/>
                <frameinstance modelid="player" pos="1.5 -26 "/>
            </frame>
            <quad pos="-0.2 -5.5" z-index="0" size="74.6 28.3" bgcolor="24262f" opacity="0.9"/>
        </frame>
    </template>
</component>