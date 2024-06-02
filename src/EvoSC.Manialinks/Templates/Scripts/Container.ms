<!--
*** OnInitialization ***
***
{
  declare Frame <=> (Page.MainFrame.GetFirstChild("{{ Util.DefaultOrRandomId("evosc_container", id) }}") as CMlFrame);
  
  if ({{ scrollable }}) {
    Frame.ScrollActive = True;
    Frame.ScrollMin = <0.0, 0.0>;
    Frame.ScrollMax = <{{ scrollWidth }}*1.0, {{ scrollHeight }}*1.0>;
    
    if ({{ scrollGridSnap }}) {
        Frame.ScrollGridSnap = True;
        Frame.ScrollGrid = <{{ scrollGridX }}*1.0, {{ scrollGridY }}*1.0>;
    }
  }
}
***
-->