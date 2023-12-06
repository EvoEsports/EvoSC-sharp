<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>

    <property type="LiveRankingWidgetPosition?" name="ranking" default="null"/>

    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w" default="66.0"/>
    <property type="double" name="h" default="6.0"/>
    <property type="double" name="rowSpacing" default="1.0"/>
    <property type="string" name="positionColor" default="ffd12d"/>
    <property type="string" name="playerRowBackgroundColor" default="999999"/>

    <template>
        <frame id="player_row_{{ ranking?.login }}" pos="0 {{ y }}">
            <quad id="player_row_trigger_{{ ranking?.login }}"
                  size="{{ w + 10.0 }} {{ h }}"
                  ScriptEvents="1"
            />
            
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
                   textfont="{{ Font.Bold }}"
                   textsize="2"
                   textcolor="{{ Theme.MatchRankingModule_MatchRanking_Default_PositionText }}"
                   textprefix="$i"
            />

            <frame pos="8 0">
                <frame size="0.92 999">
                    <quad pos="0 0.1"
                          size="2 {{ h + 0.25 }}"
                          style="UICommon64_1"
                          substyle="BgFrame2"
                          colorize="{{ Theme.MatchRankingModule_MatchRanking_Default_Bg }}"
                          opacity="0.75"
                    />
                </frame>

                <!-- GRADIENT -->
                <quad pos="1 0"
                      size="{{ w - 1 }} {{ h }}"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="{{ Theme.MatchRankingModule_MatchRanking_Default_Bg }}"
                      opacity="0.75"
                />

                <!-- NAME -->
                <label pos="2 {{ h / -2.0 }}"
                       size="{{ w * 0.66 }} 6"
                       text="{{ ranking?.player?.NickName }}"
                       valign="center2"
                       textfont="{{ Font.Regular }}"
                       textsize="1.15"
                       textprefix="$i$t"
                       textcolor="{{ Theme.MatchRankingModule_MatchRanking_Default_Text }}"/>

                <!-- TIME -->
                <label id="score"
                       pos="{{ w - 2.0 }} {{ h / -2.0 }}"
                       size="15 5"
                       text="{{ ranking?.time }}"
                       valign="center2"
                       halign="right"
                       textfont="{{ Font.Regular }}"
                       textsize="1.15"
                       textprefix="$i$t"
                       textcolor="{{ Theme.MatchRankingModule_MatchRanking_Default_Text }}"/>
            </frame>
        </frame>
    </template>

    <script>
        <!--
            *** OnMouseClick ***
            ***
            if(Event.Control.ControlId == "player_row_trigger_{{ ranking?.login }}"){
                if(!IsSpectatorClient) RequestSpectatorClient(True);
                SetSpectateTarget("{{ ranking?.login }}");
                continue;
            }
            ***
        -->
    </script>
</component>
