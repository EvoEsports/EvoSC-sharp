<component>
    <property type="double" name="scale" default="0.9"/>
    <property type="double" name="w" default="68.0"/>
    <property type="double" name="x" default="-160.0"/>
    <property type="double" name="y" default="41.0"/>
    <property type="double" name="headerHeight" default="8.0"/>
    <property type="double" name="bodyHeight" default="23.0"/>

    <property type="string" name="headerColor" default="c21d62"/>
    <property type="string" name="primaryColor" default="4357ea"/>
    <property type="string" name="playerRowBackgroundColor" default="999999"/>
    <property type="string" name="logoUrl" default=""/>
    
    <template>
        <frame pos="{{ x }} {{ y }}" size="{{ w }} 999" scale="{{ scale }}" z-index="100">
            <frame>
                <frame size="{{ w }} {{ headerHeight - 0.1 }}">
                    <!-- HEADER -->
                    <quad pos="{{ w * 0.5 - 10.0 }} 0.15"
                          size="{{ headerHeight + 0.3 }} {{ w + 20.1 }}"
                          valign="center"
                          style="UICommon64_1"
                          substyle="BgFrame1"
                          colorize="{{ primaryColor }}"
                          rot="90"
                    />

                    <!-- GRADIENT -->
                    <quad size="{{ w }} {{ headerHeight + 0.1 }}"
                          image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                          modulatecolor="{{ headerColor }}"
                    />

                    <!-- LABEL -->
                    <label pos="2 {{ headerHeight / -2.0 - 0.4 }}"
                           text="Sponsors"
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
                      size="{{ w - 0.15 }} {{ bodyHeight }}"
                      bgcolor="24262f"
                      opacity="0.9"
                />
            </frame>

            <!-- CONTENT -->
            <frame pos="0 {{ -headerHeight - 2.0 }}" z-index="10">
                <label text="ayy" textsize="1" />
                <quad pos="2 0" size="{{ w - 2.0 }} {{ bodyHeight - 4.0 }}" bgcolor="f00" />
            </frame>
        </frame>
    </template>
    
    <script><!--
    
    Void SlideIn(){
    
    }
    
    main() {
        log("hello sponsors");
    }
    --></script>
</component>