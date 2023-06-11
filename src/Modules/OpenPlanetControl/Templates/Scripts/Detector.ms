#Include "TextLib" as TL

main() {        
    declare CMlEntry entry = Page.GetFirstChild("entry") as CMlEntry;
    declare Text lastStamp = "";
    declare Text lastOPValue for This = "";
    lastOPValue = "";
    
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