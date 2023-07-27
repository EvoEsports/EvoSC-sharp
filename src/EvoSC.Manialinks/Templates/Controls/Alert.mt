<component>
    <property type="string" name="id" default="evosc_alert" />
    <property type="double" name="x" default="-30.0" />
    <property type="double" name="y" default="0.0" />
    <property type="double" name="width" default="60.0" />
    <property type="string" name="icon" default="" />
    <property type="string" name="text" default="This is an alert" />
    
    <template>
        <frame 
                pos="{{ x }} {{ y }}" 
                size="{{ width+8 }} 7" 
                id="{{ id }}" 
                data-width="{{ width+7 }}"
                class="evosc-alert-frame"
                data-startX="{{ x }}"
                data-startY="{{ y }}"
        >
            <frame id="{{ id }}-inner-frame" pos="0 0" size="{{ width+7 }} 7">
                <framemodel id="EvoSC_Model_Alert_Circle_C1_{{ id }}">
                    <quad
                            size="4 4"
                            modulatecolor="FAFAFA"
                            image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                    />
                </framemodel>

                <framemodel id="EvoSC_Model_Alert_Circle_C2_{{ id }}">
                    <quad
                            size="4 4"
                            modulatecolor="FF0058"
                            image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                    />
                </framemodel>

                <quad bgcolor="FF0058" pos="5 0" size="{{ width }} 7" />
                <quad bgcolor="FF0058" pos="{{ width + 5 }} 0" size="2 5" />
                <frame size="2 2" pos="{{ width + 7 }} -7" rot="180">
                    <frameinstance modelid="EvoSC_Model_Alert_Circle_C2_{{ id }}" />
                </frame>

                <frame size="2 2">
                    <frameinstance modelid="EvoSC_Model_Alert_Circle_C1_{{ id }}" />
                </frame>

                <frame size="2 2" pos="7 -7" rot="180">
                    <frameinstance modelid="EvoSC_Model_Alert_Circle_C1_{{ id }}" />
                </frame>

                <frame size="5 5">
                    <label text="something" />
                </frame>

                <quad bgcolor="FAFAFA" pos="2 0" size="5 2" />
                <quad bgcolor="FAFAFA" pos="0 -5" size="5 2" />
                <quad bgcolor="FAFAFA" pos="0 -2" size="7 3" />

                <label text="{{ icon }}" textcolor="FF0058" valign="center" halign="center" pos="3.5 -3.3" />
                <label class="text" text="{{ text }}" textcolor="FAFAFA" valign="center" pos="9.5 -3.2" size="{{ width }} 7" />
            </frame>
        </frame>
    </template>
    
    <script resource="EvoSC.Scripts.Alert" once="true" />
</component>
