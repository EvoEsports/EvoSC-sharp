<component>
    <property type="string" name="id"/>
    <property type="string" name="accentColor"/>
    <property type="int" name="visiblePlayers"/>
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
        declare CMlFrame ScrollHandleFrame;
        declare CMlFrame ScrollBgFrame;
        
        Void SetScrollbarPosition(CMlFrame rowsFrame, Integer playerRowsFilled, Integer rowsShown) {
            if(playerRowsFilled > {{ visiblePlayers }}){
                ScrollHandleFrame.Show();
            }else{
                ScrollHandleFrame.Hide();
                return;
            }
            
            declare Real maxScroll = playerRowsFilled * {{ rowHeight }};
            declare Real adjustedViewPortHeight = maxScroll - (rowsShown * {{ rowHeight }});
            declare Real scrollOffset = rowsFrame.ScrollOffset.Y;
            declare Real scrollableHeight = ScrollBgFrame.Size.Y - ScrollHandleFrame.Size.Y;
            declare Real ratio = scrollOffset / adjustedViewPortHeight;
            ScrollHandleFrame.RelativePosition_V3.Y = ratio * scrollableHeight * -1.0;
        }
        
        *** OnInitialization *** 
        ***
            ScrollHandleFrame <=> (Page.MainFrame.GetFirstChild("scrollbar_handle") as CMlFrame);
            ScrollBgFrame <=> (Page.MainFrame.GetFirstChild("scrollbar_bg") as CMlFrame);
        ***
        
        *** OnLoop *** 
        ***
            SetScrollbarPosition(RowsFrame, PlayerRowsFilled, PlayerRowsVisible);
        ***
        -->
    </script>
</component>