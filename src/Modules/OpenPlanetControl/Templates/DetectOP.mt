<component>
    <using namespace="EvoSC.Modules.Official.OpenPlanetControl.Config" />
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    
    <property type="IOpenPlanetControlSettings" name="config" />
    
    <template>
    </template>
    <script>
        Void CheckOpenPlanet() {
            TriggerPageAction("OpenPlanetActions/Check/"^System.ExtraTool_Info);
        }
        
        *** OnInitialization ***
        ***
        
        if (!{{ config.SignatureModeCheckEnabled }}) {
            // exit script
            return;
        }
        
        CheckOpenPlanet();

        if (!{{ config.ContinousChecksEnabled }}) {
            // don't continue to the loop if we dont have continous checks enabled
            return;
        }
        ***
        
        *** OnLoop ***
        ***
        
        ***
    </script>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
