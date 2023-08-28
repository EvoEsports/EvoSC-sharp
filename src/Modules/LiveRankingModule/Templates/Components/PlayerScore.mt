<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    
    <property type="double" name="y" default="0.0"/>
    <property type="LiveRankingWidgetPosition?" name="ranking" default="null"/>
    <property type="LiveRankingWidgetPosition?" name="newRanking" default="null"/>

    <template>
        <frame id="player_row_{{ ranking.player.AccountId }}" pos="0 {{ y }}">
            <!-- Background -->
            <quad pos="0 0" 
                  z-index="1" 
                  size="7 5" 
                  style="UICommon64_1" 
                  substyle="BgFrame2" 
                  opacity="1" 
                  colorize="dc0"
            />
            <quad z-index="2" pos="9 0" size="50 5"
                  image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga" 
                  opacity="1"
                  modulatecolor="484a5a"
            />

            <!-- POSITION -->
            <label id="position"
                   pos="3.5 -2.25"
                   z-index="2"
                   size="7 5"
                   text="{{ ranking?.position }}"
                   halign="center"
                   valign="center"
                   textfont="GameFontExtraBold"
                   textsize="2"
                   textcolor="000000FF"
                   textprefix="$i"
            />

            <!-- NAME -->
            <label pos="10 -2.5"
                   z-index="3"
                   size="50 5"
                   text="{{ ranking?.player?.NickName }}"
                   valign="center2"
                   textfont="GameFontSemiBold"
                   textsize="1"
                   textprefix="$i$t"
            />

            <!-- TIME -->
            <label id="score"
                   pos="65 -2.5"
                   z-index="3"
                   size="15 5"
                   text="{{ ranking?.time }}"
                   valign="center2"
                   textfont="GameFontSemiBold"
                   textsize="1"
                   textprefix="$i$t"
                   halign="center"
            />
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
                    AnimMgr.Add(scoreFrame, "<frame pos='0 {{ (newRanking?.position - 1) * -7 }}' />", 320, CAnimManager::EAnimManagerEasing::ExpOut);
                }
            ***
        -->
    </script>
</component>
