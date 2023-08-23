<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models" />
    <import component="LiveRankingModule.Components.PlayerScore" as="PlayerScore" />

    <property type="List<LiveRankingWidgetPosition>" name="previousRankings" />
    <property type="List<LiveRankingWidgetPosition>" name="rankingsExisting" />
    <property type="List<LiveRankingWidgetPosition>" name="rankingsNew" />

    <template>
        <frame id="live_rankings" pos="-160 82">
            <frame size="85 6">
                <label pos="8 -3" z-index="5" size="20 5" text="Live Ranking" valign="center2"
                    textfont="GameFontExtraBold" textprefix="$i$t" textsize="2" />
                <quad z-index="1" pos="32 -3" size="6.2 85" valign="center" halign="center"
                    style="UICommon64_1" substyle="BgFrame1" colorize="4357ea" rot="90" />
                <quad z-index="2" size="63 6"
                    image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                    opacity="1" modulatecolor="161a35" pos="31 0" halign="center" />
            </frame>

            <frame id="players" pos="1.5 -7" size="80 27">
                <PlayerScore foreach="LiveRankingWidgetPosition position in previousRankings"
                    id="player_row_{{ position.player.AccountId }}"
                    y="{{ position.position * -7 }}"
                    playerName="{{ position.player.NickName }}"
                    score="{{ position.time }}"
                />
                <PlayerScore foreach="LiveRankingWidgetPosition position in rankingsNew"
                    y="{{ position.position * -7 }}"
                    playerName="{{ position.player.NickName }}"
                    score="{{ position.time }}"
                    hidden="1"
                />
            </frame>

            <frame foreach="LiveRankingWidgetPosition position in rankingsExisting">
                <script>
                    <!--
                        *** Animations ***
                        ***
                        declare playerRow <=> (playersFrame.GetFirstChild("player_row_{{ position.player.AccountId }}") as CMlFrame);
                        AnimMgr.Add(playerRow, "<frame pos='0 {{ position.position * -7 }}' />", 320, CAnimManager::EAnimManagerEasing::QuadOut);
                        ***
                    -->
                </script>
            </frame>

            <quad pos="-0.2 -5.5" z-index="0" size="74.6 29" bgcolor="24262f" opacity="0.9" />
        </frame>
    </template>
    <script>
        <!--
            main() {
                declare CMlFrame playersFrame <=> (Page.MainFrame.GetFirstChild("players") as CMlFrame);
                { +++ Animations +++ }
            }
        -->
    </script>
</component>
