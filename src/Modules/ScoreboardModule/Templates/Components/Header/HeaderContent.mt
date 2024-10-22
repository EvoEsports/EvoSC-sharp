<component>
    <using namespace="System.Globalization"/>

    <import component="ScoreboardModule.Components.Header.Logo" as="Logo"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="padding" default="3.4"/>

    <template>
        <frame id="header_content" pos="{{ width / 2.0 }} {{ height / -2.0 }}">
            <label id="header_text_left"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * -0.5 - 4.0 }} 1.75"
                   size="{{ width/2.0-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2.0 }} {{ height }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   class="text-lg"
                   textfont="{{ Font.Regular }}"
                   halign="right"
                   valign="center2"
            />
            <label id="header_text_left_small"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * -0.5 - 4.0 }} -1.75"
                   size="{{ width/2.0-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2.0 }} {{ height }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   class="text-primary"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize*0.8 }}"
                   halign="right"
                   valign="center2"
                   opacity="0.8"
            />
            <label id="header_text_right"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * 0.5 + 4.0 }} 1.75"
                   size="{{ width/2.0-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2.0 }} {{ height }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   class="text-lg"
                   textfont="{{ Font.Regular }}"
                   valign="center2"
            />
            <label id="header_text_right_small"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * 0.5 + 4.0 }} -1.75"
                   size="{{ width/2.0-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2.0 }} {{ height }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   class="text-primary"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize*0.8 }}"
                   valign="center2"
                   opacity="0.8"
            />

            <Logo if='Theme.ScoreboardModule_Logo_URL != ""'/>
        </frame>
    </template>

    <script once="true"><!--
    declare Integer HeaderContentFrameLastUpdate;
    declare CMlFrame HeaderContentFrame;
    declare CMlLabel HeaderContentFrame_TextLeft;
    declare CMlLabel HeaderContentFrame_TextLeftSmall;
    declare CMlLabel HeaderContentFrame_TextRight;
    declare CMlLabel HeaderContentFrame_TextRightSmall;
    
    Text GetRoundsLabelText() {
        declare Integer EvoSC_RoundNumber for UI = -1;
        declare Integer EvoSC_RoundsPerMap for UI = -1;
        return TL::ToUpperCase("Round " ^ EvoSC_RoundNumber ^ " of " ^ EvoSC_RoundsPerMap);
    }
    
    Text GetRoundsLabelWarmUpText() {
        declare Integer EvoSC_RoundNumber for UI = -1;
        declare Integer EvoSC_WarmUpCount for UI = -1;
        return "$f90" ^ TL::ToUpperCase("Round " ^ EvoSC_RoundNumber ^ " of " ^ EvoSC_WarmUpCount);
    }
    
    Text GetPointsLimitText() {
        declare Integer EvoSC_PointsLimit for UI = -1;
        return TL::ToUpperCase("Points Limit " ^ EvoSC_PointsLimit);
    }
    
    Text GetPointsLimitWarmUpText() {
        return "$f90WARM UP";
    }
    
    Void UpdateHeader() {
        declare Boolean EvoSC_WarmUpActive for UI = False;
        
        HeaderContentFrame_TextLeft.Value = TL::StripFormatting(CurrentServerName);
        HeaderContentFrame_TextRight.Value = TL::StripFormatting(TL::Trim(Map.MapName ^ " by " ^ Map.AuthorNickName));
        
        if(EvoSC_WarmUpActive){
            HeaderContentFrame_TextLeftSmall.Value = GetPointsLimitWarmUpText();
            HeaderContentFrame_TextRightSmall.Value = GetRoundsLabelWarmUpText();
        }else{
            HeaderContentFrame_TextLeftSmall.Value = GetPointsLimitText();
            HeaderContentFrame_TextRightSmall.Value = GetRoundsLabelText();
        }
    }
    --></script>

    <script><!--
        *** OnInitialization ***
        ***
            HeaderContentFrameLastUpdate = 0;
            HeaderContentFrame <=> (Page.MainFrame.GetFirstChild("header_content") as CMlFrame);
            HeaderContentFrame_TextLeft <=> (HeaderContentFrame.GetFirstChild("header_text_left") as CMlLabel);
            HeaderContentFrame_TextLeftSmall <=> (HeaderContentFrame.GetFirstChild("header_text_left_small") as CMlLabel);
            HeaderContentFrame_TextRight <=> (HeaderContentFrame.GetFirstChild("header_text_right") as CMlLabel);
            HeaderContentFrame_TextRightSmall <=> (HeaderContentFrame.GetFirstChild("header_text_right_small") as CMlLabel);
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
