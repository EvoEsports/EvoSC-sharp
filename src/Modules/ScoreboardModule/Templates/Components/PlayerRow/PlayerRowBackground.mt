<component>
    <property type="string" name="id"/>
    <property type="double" name="rowHeight"/>
    <property type="double" name="padding"/>
    <property type="double" name="w"/>
    <property type="double" name="h"/>
    <property type="double" name="x"/>

    <template>
        <frame id="{{ id }}"
               pos="{{ x }}"
        >
            <quad id="background"
                  size="{{ w }} {{ h }}"
                  bgcolor="{{ Theme.ScoreboardModule_Background_Row_Color }}"
                  opacity="{{ Theme.ScoreboardModule_Background_Row_Opacity }}"
            />
            <frame pos="{{ w / 2.0 }} {{ h / -2.0 }}">
                <quad id="custom_gradient"
                      size="{{ h }} {{ w }}"
                      pos="{{ w / -2.0 }} {{ h / -2.0 }}"
                      rot="-90"
                      image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                      modulatecolor="{{ Theme.UI_AccentPrimary }}"
                      opacity="0.25"
                />
            </frame>
        </frame>
    </template>

    <script once="true">
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
    </script>
</component>