<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <using namespace="System.Linq"/>

    <import component="LiveRankingModule.Components.PlayerScore" as="PlayerScore"/>

    <property type="List<LiveRankingWidgetPosition>" name="previousRankings"/>
    <property type="List<LiveRankingWidgetPosition>" name="rankingsExisting"/>
    <property type="List<LiveRankingWidgetPosition>" name="rankingsNew"/>

    <property type="double" name="scale" default="0.9"/>
    <property type="double" name="w" default="68.0"/>
    <property type="double" name="x" default="-160.0"/>
    <property type="double" name="y" default="78.0"/>
    <property type="double" name="headerHeight" default="8.0"/>
    <property type="double" name="rowHeight" default="6.0"/>
    <property type="double" name="rowSpacing" default="1.0"/>
    <property type="int" name="rowsVisible" default="4"/>
    
    <property type="string" name="headerColor" default="c21d62"/>
    <property type="string" name="primaryColor" default="4357ea"/>
    <property type="string" name="playerRowBackgroundColor" default="999999"/>
    <property type="string" name="logoUrl" default=""/>

    <template>
        <frame id="live_rankings" pos="{{ x }} {{ y }}" scale="{{ scale }}" z-index="100">
            <frame>
                <frame size="{{ w }} {{ headerHeight }}">
                    <!-- HEADER -->
                    <quad pos="{{ w * 0.5 - 10.0 }} 0.15"
                          size="{{ headerHeight + 0.3 }} {{ w + 20 }}"
                          valign="center"
                          style="UICommon64_1"
                          substyle="BgFrame1"
                          colorize="{{ primaryColor }}"
                          rot="90"
                    />

                    <!-- GRADIENT -->
                    <quad size="{{ w }} {{ headerHeight }}"
                          image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                          modulatecolor="{{ headerColor }}"
                    />

                    <!-- LABEL -->
                    <label pos="2 {{ headerHeight / -2.0 - 0.4 }}"
                           text="Live Ranking"
                           valign="center2"
                           textfont="GameFontExtraBold"
                           textprefix="$i$t"
                           textsize="2"
                    />

                    <!-- LOGO -->
                    <quad if='logoUrl != ""'
                          pos="{{ w - 3.0 }} {{ headerHeight / -2.0 }}"
                          size="20 3.2"
                          valign="center"
                          halign="right"
                          keepratio="Fit"
                          image="{{ logoUrl }}"
                          opacity="0.75"
                    />
                </frame>
                
                <!-- BACKGROUND -->
                <quad pos="0 {{ -headerHeight + 0.15 }}"
                      size="{{ w - 0.15 }} {{ rowsVisible * (rowSpacing + rowHeight) + rowSpacing * 4.0 }}"
                      bgcolor="24262f"
                      opacity="0.9"
                />
            </frame>

            <!-- CONTENT -->
            <frame id="players" pos="0 {{ -headerHeight - rowSpacing * 2.0 }}" z-index="10">
                <PlayerScore foreach="LiveRankingWidgetPosition position in previousRankings"
                             y="{{ (position.position - 1) * (rowHeight + rowSpacing) * -1.0 }}"
                             w="{{ w - 8.0 }}"
                             h="{{ rowHeight }}"
                             rowSpacing="{{ rowSpacing }}"
                             playerRowBackgroundColor="{{ playerRowBackgroundColor }}"
                             ranking="{{ position }}"
                             newRanking="{{ rankingsExisting.FirstOrDefault(r => r.player.AccountId == position.player.AccountId) }}"
                />
                <PlayerScore foreach="LiveRankingWidgetPosition position in rankingsNew"
                             y="{{ (position.position - 1) * (rowHeight + rowSpacing) * -1.0 }}"
                             w="{{ w - 8.0 }}"
                             h="{{ rowHeight }}"
                             rowSpacing="{{ rowSpacing }}"
                             playerRowBackgroundColor="{{ playerRowBackgroundColor }}"
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
