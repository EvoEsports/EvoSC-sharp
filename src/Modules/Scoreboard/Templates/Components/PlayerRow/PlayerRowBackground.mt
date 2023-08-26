<component>
    <property type="string" name="backgroundColor" />
    <property type="double" name="rowHeight" />
    <property type="double" name="padding" />
    <property type="double" name="w" />
    <property type="double" name="x" />
    
    <template>
        <frame pos="{{ x }}">
            <!-- center -->
            <quad size="{{ w - 1.0 }} {{ rowHeight }}"
                  pos="0.5 0"
                  bgcolor="{{ backgroundColor }}"
                  opacity="0.6"/>
            
            <!-- corner top left -->
            <frame size="0.5 0.5">
                <quad size="1 1"
                      modulatecolor="{{ backgroundColor }}"
                      opacity="0.6"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            
            <!-- corner bottom right -->
            <frame size="0.5 0.5" pos="0 {{ -rowHeight }}" rot="270">
                <quad size="1 1"
                      modulatecolor="{{ backgroundColor }}"
                      opacity="0.6"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            
            <!-- corner top right -->
            <frame size="0.5 0.5" pos="{{ w }}" rot="90">
                <quad size="1 1"
                      modulatecolor="{{ backgroundColor }}"
                      opacity="0.6"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            
            <!-- corner bottom right -->
            <frame size="0.5 0.5" pos="{{ w }} {{ -rowHeight }}" rot="180">
                <quad size="1 1"
                      modulatecolor="{{ backgroundColor }}"
                      opacity="0.6"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            
            <!-- bar left -->
            <quad pos="0 -0.53"
                  size="0.5 {{ rowHeight - 1.06 }}"
                  bgcolor="{{ backgroundColor }}"
                  opacity="0.6"/>
            
            <!-- bar right -->
            <quad pos="{{ w - 0.5 }} -0.53"
                  size="0.5 {{ rowHeight - 1.06 }}"
                  bgcolor="{{ backgroundColor }}"
                  opacity="0.6"/>
        </frame>
    </template>
</component>