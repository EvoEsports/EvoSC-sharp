<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>

    <property type="double" name="y" default="0.0"/>
    <property type="LiveRankingWidgetPosition?" name="ranking" default="null"/>
    <property type="LiveRankingWidgetPosition?" name="newRanking" default="null"/>

    <property type="string" name="positionColor" default="ddcc00"/>
    <property type="string" name="gradientColor" default="484a5a"/>

    <template>
        <frame id="player_row_{{ ranking.player.AccountId }}" pos="0 {{ y }}">
            <!-- POSITION BOX -->
            <quad pos="-1 0"
                  size="8 6"
                  style="UICommon64_1"
                  substyle="BgFrame2"
                  colorize="{{ positionColor }}"
            />

            <!-- POSITION NUMBER -->
            <label id="position"
                   pos="3.25 -2.5"
                   size="7 5"
                   text="{{ ranking?.position }}"
                   halign="center"
                   valign="center"
                   textfont="GameFontExtraBold"
                   textsize="2"
                   textcolor="000000"
                   textprefix="$i"
            />

            <frame pos="8 0">
                <frame size="1 999">
                    <quad pos="0 0.1"
                          size="2 6.2"
                          style="UICommon64_1"
                          substyle="BgFrame2"
                          colorize="{{ gradientColor }}"
                    />
                </frame>

                <!-- GRADIENT -->
                <quad pos="1 0"
                      size="50 6"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="{{ gradientColor }}"
                />

                <!-- NAME -->
                <label pos="2 -3"
                       size="50 6"
                       text="{{ ranking?.player?.NickName }}"
                       valign="center2"
                       textfont="GameFontSemiBold"
                       textsize="1"
                       textprefix="$i$t"
                />

                <!-- TIME -->
                <label id="score"
                       pos="64 -3"
                       size="15 5"
                       text="{{ ranking?.time }}"
                       valign="center2"
                       halign="right"
                       textfont="GameFontSemiBold"
                       textsize="1"
                       textprefix="$i$t"
                />
            </frame>
        </frame>
    </template>

    <script>
        <!--
            *** Animations ***
            ***
                if({{ newRanking != null ? "True" : "False" }}){
                    declare scoreFrame <=> (Page.MainFrame.GetFirstChild("player_row_{{ ranking?.player.AccountId }}") as CMlFrame);
                    (scoreFrame.GetFirstChild("position") as CMlLabel).Value = "{{ newRanking?.position }}";
                    (scoreFrame.GetFirstChild("score") as CMlLabel).Value = "{{ newRanking?.time }}";
                    AnimMgr.Add(scoreFrame, "<frame pos='0 {{ (newRanking?.position - 1) * -7.5 }}' />", 320, CAnimManager::EAnimManagerEasing::ExpOut);
                }
            ***
        -->
    </script>
</component>
