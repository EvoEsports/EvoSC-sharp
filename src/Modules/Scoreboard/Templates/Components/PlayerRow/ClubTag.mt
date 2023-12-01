<component>
    <property type="double" name="h" />
    <property type="int" name="hidden" default="0" />
    
    <template>
        <!-- Club Tag Background -->
        <quad id="club_bg"
              size="{{ h * 2 }} {{ h }}"
              valign="center"
              modulatecolor="{{ Theme.ScoreboardModule_ClubTag_Bg }}"
              image="file://Media/Manialinks/Nadeo/Trackmania/Menus/PageClub/ClubActivities/Clubs_ActivityIcon_Mask.dds"
              alphamask="file://Media/Manialinks/Nadeo/Trackmania/Menus/PageClub/ClubActivities/Clubs_ActivityIcon_Mask.dds"
              hidden="{{ hidden }}"
        />

        <!-- Club Tag Text -->
        <label id="club"
               pos="{{ h }} 0.2"
               size="5 3"
               valign="center"
               halign="center"
               textsize="0.9"
               textfont="{{ Font.Regular }}"
               hidden="{{ hidden }}"
        />
    </template>
</component>