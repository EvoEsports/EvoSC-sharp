<component>
    <using namespace="EvoSC.Modules.Official.ScoreboardModule.Config"/>

    <property type="IScoreboardSettings" name="settings"/>
    <property type="double" name="accentBarWidth" default="1.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="width"/>
    <property type="double" name="height"/>

    <template>
        <frame id="position_box" pos="{{ x }} {{ y }}">
            <quad if='Theme.ScoreboardModule_PositionBox_ShowAccent == "True"'
                  id="position_box_accent"
                  class="accent-primary"
                  size="{{ accentBarWidth }} {{ height }}"
            />
            <quad id="position_box_background"
                  class="accent-secondary"
                  pos='{{ Theme.ScoreboardModule_PositionBox_ShowAccent=="True" ? accentBarWidth : 0 }}'
                  size='{{ Theme.ScoreboardModule_PositionBox_ShowAccent=="True" ? (width - accentBarWidth) : (width) }} {{ height }}'
                  bgcolor="{{ Theme.ScoreboardModule_PositionBox_Color }}"
                  opacity="{{ Theme.ScoreboardModule_PositionBox_Opacity }}"
            />
            <label id="position_box_position_rank"
                   pos='{{ (width-(Theme.ScoreboardModule_PositionBox_ShowAccent=="True" ? accentBarWidth : 0)) / 2.0 + (Theme.ScoreboardModule_PositionBox_ShowAccent=="True" ? accentBarWidth : 0) }} {{ height / -2.0 + 0.25 }}'
                   valign="center"
                   halign="center"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textfont="{{ Font.ExtraBold }}"
                   textcolor="{{ Theme.ScoreboardModule_PositionBox_TextColor }}"
                   opacity="{{ Theme.ScoreboardModule_PositionBox_TextOpacity }}"
            />
        </frame>
    </template>

    <script once="true">
        <!--
        Void SetPositionBoxColor(CMlFrame backgroundFrame, Vec3 accentColor, Vec3 secondaryColor) {
            if(!{{ Theme.ScoreboardModule_PositionBox_ShowAccent }}) return;
        
            declare accentQuad <=> (backgroundFrame.GetFirstChild("position_box_accent") as CMlQuad);
            declare bgQuad <=> (backgroundFrame.GetFirstChild("position_box_background") as CMlQuad);
            if(accentQuad.BgColor != accentColor) accentQuad.BgColor = accentColor;
            if(bgQuad.BgColor != secondaryColor) bgQuad.BgColor = secondaryColor;
        }
        
        Void SetPositionBoxColor(CMlFrame backgroundFrame, Vec3 color) {
            SetPositionBoxColor(backgroundFrame, color, {! Color.ToMlColor(Theme.UI_AccentSecondary) !});
        }
        
        Void SetPlayerRank(CMlFrame pointsBoxFrame, Integer rank) {
            declare positionLabel = (pointsBoxFrame.GetFirstChild("position_box_position_rank") as CMlLabel);
            positionLabel.Value = rank ^ "";
        }
        -->
    </script>
</component>
