<!--
*** OnInitialization ***
***
declare Text[] EvoSC_UI_DropdownIds = [];

{
    Page.GetClassChildren("evosc-dropdown-slotpanel", Page.MainFrame, True);
            
    foreach (Panel in Page.GetClassChildren_Result) {
        Panel.Visible = False;
        EvoSC_UI_DropdownIds.add(Panel.DataAttributeGet("id"));
    }
}
***

*** OnMouseClick ***
***
    if (Event.Control.HasClass("evosc-dropdown-btn")) {
        declare ControlId = Event.Control.DataAttributeGet("id");
        declare Panel <=> Page.MainFrame.GetFirstChild(ControlId^"-slotpanel") as CMlFrame;
        
        Panel.Visible = !Panel.Visible;
        log(Panel.Size);
    } else {
         foreach (id in EvoSC_UI_DropdownIds) {
             declare Panel <=> Page.MainFrame.GetFirstChild(id^"-btn-slotpanel") as CMlFrame;
             Panel.Visible = False;
         }
     }
***

-->