<component>
    <property type="string" name="primaryColor" />
    <property type="double" name="w" />
    
    <template>
        <label id="map_name" pos="5 -7.5" text="MAP NAME" valign="center" textsize="2.8" textfont="GameFontSemiBold"/>
        <label id="author_name" pos="5 -12" textprefix="by " text="AUTHOR NAME" valign="center" textsize="1.4" textfont="GameFontRegular"/>

        <frame pos="{{ w - 0.25 }} -6.5">
<!--            <frame size="1 1" pos="-2.6 3" rot="90">-->
<!--                &lt;!&ndash; top right corner &ndash;&gt;-->
<!--                <quad size="2 2"-->
<!--                      modulatecolor="{{ primaryColor }}"-->
<!--                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>-->
<!--            </frame>-->
<!--            <quad pos="-3.6 2" size="1 6.3" bgcolor="{{ primaryColor }}"/> &lt;!&ndash; right bar &ndash;&gt;-->
<!--            <frame size="1 1" pos="-2.6 -5.3" rot="180">-->
<!--                &lt;!&ndash; bottom right corner &ndash;&gt;-->
<!--                <quad size="2 2"-->
<!--                      modulatecolor="{{ primaryColor }}"-->
<!--                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>-->
<!--            </frame>-->
<!--            <quad size="8.3 90"-->
<!--                  rot="-90"-->
<!--                  pos="-48.6 3.0"-->
<!--                  modulatecolor="{{ primaryColor }}"-->
<!--                  halign="right"-->
<!--                  valign="center"-->
<!--                  image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"/>-->
            <label id="round_label"
                   text="ROUND 1/?"
                   textprefix="$i"
                   pos="-4 0"
                   valign="center"
                   halign="right"
                   textsize="1.9"
                   textfont="GameFontSemiBold"/>
<!--            <label id="gradient_label_small"-->
<!--                   text="MODE"-->
<!--                   pos="-4 1.1"-->
<!--                   valign="center"-->
<!--                   halign="right"-->
<!--                   textsize="0.7"-->
<!--                   opacity="0.75"-->
<!--                   textfont="GameFontSemiBold"-->
<!--                   hidden="1"-->
<!--            />-->
        </frame>

        <!-- Sub Text (Below highlighted box) -->
        <label id="sub_text"
               pos="{{ w - 7 }} -14.5"
               textcolor="fff"
               valign="center"
               halign="right"
               textsize="1"
               textfont="GameFontRegular"
        />

        <!-- Settings Icon -->
        <label id="settings_icon"
               pos="{{ w - 4.6 }} -14.7"
               size="5 5"
               textcolor="fff"
               opacity="0.9"
               valign="center"
               halign="center"
               textsize="1"
               text=""
               textfont="GameFontRegular"
               ScriptEvents="1"
               focusareacolor1="0000"
               focusareacolor2="0000"
        />
    </template>
</component>