<component>
    <property type="string" name="id"/>
    <property type="string" name="accentColor"/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="0.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="opacity" default="1.0"/>
    <property type="int" name="zIndex" default="0"/>

    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" z-index="{{ zIndex }}">
            <!-- top left corner -->
            <frame size="0.5 0.5">
                <quad size="1 1"
                      modulatecolor="{{ accentColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="{{ opacity }}"/>
            </frame>

            <!-- top right corner -->
            <frame size="0.5 0.5" pos="{{ w }} 0" rot="90">
                <quad size="1 1"
                      modulatecolor="{{ accentColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="{{ opacity }}"/>
            </frame>

            <!-- bottom right corner -->
            <frame size="0.5 0.5" pos="{{ w }} {{ -h }}" rot="180">
                <quad size="1 1"
                      modulatecolor="{{ accentColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="{{ opacity }}"/>
            </frame>

            <!-- bottom left corner -->
            <frame size="0.5 0.5" pos="0 {{ -h }}" rot="270">
                <quad size="1 1"
                      modulatecolor="{{ accentColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="{{ opacity }}"/>
            </frame>

            <!-- center quad -->
            <quad pos="0 -0.5" size="{{ w }} {{ h - 1.0 }}" bgcolor="{{ accentColor }}" opacity="{{ opacity }}"/>

            <!-- top bar -->
            <quad pos="0.5 0" size="{{ w - 1.0 }} 0.5" bgcolor="{{ accentColor }}" opacity="{{ opacity }}"/>

            <!-- right bar -->
            <quad pos="{{ w - 0.5 }} -0.5" size="0.5 {{ h - 1.0 }}" bgcolor="{{ accentColor }}" opacity="{{ opacity }}"/>

            <!-- bottom bar -->
            <quad pos="0.5 {{ -h + 0.5 }}" size="{{ w - 1.0 }} 0.5" bgcolor="{{ accentColor }}" opacity="{{ opacity }}"/>
        </frame>
    </template>
</component>