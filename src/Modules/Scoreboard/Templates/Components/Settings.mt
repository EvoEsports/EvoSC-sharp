<component>
    <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
    
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="80"/>

    <template>
        <label text="SCOREBOARD SETTINGS" textsize="5" textfont="GameFontBlack" />
        <label pos="0 -8" text="Player Settings" textsize="2" textfont="GameFontSemiBold" />

        <label pos="0 -13" text=" Show country flags" textFont="GameFontRegular" textsize="1.4" />
        <label pos="0 -18" text=" Show club tags" textFont="GameFontRegular" textsize="1.4" />
        <label pos="0 -23" text=" Show echolon" textFont="GameFontRegular" textsize="1.4" />
        <label pos="0 -28" text=" Force ubisoft name" textFont="GameFontRegular" textsize="1.4" />

<!--        <Checkbox id="show_flags" y="-8" text="Show country flags" textFont="GameFontRegular" isChecked="{{ true }}" />-->
<!--        <Checkbox id="show_club_tags" y="-10" text="Show club tags" textFont="GameFontRegular" isChecked="{{ true }}" />-->
    </template>
</component>