<component>
    <property type="string" name="id"/>
    <property type="string" name="positionBackgroundColor"/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="0.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="int" name="zIndex" default="0"/>

    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" z-index="{{ zIndex }}">
            <!-- bottom right corner -->
            <frame size="0.5 0.5" pos="{{ w }} {{ -h }}" rot="180">
                <quad size="1 1"
                      modulatecolor="{{ positionBackgroundColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            
            <!-- top right corner -->
            <frame size="0.5 0.5" pos="{{ w }} 0" rot="90">
                <quad size="1 1"
                      modulatecolor="{{ positionBackgroundColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>

            <!-- right bar -->
            <quad pos="{{ w - 0.5 }} -0.5" size="0.5 {{ h - 1.0 }}"
                  bgcolor="{{ positionBackgroundColor }}"/>

            <!-- center quad -->
            <quad pos="0 0" size="{{ w - 0.5 }} {{ h }}"
                  bgcolor="{{ positionBackgroundColor }}"/>
        </frame>

        <label id="position"
               pos="{{ x + h * 0.6 }} {{ h / -2.0 + 0.25 }}"
               valign="center"
               halign="center"
               textsize="2.6"
               textfont="GameFontBlack"
               z-index="5"
        />
    </template>
</component>