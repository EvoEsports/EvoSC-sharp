<!--
    Shows an animated alert box with a text and icon.
-->
<component>
    <!-- The ID of the control. -->
    <property type="string" name="id" default="evosc_alert" />
    
    <!-- X position of the control. -->
    <property type="double" name="x" default="-30.0" />
    
    <!-- Y position of the control. -->
    <property type="double" name="y" default="0.0" />
    
    <!-- The area width of the alert text. -->
    <property type="double" name="width" default="60.0" />
    
    <!-- Icon to show for the alert. -->
    <property type="string" name="icon" default="" />
    
    <!-- The text to display in the alert. -->
    <property type="string" name="text" default="This is an alert" />
    
    <template>
        <frame 
                pos="{{ x }} {{ y }}" 
                size="{{ width+7 }} 7" 
                id="{{ id }}"
                class="evosc-alert-frame"
                data-startX="{{ x }}"
                data-startY="{{ y }}"
                z-index="100"
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
