<component>
    <using namespace="EvoSC.Modules.Official.MatchRankingModule.Models"/>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <using namespace="System.Linq"/>
    
    <import component="LiveRankingModule.Components.PlayerScore" as="PlayerScore"/>

    <property type="List<LiveRankingWidgetPosition>" name="PreviousScores"/>
    <property type="List<LiveRankingWidgetPosition>" name="NewScores"/>
    <property type="List<LiveRankingWidgetPosition>" name="ExistingScores"/>

    <template>
        <frame pos="86 82" halign="right">
            <frame>
                <!-- BACKGROUND -->
                <quad pos="-0.2 -6.8"
                      size="74.6 29"
                      bgcolor="24262f"
                      opacity="0.9"
                />

                <!-- HEADER -->
                <quad pos="42 -3.5"
                      size="85 7"
                      valign="center"
                      halign="center"
                      style="UICommon64_1"
                      substyle="BgFrame1"
                      colorize="4357ea"
                />
                <quad pos="43 -6.8"
                      size="63 6.8"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="161a35"
                      halign="center"
                      rot="180"
                />
                <label pos="72 -3.5"
                       text="Match Ranking"
                       valign="center2"
                       textfont="GameFontExtraBold"
                       textprefix="$i$t"
                       textsize="2"
                       halign="right"
                />
            </frame>

            <!-- CONTENT -->
            <frame id="players" pos="0 -8.5" size="80 27" z-index="10">
                <PlayerScore foreach="LiveRankingWidgetPosition position in PreviousScores"
                             y="{{ (position.position - 1) * -7.5 }}"
                             ranking="{{ position }}"
                             newRanking="{{ ExistingScores.FirstOrDefault(r => r.player.AccountId == position.player.AccountId) }}"
                />
                <PlayerScore foreach="LiveRankingWidgetPosition position in NewScores"
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