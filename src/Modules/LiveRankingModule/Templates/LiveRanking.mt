<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <using namespace="System.Linq"/>

    <import component="LiveRankingModule.Components.PlayerScore" as="PlayerScore"/>

    <property type="List<LiveRankingWidgetPosition>" name="previousRankings"/>
    <property type="List<LiveRankingWidgetPosition>" name="rankingsExisting"/>
    <property type="List<LiveRankingWidgetPosition>" name="rankingsNew"/>
    
    <template>
        <frame id="live_rankings" pos="-160 82">
            <frame>
                <!-- BACKGROUND -->
                <quad pos="-0.2 -6.8"
                      size="74.6 29"
                      bgcolor="24262f"
                      opacity="0.9"
                />
                
                <!-- HEADER -->
                <quad pos="32 -3.5"
                      size="7 85"
                      valign="center"
                      halign="center"
                      style="UICommon64_1"
                      substyle="BgFrame1"
                      colorize="4357ea"
                      rot="90"
                />
                <quad pos="31 0"
                      size="63 6.8"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="161a35"
                      halign="center"
                />
                <label pos="2 -3.5"
                       size="20 5"
                       text="Live Ranking"
                       valign="center2"
                       textfont="GameFontExtraBold"
                       textprefix="$i$t"
                       textsize="2"
                />
            </frame>

            <!-- CONTENT -->
            <frame id="players" pos="0 -8.5" size="80 27" z-index="10">
                <PlayerScore foreach="LiveRankingWidgetPosition position in previousRankings"
                             y="{{ (position.position - 1) * -7.5 }}"
                             ranking="{{ position }}"
                             newRanking="{{ rankingsExisting.FirstOrDefault(r => r.player.AccountId == position.player.AccountId) }}"
                />
                <PlayerScore foreach="LiveRankingWidgetPosition position in rankingsNew"
                             y="{{ (position.position - 1) * -7.5 }}"
                             ranking="{{ position }}"
                />
            </frame>
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
