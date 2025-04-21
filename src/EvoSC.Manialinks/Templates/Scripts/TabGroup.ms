<!--
Void SelectTab(Text _NewTabPageId) {
    declare Text EvoSC_TabGroup_{{ id }}_SelectedTab for This;
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    
    if (!EvoSC_TabGroup_{{ id }}_Tabs.exists(_NewTabPageId)) {
        return;
    }
    
    foreach (TabPageId in EvoSC_TabGroup_{{ id }}_Tabs) {
        declare HeaderText <=> (Page.MainFrame.GetFirstChild(TabPageId ^ "-header-text") as CMlLabel);
        declare Body <=> (Page.MainFrame.GetFirstChild(TabPageId ^ "-body-frame") as CMlFrame);
        
        if (TabPageId == _NewTabPageId) {
            Body.Visible = True;
            Rectangle_SetBgColor(TabPageId ^ "-header-bg", {! Color.ToMlColor(Theme.UI_SurfaceBgSecondary) !});
            HeaderText.TextColor = {! Color.ToMlColor(Theme.UI_TextSecondary) !};
        } else {
            Body.Visible = False;
            Rectangle_SetBgColor(TabPageId ^ "-header-bg", {! Color.ToMlColor(Theme.UI_SurfaceBgPrimary) !});
            HeaderText.TextColor = {! Color.ToMlColor(Theme.UI_TextPrimary) !};
        }
    }
    
    EvoSC_TabGroup_{{ id }}_SelectedTab = _NewTabPageId;
    
    {
        declare Text TabPageId = _NewTabPageId;
        +++ OnTabChanged +++
    }
}

Void InitializeTabs() {
    declare Text[] Tabs;

    Page.GetClassChildren("evosc-tabpage-frame", Page.MainFrame, True);
    declare Integer Index = 0;
    declare Real TabHeaderX = 0.;
    
    foreach (TabPageFrame in Page.GetClassChildren_Result) {
        declare Text TabPageId = TabPageFrame.ControlId;
        
        Tabs.add(TabPageId);
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
declare Text EvoSC_TabGroup_{{ id }}_SelectedTab for This = "{{ selectedTab }}";
declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;

InitializeTabs();
SelectTab("{{ selectedTab }}");
***

*** OnMouseClick ***
***
{
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    declare TabPageId = Event.Control.DataAttributeGet("id");
    
    if (EvoSC_TabGroup_{{ id }}_Tabs.exists(TabPageId)) {
        SelectTab(TabPageId);
    }
}
***

*** OnMouseOver ***
***
{
    declare Text EvoSC_TabGroup_{{ id }}_SelectedTab for This;
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    declare TabPageId = Event.Control.DataAttributeGet("id");
    
    if (TabPageId != EvoSC_TabGroup_{{ id }}_SelectedTab && EvoSC_TabGroup_{{ id }}_Tabs.exists(TabPageId)) {
        Rectangle_SetBgColor(TabPageId ^ "-header-bg", {! Color.ToMlColor(Theme.UI_BgHighlight) !});
    }
}
***

*** OnMouseOut ***
***
{
    declare Text[] EvoSC_TabGroup_{{ id }}_Tabs for This;
    declare TabPageId = Event.Control.DataAttributeGet("id");
    
    if (TabPageId != EvoSC_TabGroup_{{ id }}_SelectedTab && EvoSC_TabGroup_{{ id }}_Tabs.exists(TabPageId)) {
        Rectangle_SetBgColor(TabPageId ^ "-header-bg", {! Color.ToMlColor(Theme.UI_SurfaceBgPrimary) !});
    }
}
***
-->
