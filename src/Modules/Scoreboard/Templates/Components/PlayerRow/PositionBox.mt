<component>
    <property type="string" name="id"/>
    <property type="double" name="rowHeight"/>
    <property type="string" name="primaryColor"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="int" name="zIndex" default="0"/>
    
    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" z-index="{{ zIndex }}">
            <frame size="1 1">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ rowHeight * 1.2 }} {{ -rowHeight }}" rot="180">
                <!-- bottom right corner -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ rowHeight * 1.2 }} 0" rot="90">
                <!-- top right corner -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="0 {{ -rowHeight }}" rot="-90">
                <!-- bottom left -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <quad pos="1 0" size="{{ rowHeight * 1.2 - 2.0 }} 1" bgcolor="{{ primaryColor }}"/> <!-- top bar -->
            <quad pos="1 {{ 1.0 - rowHeight }}" size="{{ rowHeight * 1.2 - 2.0 }} 1"
                  bgcolor="{{ primaryColor }}"/> <!-- bottom bar -->
            <quad pos="0 -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="{{ primaryColor }}"/> <!-- left bar -->
            <quad pos="{{ rowHeight * 1.2 - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}"
                  bgcolor="{{ primaryColor }}"/> <!-- right bar -->
            <quad pos="1 -1" size="{{ rowHeight * 1.2 - 2.0 }} {{ rowHeight - 2.0 }}"
                  bgcolor="{{ primaryColor }}"/> <!-- center quad -->
        </frame>

        <label id="position"
               pos="{{ x + rowHeight * 0.6 }} {{ rowHeight / -2.0 + 0.25 }}"
               valign="center"
               halign="center"
               textsize="2.6"
               textfont="GameFontBlack"
               z-index="5"
        />
    </template>
</component>