<component>
    <property type="string" name="id"/>
    <property type="string" name="accentColor"/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="0.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="opacity" default="1.0"/>
    <property type="int" name="zIndex" default="0"/>
    <property type="double" name="rowHeight" default="-1.0"/>

    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}" z-index="{{ zIndex }}">
            <!-- 0: top left corner -->
            <frame size="0.5 0.5">
                <quad size="1 1"
                      modulatecolor="{{ accentColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="{{ opacity }}"/>
            </frame>

            <!-- 1: bottom left corner -->
            <frame size="0.5 0.5" pos="0 {{ -h }}" rot="270">
                <quad size="1 1"
                      modulatecolor="{{ accentColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="{{ opacity }}"
                />
            </frame>

            <!-- 2: center quad -->
            <quad pos="0.5 0" size="{{ w - 0.5 }} {{ h }}" bgcolor="{{ accentColor }}" opacity="{{ opacity }}"/>

            <!-- 3: right bar -->
            <quad pos="0 -0.5" size="0.5 {{ h - 1.0 }}" bgcolor="{{ accentColor }}" opacity="{{ opacity }}"/>
        </frame>
    </template>

    <script once="true">
        <!--        
        declare Real ScrollHandleHeight;
        declare Real ScrollBackgroundHeight;
        declare CMlFrame ScrollHandleFrame;
        
        Void SetScrollbarHeight(CMlFrame scrollbarFrame, Real height) {
            scrollbarFrame.Size.Y = height;
            (scrollbarFrame.Controls[1] as CMlFrame).RelativePosition_V3.Y = height * -1.0;
            (scrollbarFrame.Controls[2] as CMlQuad).Size.Y = height;
            (scrollbarFrame.Controls[3] as CMlQuad).Size.Y = height - 1.0;
            ScrollHandleHeight = height;
        }
        
        Void SetScrollbarPosition(CMlFrame rowsFrame, Integer playerRowsFilled, Integer rowsShown) {
            declare Real maxScroll = playerRowsFilled * {{ rowHeight }};
            declare Real adjustedViewPortHeight = maxScroll - (rowsShown * {{ rowHeight }});
            declare Real scrollOffset = rowsFrame.ScrollOffset.Y;
            declare Real scrollableHeight = ScrollBackgroundHeight - ScrollHandleHeight;
            declare Real ratio = scrollOffset / adjustedViewPortHeight;
            ScrollHandleFrame.RelativePosition_V3.Y = ratio * scrollableHeight * -1.0;
        }
        
        *** OnInitialization *** 
        ***
            ScrollHandleFrame <=> (Page.MainFrame.GetFirstChild("scrollbar_handle") as CMlFrame);
            ScrollHandleHeight = 1.0;
            ScrollBackgroundHeight = 1.0;
        ***
        
        *** OnLoop *** 
        ***
            SetScrollbarPosition(RowsFrame, PlayerRowsFilled, PlayerRowsVisible);
        ***
        -->
    </script>
</component>