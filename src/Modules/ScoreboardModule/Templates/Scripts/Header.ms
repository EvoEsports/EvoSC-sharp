<!--
declare Integer HeaderContentFrameLastUpdate;
declare CMlFrame HeaderContentFrame;
declare CMlLabel HeaderContentFrame_TextLeft;
declare CMlLabel HeaderContentFrame_TextLeftSmall;
declare CMlLabel HeaderContentFrame_TextRight;
declare CMlLabel HeaderContentFrame_TextRightSmall;

Text GetNadeoScoreboardInfoText() {
    declare netread Text Net_TMGame_ScoresTable_FooterInfo for Teams[0];
    return Net_TMGame_ScoresTable_FooterInfo;
}

Boolean IsWarmUp () {
    return TL::EndsWith("Warm up", GetNadeoScoreboardInfoText());
}

Vec3 GetHeaderPrimaryTextColor() {
    return CL::HexToRgb("{{ Theme.ScoreboardModule_Text_Color }}");
}

Vec3 GetHeaderWarmUpTextColor(){
    return CL::HexToRgb("{{ Theme.ScoreboardModule_WarmUp_Color }}");
}

Text GetPlayerCountText() {
    switch (Players.count) {
        case 0: return _("No player");
        case 1: return _("1 player");
    }
    
    return TL::Compose(_("%1 players"), TL::ToText(Players.count));
}

Text GetRoundsLabelText() {
    declare Integer EvoSC_PointsLimit for UI = -1;
    declare Integer EvoSC_RoundNumber for UI = -1;
    declare Integer EvoSC_RoundsPerMap for UI = -1;
    
    if(EvoSC_RoundsPerMap == -1){
        if(EvoSC_PointsLimit == -1){
            return TL::ToUpperCase(GetNadeoScoreboardInfoText());
        }
    
        return TL::ToUpperCase("Round " ^ EvoSC_RoundNumber);
    }
    
    return TL::ToUpperCase("Round " ^ EvoSC_RoundNumber ^ " of " ^ EvoSC_RoundsPerMap);
}

Text GetRoundsLabelWarmUpText() {
    declare Integer EvoSC_RoundNumber for UI = -1;
    declare Integer EvoSC_WarmUpCount for UI = -1;
    
    if(EvoSC_WarmUpCount == -1){
        return TL::ToUpperCase("Round " ^ EvoSC_RoundNumber);
    }
    
    return TL::ToUpperCase("Round " ^ EvoSC_RoundNumber ^ " of " ^ EvoSC_WarmUpCount);
}

Text GetPointsLimitText() {
    declare Integer EvoSC_PointsLimit for UI = -1;
    
    if(EvoSC_PointsLimit == -1){
        return TL::ToUpperCase(GetPlayerCountText());
    }
    
    return TL::ToUpperCase("Points Limit " ^ EvoSC_PointsLimit);
}

Void UpdateHeaderSmallTextColors() {
    declare Vec3 textColor = <1.0, 1.0, 1.0>;
    if(IsWarmUp()){
        textColor = GetHeaderWarmUpTextColor();
    }else{
        textColor = GetHeaderPrimaryTextColor();
    }
    HeaderContentFrame_TextRightSmall.TextColor = textColor;
    HeaderContentFrame_TextLeftSmall.TextColor = textColor;
}

Void UpdateHeaderContent() {
    HeaderContentFrame_TextLeft.Value = TL::StripFormatting(CurrentServerName);
    HeaderContentFrame_TextRight.Value = TL::StripFormatting(TL::Trim(Map.MapName ^ " by " ^ Map.AuthorNickName));
    
    if(IsWarmUp()){
        HeaderContentFrame_TextLeftSmall.Value = TL::ToUpperCase(GetNadeoScoreboardInfoText());
        HeaderContentFrame_TextRightSmall.Value = GetRoundsLabelWarmUpText();
    }else{
        HeaderContentFrame_TextLeftSmall.Value = GetPointsLimitText();
        HeaderContentFrame_TextRightSmall.Value = GetRoundsLabelText();
    }
    
    UpdateHeaderSmallTextColors();
}

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
    if(Now > HeaderContentFrameLastUpdate + 750){
        HeaderContentFrameLastUpdate = Now;
        UpdateHeaderContent();
    }
***
-->
