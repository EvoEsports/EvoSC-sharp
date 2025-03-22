<component>
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    <import component="EvoSC.Containers.Window" as="Window" />
    <import component="EvoSC.Controls.Switch" as="Switch" />
    
    <property type="dynamic" name="Locale" />
    
    <template>
        <UIStyle />
        
        <Window
                width="60"
                height="26"
                x="-30"
                y="14"
                title="{{ Locale.PlayerLanguage.ScoreboardSettingsAbbreviation }}"
                icon="{{ Icons.Wrench }}"
        >
            <Switch id="switchHideSpectators" value="false" />
            <label text="{{ Locale.PlayerLanguage.Setting_HideSpectators }}" class="text-primary" pos="12 -1" />
            
            <Switch id="switchHideDisconnected" value="false" y="-7" />
            <label text="{{ Locale.PlayerLanguage.Setting_HideDisconnectedPlayers }}" class="text-primary" pos="12 -8" />
        </Window>
    </template>

    <script resource="ScoreboardModule.Scripts.Settings" />
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
