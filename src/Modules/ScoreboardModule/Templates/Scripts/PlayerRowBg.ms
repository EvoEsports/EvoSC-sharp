<!--
Void RowMouseOver(CMlFrame backgroundFrame) {
    declare backgroundQuad = (backgroundFrame.Controls[0] as CMlQuad);
    backgroundQuad.BgColor = {! Color.ToMlColor(Theme.ScoreboardModule_Background_Hover_Color) !};
    backgroundQuad.Opacity = {{ Theme.ScoreboardModule_Background_Hover_Opacity }};
}

Void RowMouseOut(CMlFrame backgroundFrame) {
    declare backgroundQuad = (backgroundFrame.Controls[0] as CMlQuad);
    backgroundQuad.BgColor = {! Color.ToMlColor(Theme.ScoreboardModule_Background_Row_Color) !};
    backgroundQuad.Opacity = {{ Theme.ScoreboardModule_Background_Row_Opacity }};
}

Void SetPlayerHighlightColor(CMlFrame playerRow, Vec3 color) {
    declare customGradient = (playerRow.GetFirstChild("custom_gradient") as CMlQuad);
    customGradient.ModulateColor = color;
    customGradient.Show();
}

Void ResetPlayerHighlightColor(CMlFrame playerRow) {
    declare customGradient = (playerRow.GetFirstChild("custom_gradient") as CMlQuad);
    customGradient.Hide();
}
-->
