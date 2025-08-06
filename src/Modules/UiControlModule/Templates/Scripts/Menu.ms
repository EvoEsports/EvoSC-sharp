<!--
Void AttachDataToCheckboxes() {
    declare checkboxList <=> (Page.MainFrame.GetFirstChild("checkboxList") as CMlFrame);
    declare Text[] moduleNames = [{! string.Join(",", moduleNames.Select(name => "\u0022" + name + "\u0022")) !}];
    declare moduleIndex = 1; //Start at 1 because of container.
    
    foreach(name in moduleNames){
        declare Control <=> checkboxList.Controls[moduleIndex];
        declare Text moduleName for Control = name;
        moduleIndex += 1;
    }
}

*** OnInitialization ***
***
    AttachDataToCheckboxes();
    declare Text[] hiddenModules = [{! string.Join(",", hiddenModules.Select(name => "\u0022" + name + "\u0022")) !}];
    declare CMlEntry hiddenManialinksEntry <=> (Page.MainFrame.GetFirstChild("hiddenManialinksEntry") as CMlEntry);
***

*** OnCheckboxToggle ***
***
    declare Text moduleName for Control = "";
    declare Boolean showModule = !IsChecked;
    
    if(showModule){
        if(hiddenModules.exists(moduleName)){
            hiddenModules.remove(moduleName);
        }
    }else{
        hiddenModules.add(moduleName);
    }
    
    hiddenManialinksEntry.Value = TextLib::Join("|", hiddenModules);
***
-->