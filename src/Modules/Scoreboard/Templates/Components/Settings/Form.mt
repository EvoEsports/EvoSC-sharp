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
                declare persistent Boolean SB_ShowFlags for LocalUser = True;
                SB_ShowFlags = IsChecked;
                UpdateScoreboardLayout();
            }else if(ControlId == "show_club_tags"){
                declare persistent Boolean SB_ShowClubTags for LocalUser = True;
                SB_ShowClubTags = IsChecked;
                UpdateScoreboardLayout();
            }else if(ControlId == "show_spectators"){
                declare persistent Boolean SB_ShowSpectators for LocalUser = True;
                SB_ShowSpectators = IsChecked;
            }else if(ControlId == "show_disconnected"){
                declare persistent Boolean SB_ShowDisconnected for LocalUser = True;
                SB_ShowDisconnected = IsChecked;
            }
        ***
        
        *** OnInitialization ***
        ***
            declare persistent Boolean SB_ShowFlags for LocalUser;
            declare persistent Boolean SB_ShowClubTags for LocalUser;
            declare persistent Boolean SB_ShowSpectators for LocalUser;
            declare persistent Boolean SB_ShowDisconnected for LocalUser;
            
            SetCheckboxState("show_flags", SB_ShowFlags);
            SetCheckboxState("show_club_tags", SB_ShowClubTags);
            SetCheckboxState("show_spectators", SB_ShowSpectators);
            SetCheckboxState("show_disconnected", SB_ShowDisconnected);
        ***
        -->
    </script>
</component>