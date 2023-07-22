<!--
*** OnInitialization ***
***
declare Text[] EvoSC_UI_DropdownIds = [];

{
    Page.GetClassChildren("evosc-dropdown-frame", Page.MainFrame, True);
            
    foreach (ctrl in Page.GetClassChildren_Result) {
        log(ctrl.ControlId^"-panel");
        declare Panel <=> Page.MainFrame.GetFirstChild(ctrl.ControlId^"-panel") as CMlFrame;
        Panel.Visible = False;
        EvoSC_UI_DropdownIds.add(ctrl.ControlId);
    }
}
***

*** OnMouseClick ***
***
    if (Event.Control.HasClass("evosc-dropdown-btn")) {
        declare ControlId = Event.Control.DataAttributeGet("id");
        declare Panel <=> Page.MainFrame.GetFirstChild(ControlId^"-panel") as CMlFrame;
        
        Panel.Visible = !Panel.Visible;
    } else {
         foreach (id in EvoSC_UI_DropdownIds) {
             declare Panel <=> Page.MainFrame.GetFirstChild(id^"-panel") as CMlFrame;
             Panel.Visible = False;
         }
     }
***

-->