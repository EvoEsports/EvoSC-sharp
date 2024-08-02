<component>
    <property type="double" name="h" />
    <property type="bool" name="hidden" default="false" />
    
    <template>
        <!-- Club Tag Background -->
        <quad id="club_bg"
              size="{{ h * 2 }} {{ h }}"
              valign="center"
              modulatecolor="{{ Theme.UI_ClubTag_Bg }}"
              image="file://Media/Manialinks/Nadeo/Trackmania/Menus/PageClub/ClubActivities/Clubs_ActivityIcon_Mask.dds"
              alphamask="file://Media/Manialinks/Nadeo/Trackmania/Menus/PageClub/ClubActivities/Clubs_ActivityIcon_Mask.dds"
              hidden="{{ hidden ? 1 : 0 }}"
        />

        <!-- Club Tag Text -->
        <label id="club"
               pos="{{ h }} 0.2"
               size="5 3"
               valign="center"
               halign="center"
               textsize="0.9"
               textfont="{{ Font.Regular }}"
               hidden="{{ hidden ? 1 : 0 }}"
               textcolor="{{ Theme.UI_TextPrimary }}"
        />
    </template>
</component>
