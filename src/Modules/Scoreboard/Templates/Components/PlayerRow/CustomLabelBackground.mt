<component>
    <property type="string" name="id" />
    <property type="double" name="rowHeight"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="widthMultiplier" default="0.63"/>
    <property type="int" name="zIndex" default="0"/>
    
    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" z-index="{{ zIndex }}">
            <frame size="1 1" pos="0 {{ -rowHeight }}" rot="180">
                <!-- bottom right corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="000"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.25"
                />
            </frame>
            <frame size="1 1" pos="0 0" rot="90">
                <!-- top right corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="000"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.25"
                />
            </frame>
            <quad class="set" pos="-1 -1.05" size="1 {{ rowHeight - 2.1 }}" bgcolor="000" z-index="2" opacity="0.25"/> <!-- right bar -->
            <frame pos="-1 {{ -rowHeight }}">
                <quad size="{{ rowHeight }} {{ w * widthMultiplier }}"
                      pos="{{ w * -widthMultiplier }} 0"
                      rot="-90"
                      class="modulate"
                      modulatecolor="000"
                      opacity="0.25"
                      image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"/>
            </frame>
        </frame>
    </template>
</component>