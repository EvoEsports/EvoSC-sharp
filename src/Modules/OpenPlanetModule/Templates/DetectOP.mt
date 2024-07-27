<component>
    <using namespace="EvoSC.Modules.Official.OpenPlanetModule.Config" />
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    
    <property type="IOpenPlanetControlSettings" name="config" />
    
    <template>
    </template>
    <script><!--
        Void CheckOpenPlanet() {
            TriggerPageAction("OpenPlanetActions/Check/"^TextLib::URLEncode(System.ExtraTool_Info));
        }
        
        *** OnInitialization ***
        ***
        declare lastTime = Now;
        declare lastToolInfo = System.ExtraTool_Info;
        
        if (!{{ config.SignatureModeCheckEnabled }}) {
            // exit script
            return;
        }
        
        CheckOpenPlanet();

        if (!{{ config.ContinuousChecksEnabled }}) {
            // don't continue to the loop if we dont have continous checks enabled
            return;
        }
        ***
        
        *** OnLoop ***
        ***
        if (lastTime + {{ config.CheckInterval }} <= Now) {
            if (lastToolInfo != System.ExtraTool_Info) {
              CheckOpenPlanet();
            }
            
            lastTime = Now;
            lastToolInfo = System.ExtraTool_Info;
        }
        ***
    --></script>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
