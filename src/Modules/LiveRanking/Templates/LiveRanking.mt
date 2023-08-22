<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models" />
    <using namespace="EvoSC.Common.Interfaces.Models" />
    <property type="List<LiveRankingWidgetPosition>" name="liverankings" />
    
    <template>
        <frame id="live_rankings" pos="-160 82">
            <frame size="85 6">
                <label pos="8 -3" z-index="5" size="20 5" text="Live Ranking" valign="center2" textfont="GameFontExtraBold" textprefix="$i$t" textsize="2"/>
                <quad z-index="1" pos="32 -3" size="6.2 85" valign="center"  halign="center" style="UICommon64_1" substyle="BgFrame1" colorize="4357ea"  rot="90"/>
                <quad z-index="2" size="63 6" image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga" opacity="1" modulatecolor="161a35" pos="31 0" halign="center"/>
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