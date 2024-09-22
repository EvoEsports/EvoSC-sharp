<component>
    <import component="ScoreboardModule.ComponentsNew.Header.Logo" as="Logo"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="padding" default="3.8"/>

    <template>
        <frame id="header_content"
               pos="{{ padding }} {{ -padding }}"
               size="{{ width - padding * 2.0 }} {{ height - padding }}"
        >
            <label id="big_text"
                   class="text-primary"
                   text="BIG TEXT"
                   textsize="{{ Theme.UI_FontSize*4 }}"
            />
            <label id="small_text"
                   class="text-primary"
                   pos="0 -5.5"
                   text="SMALL TEXT"
                   textsize="{{ Theme.UI_FontSize }}"
            />
            <label id="smaller_text"
                   class="text-primary"
                   pos="0 -8.5"
                   text="SMALLER TEXT"
                   textsize="{{ Theme.UI_FontSize*0.8 }}"
                   opacity="0.6"
            />

            <Logo x="{{ (width - padding * 2.0) / 2.0 }}"
                  y="{{ (height - padding * 2.0) / -2.0 }}"
                  width="{{ height / 2.0 }}"
                  height="{{ height / 2.0 }}"
            />
        </frame>
    </template>

    <script once="true"><!--
    declare Integer HeaderContentFrameLastUpdate;
    declare CMlFrame HeaderContentFrame;
    declare CMlLabel HeaderContentFrame_BigText;
    declare CMlLabel HeaderContentFrame_SmallText;
    declare CMlLabel HeaderContentFrame_SmallerText;
    
    Void UpdateHeader() {
        HeaderContentFrame_BigText.Value = CurrentServerName;
        HeaderContentFrame_SmallText.Value = Map.MapName;
        HeaderContentFrame_SmallerText.Value = Map.AuthorNickName;
    }
    --></script>

    <script><!--
        *** OnInitialization ***
        ***
            HeaderContentFrameLastUpdate = 0;
            HeaderContentFrame <=> (Page.MainFrame.GetFirstChild("header_content") as CMlFrame);
            HeaderContentFrame_BigText <=> (HeaderContentFrame.GetFirstChild("big_text") as CMlLabel);
            HeaderContentFrame_SmallText <=> (HeaderContentFrame.GetFirstChild("small_text") as CMlLabel);
            HeaderContentFrame_SmallerText <=> (HeaderContentFrame.GetFirstChild("smaller_text") as CMlLabel);
        ***
        
        *** OnLoop *** 
        ***
            if(Now > HeaderContentFrameLastUpdate + 1000){
                HeaderContentFrameLastUpdate = Now;
                UpdateHeader();
            }
        ***
    --></script>
</component>
