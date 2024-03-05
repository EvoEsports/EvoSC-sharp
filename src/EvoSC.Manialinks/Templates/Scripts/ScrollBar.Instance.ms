<!--
*** OnInitialization ***
***
    declare CMlFrame ScrollFrame_{{ forFrame }} <=> (Page.MainFrame.GetFirstChild("{{ forFrame }}") as CMlFrame);
    declare Boolean Scrollbar_Hover_{{ id }} = False;
    declare CMlQuad ScrollBar_Quad_{{ id }} <=> (Page.MainFrame.GetFirstChild("scrollbar_quad_{{ id }}") as CMlQuad);
***

*** OnLoop ***
***
    if (!Scrollbar_Hover_{{ id }}) {
        declare scrollBarPos = ScrollFrame_{{ forFrame }}.ScrollOffset.Y/{{ max }}*{{ length-5 }};
        ScrollBar_Quad_{{ id }}.RelativePosition_V3 = <ScrollBar_Quad_{{ id }}.RelativePosition_V3.X, -scrollBarPos>;
    }
***

*** OnMouseOver ***
***
    if (Event.Control.ControlId == "scrollbar_quad_{{ id }}") {
        Scrollbar_Hover_{{ id }} = True;
    }
***

*** OnMouseUp ***
***
    Scrollbar_Hover_{{ id }} = False;
***

*** OnMouseDrag ***
***
    if (Scrollbar_Hover_{{ id }}) {
        declare Vec2 FramePos = ScrollFrame_{{ forFrame }}.AbsolutePosition_V3;
        declare Real NewY = MouseY - FramePos.Y;
        
        if (NewY > 0) { NewY = 0.; }
        else if (NewY < -{{ length-5 }}) { NewY = -1.0*{{ length-5 }}; }
        
        ScrollBar_Quad_{{ id }}.RelativePosition_V3 = <0., NewY>;
        
        declare Real ScrollPercentage = NewY/{{ length - 5 }};
        declare Vec2 CurrScrollOffset = ScrollFrame_{{ forFrame }}.ScrollOffset;
        
        declare Real NewScrollOffsetY = ScrollFrame_{{ forFrame }}.ScrollMin.Y 
                                 + ScrollPercentage
                                 * (ScrollFrame_{{ forFrame }}.ScrollMin.Y - ScrollFrame_{{ forFrame }}.ScrollMax.Y);
        
        ScrollFrame_{{ forFrame }}.ScrollOffset = <CurrScrollOffset.X, NewScrollOffsetY>;
    }
***
-->
