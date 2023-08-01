<component>
    <using namespace="EvoSC.Modules.Official.OpenPlanetModule.Config" />
    
    <import component="EvoSC.Window" as="Window" />
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
    
    <property type="string[]" name="allowedSignatures" />
    <property type="IOpenPlanetControlSettings" name="config" />
    
    <template>
        <Theme />
        
        <Window 
                canClose="false" 
                canMove="false" 
                icon="$FB0" 
                title="OpenPlanet Verification Warning"
                width="200"
                height="120"
                x="-100"
                y="60"
        >
            <frame size="200 100" pos="0 -14">
                <label textsize="15" halign="center" class="text" text="$FB0" pos="100 -1" />
                <label textsize="4" class="text" text="$FB0OpenPlanet Detected" halign="center" pos="100 -18"/>
                <label class="text" text="OpenPlanet has been restricted on this server." halign="center" pos="100 -24"/>
                
                <label class="text" text="Chose the allowed signature mode:" halign="center" pos="100 -32"/>
                <label class="text" textcolor="99ddff" text='{{ string.Join(", ", allowedSignatures) }}' halign="center" pos="100 -36"/>

                <quad bgcolor="ffffff" size="125 22" pos="37.5 -45" />
                <quad bgcolor="5C5F76" size="124.8 21.8" pos="37.6 -45.1" />
                
                <label class="text" textsize="2" text="How do I switch signature mode?" halign="center" pos="100 -47" />
                <label 
                        class="text" 
                        text="you gotta do this jklahdgkj hadfgkj hadfkg hadkgjhafdkjg ahkgj ahkg jhakgj hakdfgh aldfkgh akdfjgh sdfkljghskfdjhg nkjsdfhn klsjdfh gløkadfhj g ølkdfahjpqoi45riyhq4ir9hgeroqwihgvqiasuwgewho8fthyaskidzwjhglkjzasdehdlkjsagxchdlirtesfh gøsofdznhg løwikern hsdflkajng saldfkjng alidfkng aslkdfng lska glkjsdfn glikajsdfn g" 
                        halign="center" 
                        size="110 14" 
                        pos="100 -53"
                        autonewline="1"
                        maxline="3"
                />
                
                <label class="text" text="$iYou will be automatically kicked in {{ config.KickTimeout }} seconds." halign="center" pos="100 -73" id="countdownText" />
                
                <IconButton icon="" text="Disconnect Now" width="30" y="-78" x="84" id="btnDisconnect" action="ManialinkActions/Disconnect" />
            </frame>
        </Window>
    </template>
    
    <script><!--
        *** OnInitialization ***
        ***
        declare lastTime = Now;
        declare countdown = {{ config.KickTimeout }};
        ***
        
        *** OnLoop ***
        ***
        if (countdown > 0 && lastTime + 1000 <= Now) {
            countdown -= 1;
        
            declare countdownText <=> Page.MainFrame.GetFirstChild("countdownText") as CMlLabel;
            countdownText.SetText("$iYou will be automatically kicked in "^countdown^" seconds.");
            
            lastTime = Now;
        }
        
        /* if (countdown <= 0) {
            TriggerPageAction("ManialinkActions/Disconnect");
        } */
        ***
    --></script>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
