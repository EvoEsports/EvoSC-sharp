<component>
    <property type="string" name="headerColor" />
    <property type="double" name="rowHeight" />
    <property type="double" name="padding" />
    <property type="double" name="w" />
    
    <template>
        <quad pos="{{ padding + 1.0 }} 0"
              size="{{ w - padding * 2.0 - 2.0 }} {{ rowHeight }}"
              bgcolor="{{ headerColor }}"
              opacity="0.6"/>
        <quad pos="{{ w - padding - 1.0 }} -1"
              size="1 {{ rowHeight - 2.0 }}"
              bgcolor="{{ headerColor }}"
              opacity="0.6"/>
        <frame size="1 1" pos="{{ w - padding }}" rot="90">
            <!-- top left corner -->
            <quad size="2 2"
                  modulatecolor="{{ headerColor }}"
                  opacity="0.6"
                  image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
        </frame>
        <frame size="1 1" pos="{{ w - padding }} {{ -rowHeight }}" rot="180">
            <!-- top left corner -->
            <quad size="2 2"
                  modulatecolor="{{ headerColor }}"
                  opacity="0.6"
                  image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
        </frame>
    </template>
</component>