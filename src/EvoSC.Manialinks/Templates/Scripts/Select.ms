<!--

*** OnInitialization ***
***
declare Text[] EvoSC_UI_SelectIds = [];

{
    Page.GetClassChildren("evosc-select-dpanel", Page.MainFrame, True);
            
    foreach (DPanel in Page.GetClassChildren_Result) {
        log(DPanel.DataAttributeGet("id"));
        DPanel.Hide();
        EvoSC_UI_SelectIds.add(DPanel.DataAttributeGet("id"));
    }
}
***

*** OnMouseClick ***
***
if (Event.Control.HasClass("evosc-select-trigger")) {
    declare Text SelectId = Event.Control.DataAttributeGet("id");
    declare DPanel <=> Page.MainFrame.GetFirstChild("evosc-select-dpanel-" ^ SelectId) as CMlFrame;
    
    DPanel.Show();
} else {
    log(EvoSC_UI_SelectIds);
    foreach (SelectId in EvoSC_UI_SelectIds) {
        log(SelectId);
        declare DPanel <=> Page.MainFrame.GetFirstChild("evosc-select-dpanel-" ^ SelectId) as CMlFrame;
        DPanel.Hide();
    }
}
***

-->