<component>
    <property type="string" name="id" />
    <property type="double" name="rowHeight"/>
    <property type="double" name="pointsWidth"/>
    <property type="double" name="padding"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="0.0"/>
    <property type="double" name="scale" default="1.0"/>
    <property type="int" name="zIndex" default="0"/>
    <property type="int" name="hidden" default="0"/>
    <property type="string" name="backgroundColor" default="ddd"/>
    
    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}" scale="{{ scale }}" z-index="{{ zIndex }}" hidden="{{ hidden }}">
            <frame size="1 1">
                <!-- top left corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="{{ backgroundColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ pointsWidth }} {{ -rowHeight }}" rot="180">
                <!-- bottom right corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="{{ backgroundColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ pointsWidth }} 0" rot="90">
                <!-- top right corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="{{ backgroundColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="0 {{ -rowHeight }}" rot="-90">
                <!-- bottom left -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="{{ backgroundColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <quad class="set" pos="1 0" size="{{ pointsWidth - 2.0 }} 1" bgcolor="{{ backgroundColor }}"/> <!-- top bar -->
            <quad class="set" pos="1 {{ 1.0 - rowHeight }}" size="{{ pointsWidth - 2.0 }} 1"
                  bgcolor="{{ backgroundColor }}"/> <!-- bottom bar -->
            <quad class="set" pos="0 -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="{{ backgroundColor }}"/> <!-- left bar -->
            <quad class="set" pos="{{ pointsWidth - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}"
                  bgcolor="{{ backgroundColor }}"/> <!-- right bar -->
            <quad class="set" pos="1 -1" size="{{ pointsWidth - 2.0 }} {{ rowHeight - 2.0 }}"
                  bgcolor="{{ backgroundColor }}"/> <!-- center quad -->
        </frame>
        
        <label id="points"
               pos="{{ x + (pointsWidth * scale) / 2.0 }} {{ rowHeight / -2.0 + 0.4 }}"
               text="x"
               valign="center"
               halign="center"
               textsize="2"
               textcolor="333"
               textfont="GameFontSemiBold"
               z-index="11"
        />
    </template>
</component>