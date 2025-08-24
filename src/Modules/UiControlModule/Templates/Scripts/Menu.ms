<!--
declare Text[] moduleNames;
declare Text[] hiddenModules;
declare CMlEntry hiddenManialinksEntry;

Void AttachDataToCheckboxes() {
    declare checkboxList <=> (Page.MainFrame.GetFirstChild("checkboxList") as CMlFrame);
    declare moduleIndex = 1; //Start at 1 because of container.
    
    foreach(name in moduleNames){
        declare Control <=> checkboxList.Controls[moduleIndex];
        declare Text moduleName for Control = name;
        moduleIndex += 1;
    }
}

Void UpdateHiddenEntry(){
    hiddenManialinksEntry.Value = TextLib::Join("|", hiddenModules);
}

Void HideManialink(Text moduleName){
    if(!hiddenModules.exists(moduleName)){
        hiddenModules.add(moduleName);
    }
}

Void ShowManialink(Text moduleName){
    if(hiddenModules.exists(moduleName)){
        hiddenModules.remove(moduleName);
    }
}

Void SetCheckboxStateAll(Boolean isChecked){
    declare checkboxIndex = 0;
    
    foreach(moduleName in moduleNames){
        SetCheckboxState("checkbox_" ^ checkboxIndex, isChecked);
        
        if(isChecked){
            HideManialink(moduleName);
        }else{
            ShowManialink(moduleName);
        }
        
        checkboxIndex += 1;
    }
    
    UpdateHiddenEntry();
}

*** OnInitialization ***
***
    moduleNames = [{! string.Join(",", moduleNames.Select(name => "\u0022" + name + "\u0022")) !}];
    hiddenModules = [{! string.Join(",", hiddenModules.Select(name => "\u0022" + name + "\u0022")) !}];
    hiddenManialinksEntry <=> (Page.MainFrame.GetFirstChild("hiddenManialinksEntry") as CMlEntry);
    
    AttachDataToCheckboxes();
***

*** OnCheckboxToggle ***
***
    declare Text moduleName for Control = "";
    declare Boolean showModule = !IsChecked;
    
    if(showModule){
        ShowManialink(moduleName);
    }else{
        HideManialink(moduleName);
    }
    
    UpdateHiddenEntry();
***

*** OnMouseClick ***
***
    if(Event.Control.ControlId == "btnSelectAll"){
        SetCheckboxStateAll(True);
    }else if(Event.Control.ControlId == "btnUnselectAll"){
        SetCheckboxStateAll(False);
    }
***
-->