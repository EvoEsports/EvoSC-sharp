<!--
Void SelectTab(Integer _TabIndex) {
    declare Integer EvoSC_TabGroup_{{ id }}_SelectedTab for This;
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    declare Integer Index = 0;
    
    log(EvoSC_TabGroup_{{ id }}_Tabs );
    
    foreach (TabPageId in EvoSC_TabGroup_{{ id }}_Tabs) {
        declare HeaderText <=> (Page.MainFrame.GetFirstChild(TabPageId ^ "-header-text") as CMlLabel);
        declare Body <=> (Page.MainFrame.GetFirstChild(TabPageId ^ "-body-frame") as CMlFrame);
        
        if (Index == _TabIndex) {
            Body.Visible = True;
            Rectangle_SetBgColor(TabPageId ^ "-header-bg", {! Color.ToMlColor(Theme.UI_SurfaceBgSecondary) !});
            HeaderText.TextColor = {! Color.ToMlColor(Theme.UI_TextSecondary) !};
        } else {
            Body.Visible = False;
            Rectangle_SetBgColor(TabPageId ^ "-header-bg", {! Color.ToMlColor(Theme.UI_SurfaceBgPrimary) !});
            HeaderText.TextColor = {! Color.ToMlColor(Theme.UI_TextPrimary) !};
        }
        
        Index += 1;
    }
    
    EvoSC_TabGroup_{{ id }}_SelectedTab = _TabIndex;
}

Void InitializeTabs() {
    declare Text[] Tabs;

    Page.GetClassChildren("evosc-tabpage-frame", Page.MainFrame, True);
    declare Integer Index = 0;
    declare Real TabHeaderX = 0.;
    
    foreach (TabPageFrame in Page.GetClassChildren_Result) {
        declare Text TabPageId = TabPageFrame.ControlId;
        
        log("init: " ^ TabPageId);
        
        Tabs.add(TabPageId);
        log(Tabs);
        declare HeaderFrame <=> (Page.MainFrame.GetFirstChild(TabPageId ^ "-header-frame") as CMlFrame);
        declare HeaderBg <=> (Page.MainFrame.GetFirstChild(TabPageId ^ "-header-bg") as CMlQuad);
        
        HeaderFrame.RelativePosition_V3 = <TabHeaderX, 0.>;
        
        Index += 1;
        TabHeaderX += HeaderBg.Size.X + 2.0;
    }
    
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    EvoSC_TabGroup_{{ id }}_Tabs = Tabs;
}

*** OnInitialization ***
***
declare Integer EvoSC_TabGroup_{{ id }}_SelectedTab for This = {{ selectedIndex }};
declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;

InitializeTabs();
SelectTab({{ selectedIndex }});
***

*** OnMouseClick ***
***
{
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    declare TabIndex = 0;
    
    foreach (TabPageId in EvoSC_TabGroup_{{ id }}_Tabs) {
        if (Event.Control.DataAttributeGet("id") == TabPageId) {
            SelectTab(TabIndex);
            break;
        }
        
        TabIndex += 1;
    }
}
***
-->
