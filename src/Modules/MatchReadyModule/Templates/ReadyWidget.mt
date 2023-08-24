<component>
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.Switch" as="Switch" />
    
    <property type="bool" name="isReady" default="false" />
    
    <template>
        <Theme />

        
        <frame pos="-11.5 60">
            <!-- <framemodel id="EvoSC_Model_ReadyWidget_Corner">
                <quad
                        size="4 4"
                        image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                        modulatecolor='{{ isReady ? "00ff00" : "ff0000" }}'
                />
            </framemodel>
            
            <frame size="2 2">
                <frameinstance modelid="EvoSC_Model_ReadyWidget_Corner" />
            </frame>

            <frame size="2 2" pos="23 0" rot="90">
                <frameinstance modelid="EvoSC_Model_ReadyWidget_Corner" />
            </frame>

            <frame size="2 2" pos="23 -7" rot="180">
                <frameinstance modelid="EvoSC_Model_ReadyWidget_Corner" />
            </frame>

            <frame size="2 2" pos="0 -7" rot="-90">
                <frameinstance modelid="EvoSC_Model_ReadyWidget_Corner" />
            </frame>
            
            <quad bgcolor='{{ isReady ? "00ff00" : "ff0000" }}' size="23 3" pos="0 -2" />
            <quad bgcolor='{{ isReady ? "00ff00" : "ff0000" }}' size="19 2" pos="2 0" />
            <quad bgcolor='{{ isReady ? "00ff00" : "ff0000" }}' size="19 2" pos="2 -5" /> -->

            <quad
                    bgcolor="1f232c"
            />
            
            <!-- <label
                    text='$s{{ isReady ? " READY" : " NOT READY" }}'
                    class="text"
                    valign="center"
                    halign="center"
                    textsize="4"
                    pos="15 6"
                    textcolor='{{ isReady ? "00ff00" : "ff0000" }}'
            />
            
            <quad
                    bgcolor="47495A"
                    size="30 8"
            />
            
            <frame size="30 8">
                <quad
                        rot="167"
                        modulatecolor="6b6f88"
                        size="32 20"
                        pos="33 -12"
                        image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                />
            </frame>
            
            <label
                    class="text"
                    text='{{ isReady ? "UNREADY" : "I AM READY" }}'
                    halign="center"
                    valign="center"
                    pos="15 -3.8"
            />
            
            <label 
                    text="$i0/4 players ready"
                    textsize="1"
                    class="text"
                    textcolor="000000"
                    opacity="0.5"
                    halign="center"
                    pos="15 -10"
            /> -->
        </frame>
    </template>

    <script resource="EvoSC.Scripts.UIScripts" />
</component>
