<component>
    <template>
        <entry id="entry" name="Data" halign="center" default="" pos="900 900" size="10 5"/>
        <script><!--        
#Include "TextLib" as TL

main() {        
    declare CMlEntry entry = Page.GetFirstChild("entry") as CMlEntry;
    declare Text lastStamp = "";
    declare Text lastOPValue for This = "";
    while (True) {    
        yield;
        if (System.CurrentLocalDateText != lastStamp) {
            lastStamp = System.CurrentLocalDateText;
            if (lastOPValue != System.ExtraTool_Info) {
                lastOPValue = System.ExtraTool_Info;
                entry.Value = System.ExtraTool_Info;
                TriggerPageAction("OpenPlanetControl/Detect");
            }                                    
        }        
    }
}
--></script>
    </template>
</component>