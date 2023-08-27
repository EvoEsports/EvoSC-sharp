<component>
    <property type="string" name="id" />
    <property type="string" name="backgroundColor" />
    <property type="double" name="rowHeight" />
    <property type="double" name="padding" />
    <property type="double" name="w" />
    <property type="double" name="x" />
    
    <template>
        <frame id="{{ id }}" pos="{{ x }}">
            <!-- START BG 1 -->
            <frame>
                <!-- center -->
                <quad size="{{ w - 1.0 }} {{ rowHeight }}"
                      pos="0.5 0"
                      bgcolor="{{ backgroundColor }}"
                      opacity="0.25"/>

                <!-- corner top left -->
                <frame size="0.5 0.5">
                    <quad size="1 1"
                          modulatecolor="{{ backgroundColor }}"
                          opacity="0.25"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
                </frame>

                <!-- corner top right -->
                <frame size="0.5 0.5" pos="{{ w }}" rot="90">
                    <quad size="1 1"
                          modulatecolor="{{ backgroundColor }}"
                          opacity="0.25"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
                </frame>

                <!-- corner bottom right -->
                <frame size="0.5 0.5" pos="{{ w }} {{ -rowHeight }}" rot="180">
                    <quad size="1 1"
                          modulatecolor="{{ backgroundColor }}"
                          opacity="0.25"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
                </frame>

                <!-- corner bottom left -->
                <frame size="0.5 0.5" pos="0 {{ -rowHeight }}" rot="270">
                    <quad size="1 1"
                          modulatecolor="{{ backgroundColor }}"
                          opacity="0.25"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
                </frame>

                <!-- bar left -->
                <quad pos="0 -0.53"
                      size="0.5 {{ rowHeight - 1.06 }}"
                      bgcolor="{{ backgroundColor }}"
                      opacity="0.25"/>

                <!-- bar right -->
                <quad pos="{{ w - 0.5 }} -0.53"
                      size="0.5 {{ rowHeight - 1.06 }}"
                      bgcolor="{{ backgroundColor }}"
                      opacity="0.25"
                />
            </frame>
            <!-- END BG 1 -->
            
            <!-- START BG 2 -->
            <frame>
                <!-- 0: bar left -->
                <quad pos="0 -0.54"
                      size="0.5 {{ rowHeight - 1.07 }}"
                      bgcolor="{{ backgroundColor }}"
                      opacity="0.75"
                />
                
                <!-- 1: corner top left -->
                <frame size="0.43 0.45">
                    <quad size="1 1"
                          modulatecolor="{{ backgroundColor }}"
                          class="modulate"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                          opacity="0.75"
                    />
                </frame>

                <!-- 2: corner bottom left -->
                <frame size="0.43 0.5" pos="0 {{ -rowHeight }}" rot="270">
                    <quad size="1 1"
                          modulatecolor="{{ backgroundColor }}"
                          class="modulate"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                          opacity="0.75"
                    />
                </frame>

                <!-- 3: gradient -->
                <frame pos="0.5 0">
                    <quad size="{{ rowHeight }} {{ w / 2.0 }}"
                          rot="90"
                          pos="{{ w / 2.0 }} 0"
                          modulatecolor="{{ backgroundColor }}"
                          class="modulate"
                          opacity="0.75"
                          image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                    />
                </frame>
            </frame>
            <!-- END BG 2 -->
        </frame>
    </template>
    
    <script once="true">
        <!--
        Void SetPlayerBackgroundColor(CMlFrame backgroundFrame, Vec3 color) {
            declare targetFrame = (backgroundFrame.Controls[1] as CMlFrame);
            (targetFrame.Controls[0] as CMlQuad).BgColor = color;
            
            Page.GetClassChildren("modulate", targetFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).ModulateColor = color;
            }
        }
        -->
    </script>
</component>