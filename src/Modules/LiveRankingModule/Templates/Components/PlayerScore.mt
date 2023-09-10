<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>

    <property type="LiveRankingWidgetPosition?" name="ranking" default="null"/>
    <property type="LiveRankingWidgetPosition?" name="newRanking" default="null"/>

    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w" default="66.0"/>
    <property type="double" name="h" default="6.0"/>
    <property type="double" name="rowSpacing" default="1.0"/>
    <property type="string" name="positionColor" default="ddcc00"/>
    <property type="string" name="playerRowBackgroundColor" default="999999"/>

    <template>
        <frame id="player_row_{{ ranking.player.AccountId }}" pos="0 {{ y }}">
            <!-- POSITION BOX -->
            <quad pos="-1 0"
                  size="8 {{ h }}"
                  style="UICommon64_1"
                  substyle="BgFrame2"
                  colorize="{{ positionColor }}"
            />

            <!-- POSITION NUMBER -->
            <label id="position"
                   pos="3.25 {{ h / -2.0 + 0.25 }}"
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
                          size="2 {{ h + 0.25 }}"
                          style="UICommon64_1"
                          substyle="BgFrame2"
                          colorize="{{ playerRowBackgroundColor }}"
                          opacity="0.75"
                    />
                </frame>

                <!-- GRADIENT -->
                <quad pos="1 0"
                      size="{{ w - 1 }} {{ h }}"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="{{ playerRowBackgroundColor }}"
                      opacity="0.75"
                />

                <!-- NAME -->
                <label pos="2 {{ h / -2.0 }}"
                       size="{{ w * 0.66 }} 6"
                       text="{{ ranking?.player?.NickName }}"
                       valign="center2"
                       textfont="GameFontSemiBold"
                       textsize="1.15"
                       textprefix="$i$t"
                />

                <!-- TIME -->
                <label id="score"
                       pos="{{ w - 2.0 }} {{ h / -2.0 }}"
                       size="15 5"
                       text="{{ ranking?.time }}"
                       valign="center2"
                       halign="right"
                       textfont="GameFontSemiBold"
                       textsize="1.15"
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
                    AnimMgr.Add(scoreFrame, "<frame pos='0 {{ (newRanking?.position - 1) * (rowSpacing + h) * -1.0 }}' />", 320, CAnimManager::EAnimManagerEasing::ExpOut);
                }
            ***
        -->
    </script>
</component>
