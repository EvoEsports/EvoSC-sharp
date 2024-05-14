<!--
*** OnInitialization ***
***
declare Text[] EvoSC_UI_DropdownIds = [];
{
    Page.GetClassChildren("dropdown-slotpanel", Page.MainFrame, True);
            
    foreach (Panel in Page.GetClassChildren_Result) {
        Panel.Visible = False;
        EvoSC_UI_DropdownIds.add(Panel.DataAttributeGet("id"));
    }
}
***

*** OnMouseClick ***
***
    foreach (id in EvoSC_UI_DropdownIds) {
         declare Panel <=> Page.MainFrame.GetFirstChild(id^"-btn-slotpanel") as CMlFrame;
         Panel.Hide();
     }

    if (Event.Control.HasClass("evosc-dropdown-btn")) {
        declare ControlId = Event.Control.DataAttributeGet("id");
        declare Panel <=> Page.MainFrame.GetFirstChild(ControlId^"-slotpanel") as CMlFrame;
        
        if (Panel.Visible) {
            Panel.Hide();
        } else {
            Panel.Show();
        }
    }
***

-->