<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <using namespace="System.Linq" />
    
    <import component="LiveRankingModule.Components.PlayerScore" as="PlayerScore"/>

    <property type="List<LiveRankingWidgetPosition>" name="previousRankings"/>
    <property type="List<LiveRankingWidgetPosition>" name="rankingsExisting"/>
    <property type="List<LiveRankingWidgetPosition>" name="rankingsNew"/>

    <template>
        <frame id="live_rankings" pos="-160 82">
            <frame size="85 6">
                <quad pos="32 -3" size="6.2 85" valign="center" halign="center"
                      style="UICommon64_1" substyle="BgFrame1" colorize="4357ea" rot="90"/>
                <quad size="63 6"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      opacity="1" modulatecolor="161a35" pos="31 0" halign="center"/>
                <label pos="8 -3" size="20 5" text="Live Ranking" valign="center2"
                       textfont="GameFontExtraBold" textprefix="$i$t" textsize="2"/>
            </frame>

            <frame id="players" pos="1.5 -7" size="80 27" z-index="10">
                <PlayerScore foreach="LiveRankingWidgetPosition position in previousRankings"
                             y="{{ (position.position - 1) * -7 }}"
                             ranking="{{ position }}"
                             newRanking="{{ rankingsExisting.FirstOrDefault(r => r.player.AccountId == position.player.AccountId) }}"
                             type="existing"
                />
                <PlayerScore foreach="LiveRankingWidgetPosition position in rankingsNew"
                             y="{{ (position.position - 1) * -7 }}"
                             ranking="{{ position }}"
                             type="new     "
                />
            </frame>

            <quad pos="-0.2 -5.5" z-index="0" size="74.6 29" bgcolor="24262f" opacity="0.9"/>
        </frame>
    </template>

    <script>
        <!--
            main() {
                +++ Animations +++
            }
        -->
    </script>
</component>
