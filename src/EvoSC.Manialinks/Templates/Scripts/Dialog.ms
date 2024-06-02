<!--
Void Internal_Dialog_Hide() {
    declare containerFrame <=> (Page.MainFrame.GetFirstChild("containerFrame")) as CMlFrame;
      declare backdrop <=> (Page.MainFrame.GetFirstChild("evosc-backdrop")) as CMlFrame;
      
      containerFrame.Hide();
      backdrop.Hide();
}

*** OnMouseClick ***
***
if (Event.Control.ControlId == "btnCancel" || Event.Control.ControlId == "btnOk") {
  Internal_Dialog_Hide();
}
***
-->