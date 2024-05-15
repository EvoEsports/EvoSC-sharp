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
    
    <!-- The text to display in the alert. -->
    <property type="string" name="text" default="This is an alert" />

    <!-- The text to display in the alert. -->
    <property type="string" name="type" default="primary" />
    
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
                            modulatecolor="{{ Theme.UI_Alert_Default_BgSecondary }}"
                            image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                    />
                </framemodel>

                <framemodel id="EvoSC_Model_Alert_Circle_C2_{{ id }}">
                    <quad
                            size="4 4"
                            modulatecolor="{{ Util.TypeToColorBg(type) }}"
                            image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                    />
                </framemodel>

                <quad bgcolor="{{ Util.TypeToColorBg(type) }}" pos="5 0" size="{{ width }} 7" />
                <quad bgcolor="{{ Util.TypeToColorBg(type) }}" pos="{{ width + 5 }} 0" size="2 5" />
                <frame size="2 2" pos="{{ width + 7 }} -7" rot="180">
                    <frameinstance modelid="EvoSC_Model_Alert_Circle_C2_{{ id }}" />
                </frame>

                <frame size="2 2">
                    <frameinstance modelid="EvoSC_Model_Alert_Circle_C1_{{ id }}" />
                </frame>

                <frame size="2 2" pos="7 -7" rot="180">
                    <frameinstance modelid="EvoSC_Model_Alert_Circle_C1_{{ id }}" />
                </frame>
              
                <quad bgcolor="{{ Theme.UI_Alert_Default_BgSecondary }}" pos="2 0" size="5 2" />
                <quad bgcolor="{{ Theme.UI_Alert_Default_BgSecondary }}" pos="0 -5" size="5 2" />
                <quad bgcolor="{{ Theme.UI_Alert_Default_BgSecondary }}" pos="0 -2" size="7 3" />

                <label text="{{ Util.TypeToIcon(type) }}" textcolor="{{ Util.TypeToColorBg(type) }}" valign="center" halign="center" pos="3.5 -3.3" />
                <label class="text-primary" text="{{ text }}" textcolor="{{ Theme.UI_Alert_Default_Text }}" valign="center" pos="9.5 -3.2" size="{{ width }} 7" />
            </frame>
        </frame>
    </template>
    
    <script resource="EvoSC.Scripts.Alert" once="true" />
</component>
