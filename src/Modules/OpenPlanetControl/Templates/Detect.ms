#Include "TextLib" as TL

main() {        
    declare CMlEntry entry = Page.GetFirstChild("entry") as CMlEntry;                
    TriggerPageAction("," ^ TL::URLEncode(System.ExtraTool_Info));
}
        