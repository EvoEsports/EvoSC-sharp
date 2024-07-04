<component>
    <using namespace="EvoSC.Manialinks.Validation"/>
    <using namespace="EvoSC.Common.Interfaces.Localization"/>
    <using namespace="EvoSC.Modules.Official.TeamSettingsModule.Models"/>

    <import component="EvoSC.FormEntry" as="FormEntry"/>
    <import component="EvoSC.Containers.Window" as="Window"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Controls.Button" as="Button"/>
    <import component="TeamSettings.ColorInput" as="ColorInput"/>

    <property type="FormValidationResult" name="Validation"/>
    <property type="TeamSettingsModel" name="Settings"/>
    <property type="dynamic" name="Locale"/>

    <template>
        <UIStyle/>

        <Window
                width="132"
                height="84"
                x="-67"
                y="44"
                title="{{ Locale.PlayerLanguage.UI_EditTeamInfo }}"
                icon=""
        >
            <!-- TEAM 1 -->
            <frame>
                <Label
                        text="{{ Locale.PlayerLanguage.UI_Team_One }}"
                        class="text-primary"
                        textsize="2"
                        halign="left"
                        valign="top"
                />

                <FormEntry
                        validationResults='{{ Validation?.GetResult("Team1EmblemUrl") }}'
                        name="Team1EmblemUrl"
                        value="{{ Settings.Team1EmblemUrl }}"
                        label="{{ Locale.PlayerLanguage.UI_Team_Emblem_URL }}"
                        w="60"
                        y="-50"
                />

                <ColorInput y="-35"
                            width="60"
                            name="Team1SecondaryColor"
                            label="{{ Locale.PlayerLanguage.UI_Team_Color_Secondary }}"
                            value="{{ Settings.Team1SecondaryColor }}"
                            validationResults='{{ Validation?.GetResult("Team1SecondaryColor") }}'
                />

                <ColorInput y="-20"
                            width="60"
                            name="Team1PrimaryColor"
                            label="{{ Locale.PlayerLanguage.UI_Team_Color_Primary }}"
                            value="{{ Settings.Team1PrimaryColor }}"
                            validationResults='{{ Validation?.GetResult("Team1PrimaryColor") }}'
                />

                <FormEntry
                        validationResults='{{ Validation?.GetResult("Team1Name") }}'
                        name="Team1Name"
                        value="{{ Settings.Team1Name }}"
                        label="{{ Locale.PlayerLanguage.UI_Team_Name }}"
                        w="60"
                        y="-5"
                />
            </frame>

            <!-- TEAM 2 -->
            <frame pos="65 0">
                <Label
                        text="{{ Locale.PlayerLanguage.UI_Team_Two }}"
                        class="text-primary"
                        textsize="2"
                        halign="left"
                        valign="top"
                />

                <FormEntry
                        validationResults='{{ Validation?.GetResult("Team2EmblemUrl") }}'
                        name="Team2EmblemUrl"
                        value="{{ Settings.Team2EmblemUrl }}"
                        label="{{ Locale.PlayerLanguage.UI_Team_Emblem_URL }}"
                        w="60"
                        y="-50"
                />

                <ColorInput y="-35"
                            width="60"
                            name="Team2SecondaryColor"
                            label="{{ Locale.PlayerLanguage.UI_Team_Color_Secondary }}"
                            value="{{ Settings.Team2SecondaryColor }}"
                            validationResults='{{ Validation?.GetResult("Team2SecondaryColor") }}'
                />

                <ColorInput y="-20"
                            width="60"
                            name="Team2PrimaryColor"
                            label="{{ Locale.PlayerLanguage.UI_Team_Color_Primary }}"
                            value="{{ Settings.Team2PrimaryColor }}"
                            validationResults='{{ Validation?.GetResult("Team2PrimaryColor") }}'
                />

                <FormEntry
                        validationResults='{{ Validation?.GetResult("Team2Name") }}'
                        name="Team2Name"
                        value="{{ Settings.Team2Name }}"
                        label="{{ Locale.PlayerLanguage.UI_Team_Name }}"
                        w="60"
                        y="-5"
                />
            </frame>

            <frame pos="0 -65">
                <Button id="btnSubmit"
                        text="{{ Locale.PlayerLanguage.UI_Submit }}"
                        action="TeamSettings/SaveTeamSettings"
                />
                <Button id="btnCancel"
                        type="secondary"
                        text="{{ Locale.PlayerLanguage.UI_Cancel }}"
                        action="TeamSettings/HideTeamSettings"
                        x="106"
                />
            </frame>
        </Window>
    </template>

    <script resource="EvoSC.Scripts.UIScripts"/>
</component>