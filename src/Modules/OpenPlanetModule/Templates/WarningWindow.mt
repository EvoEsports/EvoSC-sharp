<component>
    <using namespace="EvoSC.Modules.Official.OpenPlanetModule.Config" />
    <using namespace="EvoSC.Modules.Official.OpenPlanetModule.Models" />
    
    <import component="EvoSC.Window" as="Window" />
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
    
    <property type="string[]" name="AllowedSignatures" />
    <property type="IOpenPlanetControlSettings" name="Config" />
    <property type="OpJailReason" name="Reason" />
    <property type="dynamic" name="Locale" />
    <property type="(string Explanation, string Question)" name="WhatToDo" />
    
    <template>
        <Theme />
        
        <Window 
                canClose="false" 
                canMove="false" 
                icon="$FB0{{ Icons.Warning }}" 
                title="OpenPlanet Verification Warning"
                width="200"
                height="120"
                x="-100"
                y="60"
        >
            <frame size="200 100" pos="0 -14">
                <label textsize="15" halign="center" class="text" text="{{ Icons.Warning }}" pos="100 -1" textcolor="{{ Theme.OpenPlanetModule_WarningWindow_TextWarning }}" />
                <label textsize="4" class="text" text="{{ Locale.PlayerLanguage.WarningMl_OpenPlanetDetected }}" halign="center" pos="100 -18"  textcolor="{{ Theme.OpenPlanetModule_WarningWindow_TextWarning }}"/>
                <label class="text" text="{{ Locale.PlayerLanguage.WarningMl_OpenPlanetRestricted }}" halign="center" pos="100 -24"/>
                
                <label class="text" text="{{ Locale.PlayerLanguage.WarningMl_ChoseSignatureMode }}" halign="center" pos="100 -32" if="Reason == OpJailReason.InvalidSignatureMode" />
                <label class="text" textcolor="{{ Theme.OpenPlanetModule_WarningWindow_TextHighlight }}" text='{{ string.Join(", ", AllowedSignatures) }}' halign="center" pos="100 -36" if="Reason == OpJailReason.InvalidSignatureMode"/>

                <label class="text" text="{{ Locale.PlayerLanguage.WarningMl_MinimumVersionRequired }}" halign="center" pos="100 -32" if="Reason == OpJailReason.InvalidVersion" />
                <label class="text" textcolor="{{ Theme.OpenPlanetModule_WarningWindow_TextHighlight }}" text='{{ Config.MinimumRequiredVersion }}' halign="center" pos="100 -36" if="Reason == OpJailReason.InvalidVersion"/>

                <label class="text" text="{{ Locale.PlayerLanguage.WarningMl_DisableOpenPlanet }}" halign="center" pos="100 -32" if="Reason == OpJailReason.OpenPlanetNotAllowed" />
                
                <quad bgcolor="{{ Theme.OpenPlanetModule_WarningWindow_Border }}" size="125 22" pos="37.5 -45" />
                <quad bgcolor="{{ Theme.OpenPlanetModule_WarningWindow_BgSecondary }}" size="124.8 21.8" pos="37.6 -45.1" />
                
                <label class="text" textsize="2" text="{{ WhatToDo.Question }}" halign="center" pos="100 -47" />
                <label 
                        class="text" 
                        text="{{ WhatToDo.Explanation }}" 
                        halign="center" 
                        size="110 14" 
                        pos="100 -53"
                        autonewline="1"
                        maxline="3"
                />
                
                <label class="text" text="$iYou will be automatically kicked in {{ Config.KickTimeout }} seconds." halign="center" pos="100 -73" id="countdownText" />
                
                <IconButton icon="{{ Icons.SignIn }}" text="Disconnect Now" width="30" y="-78" x="84" id="btnDisconnect" action="ManialinkActions/Disconnect" />
            </frame>
        </Window>
    </template>
    
    <script><!--
        *** OnInitialization ***
        ***
        declare lastTime = Now;
        declare countdown = {{ Config.KickTimeout }};
        ***
        
        *** OnLoop ***
        ***
        if (countdown > 0 && lastTime + 1000 <= Now) {
            countdown -= 1;
        
            declare countdownText <=> Page.MainFrame.GetFirstChild("countdownText") as CMlLabel;
            countdownText.SetText("$i{{ Locale.PlayerLanguage.WarningMl_CountDownPart1 }}"^countdown^"{{ Locale.PlayerLanguage.WarningMl_CountDownPart2 }}");
            
            lastTime = Now;
        }
        ***
    --></script>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
