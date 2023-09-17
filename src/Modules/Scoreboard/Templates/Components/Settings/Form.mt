<component>
    <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
    
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="80"/>

    <template>
        <label text="SCOREBOARD SETTINGS" textsize="5" textfont="GameFontBlack" />
        <frame pos="0 -8">
            <label text="Player Settings" textsize="2" textfont="GameFontSemiBold" />
            <Checkbox id="show_flags" y="-6" text="Show country flags" textFont="GameFontRegular" isChecked="{{ true }}" />
            <Checkbox id="show_club_tags" y="-11" text="Show club tags" textFont="GameFontRegular" isChecked="{{ true }}" />
            <Checkbox id="show_spectators" y="-16" text="Show spectators" textFont="GameFontRegular" isChecked="{{ true }}" />
            <Checkbox id="show_disconnected" y="-21" text="Show disconnected" textFont="GameFontRegular" isChecked="{{ true }}" />
        </frame>
    </template>
    
    <script>
        <!--
        *** OnCheckboxToggle ***
        ***
            if(ControlId == "show_flags"){
                declare persistent Boolean SB_Setting_ShowFlags for LocalUser = True;
                SB_Setting_ShowFlags = IsChecked;
                UpdateScoreboardLayout();
            }else if(ControlId == "show_club_tags"){
                declare persistent Boolean SB_Setting_ShowClubTags for LocalUser = True;
                SB_Setting_ShowClubTags = IsChecked;
                UpdateScoreboardLayout();
            }else if(ControlId == "show_spectators"){
                declare persistent Boolean SB_Setting_ShowSpectators for LocalUser = True;
                SB_Setting_ShowSpectators = IsChecked;
            }else if(ControlId == "show_disconnected"){
                declare persistent Boolean SB_Setting_ShowDisconnected for LocalUser = True;
                SB_Setting_ShowDisconnected = IsChecked;
            }
        ***
        
        *** OnInitialization ***
        ***
            declare persistent Boolean SB_Setting_ShowFlags for LocalUser = True;
            declare persistent Boolean SB_Setting_ShowClubTags for LocalUser = True;
            declare persistent Boolean SB_Setting_ShowSpectators for LocalUser = True;
            declare persistent Boolean SB_Setting_ShowDisconnected for LocalUser = True;
            
            SetCheckboxState("show_flags", SB_Setting_ShowFlags);
            SetCheckboxState("show_club_tags", SB_Setting_ShowClubTags);
            SetCheckboxState("show_spectators", SB_Setting_ShowSpectators);
            SetCheckboxState("show_disconnected", SB_Setting_ShowDisconnected);
        ***
        -->
    </script>
</component>