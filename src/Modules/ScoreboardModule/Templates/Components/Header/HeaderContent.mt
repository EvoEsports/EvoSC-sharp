<component>
    <using namespace="System.Globalization"/>

    <import component="ScoreboardModule.Components.Header.Logo" as="Logo"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="padding" default="3.4"/>

    <template>
        <frame id="header_content" pos="{{ width / 2.0 }} {{ height / -2.0 }}">
            <label id="header_text_left"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * -0.5 - 4.0 }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   class="text-lg"
                   textfont="{{ Font.Regular }}"
                   halign="right"
                   valign="center2"
            />
            <label id="header_text_right"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * 0.5 + 4.0 }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   class="text-lg"
                   textfont="{{ Font.Regular }}"
                   valign="center2"
            />

            <Logo if='Theme.ScoreboardModule_Logo_URL != ""'/>
        </frame>
    </template>

    <script once="true"><!--
    declare Integer HeaderContentFrameLastUpdate;
    declare CMlFrame HeaderContentFrame;
    declare CMlLabel HeaderContentFrame_TextLeft;
    declare CMlLabel HeaderContentFrame_TextRight;
    
    Void UpdateHeader() {
        HeaderContentFrame_TextLeft.Value = CurrentServerName;
        HeaderContentFrame_TextRight.Value = Map.MapName ^ " by " ^ Map.AuthorNickName;
    }
    --></script>

    <script><!--
        *** OnInitialization ***
        ***
            HeaderContentFrameLastUpdate = 0;
            HeaderContentFrame <=> (Page.MainFrame.GetFirstChild("header_content") as CMlFrame);
            HeaderContentFrame_TextLeft <=> (HeaderContentFrame.GetFirstChild("header_text_left") as CMlLabel);
            HeaderContentFrame_TextRight <=> (HeaderContentFrame.GetFirstChild("header_text_right") as CMlLabel);
        ***
        
        *** OnLoop *** 
        ***
            if(Now > HeaderContentFrameLastUpdate + 2500){
                HeaderContentFrameLastUpdate = Now;
                UpdateHeader();
            }
        ***
    --></script>
</component>
