<component>
    <using namespace="EvoSC.Modules.Official.MatchRankingModule.Models"/>
    <using namespace="System.Linq"/>
    
    <import component="MatchRankingModule.Components.PlayerScore" as="PlayerScore"/>

    <property type="List<MatchRankingWidgetPosition>" name="Scores"/>

    <property type="double" name="scale" default="0.9"/>
    <property type="double" name="w" default="68.0"/>
    <property type="double" name="y" default="57.0"/>
    <property type="double" name="headerHeight" default="8.0"/>
    <property type="double" name="rowHeight" default="6.0"/>
    <property type="double" name="rowSpacing" default="1.0"/>
    <property type="int" name="rowsVisible" default="4"/>

    <property type="string" name="headerColor" default="c21d62"/>
    <property type="string" name="primaryColor" default="4357ea"/>
    <property type="string" name="playerRowBackgroundColor" default="999999"/>
    <property type="string" name="logoUrl" default=""/>

    <template>
        <frame pos="{{ 160.0 - w * scale }} {{ y }}" scale="{{ scale }}" z-index="100">
            <frame>
                <frame size="{{ w }} {{ headerHeight - 0.1 }}">
                    <!-- HEADER -->
                    <quad pos="-0.15 0"
                          size="{{ w + 20.1 }} {{ headerHeight + 0.3 }}"
                          style="UICommon64_1"
                          substyle="BgFrame1"
                          colorize="{{ Theme.MatchRankingModule_MatchRanking_Default_BgHeaderGrad1 }}"
                    />

                    <!-- GRADIENT -->
                    <quad pos="{{ w }} {{ -headerHeight }}"
                          size="{{ w }} {{ headerHeight - 0.1 }}"
                          image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                          modulatecolor="{{ Theme.MatchRankingModule_MatchRanking_Default_BgHeaderGrad2 }}"
                          rot="180"
                    />

                    <!-- LABEL -->
                    <label pos="2 {{ headerHeight / -2.0 - 0.4 }}"
                           text="Match Ranking"
                           valign="center2"
                           textfont="{{ Font.Bold }}"
                           textprefix="$i$t"
                           textsize="2"
                           textcolor="{{ Theme.MatchRankingModule_MatchRanking_Default_Text }}"
                    />

                    <!-- LOGO -->
                    <quad if='Theme.MatchRankingModule_MatchRanking_Default_Logo != ""'
                          pos="{{ w - 3.0 }} {{ headerHeight / -2.0 }}"
                          size="20 3.2"
                          valign="center"
                          halign="right"
                          keepratio="Fit"
                          image="{{ Theme.MatchRankingModule_MatchRanking_Default_Logo }}"
                          opacity="0.75"
                    />
                </frame>

                <!-- BACKGROUND -->
                <quad pos="0 {{ -headerHeight + 0.1 }}"
                      size="{{ w }} {{ rowsVisible * (rowSpacing + rowHeight) + rowSpacing * 4.0 }}"
                      bgcolor="{{ Theme.MatchRankingModule_MatchRanking_Default_BgContent }}"
                      opacity="0.9"
                />
            </frame>

            <!-- CONTENT -->
            <frame id="players" pos="0 {{ -headerHeight - rowSpacing * 2.0 }}" size="{{ w }} 999" z-index="10">
                <PlayerScore foreach="MatchRankingWidgetPosition position in Scores"
                             y="{{ (position.Position - 1) * (rowHeight + rowSpacing) * -1.0 }}"
                             w="{{ w - 8.0 }}"
                             h="{{ rowHeight }}"
                             rowSpacing="{{ rowSpacing }}"
                             playerRowBackgroundColor="{{ Theme.MatchRankingModule_MatchRanking_Default_BgRow }}"
                             ranking="{{ position }}"
                />
            </frame>
        </frame>
    </template>

    <script>
        <!--
            main() {
                while(True){
                    yield;
                    
                    foreach(Event in PendingEvents){
                        if(Event.Type == CMlScriptEvent::Type::MouseClick){
                            +++ OnMouseClick +++
                        }
                    }
                }
            }
        -->
    </script>
</component>