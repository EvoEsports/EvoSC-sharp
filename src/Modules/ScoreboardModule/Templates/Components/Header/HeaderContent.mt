<component>
    <using namespace="System.Globalization"/>

    <import component="ScoreboardModule.Components.Header.Logo" as="Logo"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="padding" default="3.4"/>

    <template>
        <frame id="header_content" pos="{{ width / 2f }} {{ height / -2f }}">
            <label id="header_text_left"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * -0.5 - 4f }} 1.75"
                   size="{{ width/2f-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2f }} {{ height }}"
                   textcolor="{{ Theme.ScoreboardModule_Header_Text_Color }}"
                   class="text-lg"
                   textfont="{{ Font.Regular }}"
                   halign="right"
                   valign="center2"
            />
            <label id="header_text_left_small"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * -0.5 - 4f }} -1.75"
                   size="{{ width/2f-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2f }} {{ height }}"
                   textcolor="{{ Theme.ScoreboardModule_Header_Text_Color }}"
                   class="text-primary"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize*0.8 }}"
                   halign="right"
                   valign="center2"
                   opacity="0.8"
            />
            <label id="header_text_right"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * 0.5 + 4f }} 1.75"
                   size="{{ width/2f-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2f }} {{ height }}"
                   textcolor="{{ Theme.ScoreboardModule_Header_Text_Color }}"
                   class="text-lg"
                   textfont="{{ Font.Regular }}"
                   valign="center2"
            />
            <label id="header_text_right_small"
                   pos="{{ float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture) * 0.5 + 4f }} -1.75"
                   size="{{ width/2f-float.Parse(Theme.ScoreboardModule_Logo_Width, CultureInfo.InvariantCulture)-2f }} {{ height }}"
                   textcolor="{{ Theme.ScoreboardModule_Header_Text_Color }}"
                   class="text-primary"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize*0.8 }}"
                   valign="center2"
                   opacity="0.8"
            />

            <frame pos="{{ width / 2f }} {{ height/ -2f + 4f }}">
                <label id="settings_button"
                       class="text-primary"
                       size="16 4"
                       pos="{{ -padding }}"
                       text="SETTINGS {{ Icons.Cog }}"
                       textsize="{{ Theme.UI_FontSize*0.5 }}"
                       opacity="0.1"
                       valign="center"
                       halign="right"
                       action="ScoreboardSettingsManialink/ShowSettings"
                       ScriptEvents="1"
                       focusareacolor1="00000000"
                       focusareacolor2="00000000"
                />
            </frame>

            <Logo if='Theme.ScoreboardModule_Logo_URL != ""'/>
        </frame>
    </template>

    <script resource="ScoreboardModule.Scripts.Header"/>
</component>
