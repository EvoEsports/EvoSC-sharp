<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>
    <using namespace="EvoSC.Modules.Official.WorldRecordModule.Models"/>

    <property type="IMap?" name="map" default="null"/>
    <property type="string" name="mapauthor" default="null"/>
    <property type="WorldRecord?" name="record" default="null"/>

    <property type="double" name="scale" default="0.9"/>
    <property type="double" name="w" default="68.0"/>
    <property type="double" name="y" default="85.0"/>
    <property type="double" name="headerHeight" default="8.0"/>
    <property type="double" name="bodyHeight" default="22.0"/>

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
                          colorize="{{ headerColor }}"
                    />

                    <!-- GRADIENT -->
                    <quad pos="{{ w }} {{ -headerHeight }}"
                          size="{{ w }} {{ headerHeight - 0.1 }}"
                          image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                          modulatecolor="{{ primaryColor }}"
                          rot="180"
                    />

                    <!-- LABEL -->
                    <label pos="2 {{ headerHeight / -2.0 - 0.4 }}"
                           text="Current Map"
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
                <quad pos="0 {{ -headerHeight + 0.1 }}"
                      size="{{ w }} {{ bodyHeight }}"
                      bgcolor="24262f"
                      opacity="0.9"
                />
            </frame>

            <framemodel id="gradient_box">
                <frame size="0.95 8">
                    <quad pos="0 0.2"
                          size="2 8.4"
                          style="UICommon64_1"
                          substyle="BgFrame2"
                          colorize="{{ playerRowBackgroundColor }}"
                          opacity="0.75"
                    />
                </frame>

                <!-- GRADIENT -->
                <quad pos="1 0"
                      size="{{ w - 10.0 }} 8"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="{{ playerRowBackgroundColor }}"
                      opacity="0.75"
                />
            </framemodel>

            <!-- CONTENT -->
            <frame pos="2 {{ -headerHeight - 2.0 }}" size="{{ w }} 999" z-index="10">
                <frameinstance modelid="gradient_box" />
                <frame pos="-0.2 -1">
                    <label pos="2 0"
                           text='{{ map?.Name ?? "loading..." }}'
                           textsize="1.4"
                           textfont="GameFontSemiBold"
                    />
                    <label pos="2 -3.2"
                           text='by {{ mapauthor ?? "loading..." }}'
                           textsize="1.1"
                           textfont="GameFontRegular"
                    />
                </frame>

                <frame v-if="record != null">
                    <frameinstance modelid="gradient_box" pos="0 -9" />
                    <frame pos="-0.2 -10">
                        <label pos="2 0"
                               text="Record: {{ record?.Time.ToString() }} [{{ record?.Source }}]"
                               textsize="1.4"
                               textfont="GameFontSemiBold"
                        />
                        <label pos="2 -3.2"
                               text="by {{ record?.PlayerName }}"
                               textsize="1.1"
                               textfont="GameFontRegular"
                        />
                    </frame>
                </frame>
            </frame>
        </frame>
    </template>
</component>