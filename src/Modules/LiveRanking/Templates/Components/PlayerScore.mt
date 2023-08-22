<component>
    <property type="string" name="ID" default="player1"/>
    <property type="string" name="PlayerName" default="Playername12345"/>
    <property type="string" name="CurrentPoints" default="0" />
    <property type="string" name="PointsGainHidden" default="0" />
    <property type="string" name="PointsGain" default="0"/>
    <property type="string" name="X" default="-135" />
    <property type="string" name="Y" default="80"/>
    <template>
        <framemodel id="player">
            <quad pos="0 0" z-index="1" size="7 5" style="UICommon64_1" substyle="BgFrame2" opacity="1" colorize="dc0"/>
            <label pos="3.5 -2.25" z-index="2" size="7 5" text="1" halign="center" valign="center" textfont="GameFontExtraBold" textsize="2" textcolor="000000FF" textprefix="$i"/>

            <label pos="10 -2.5" z-index="3" size="50 5" text="{{PlayerName}}" valign="center2" textfont="GameFontSemiBold" textsize="1" textprefix="$i$t"/>
            <label pos="65 -2.5" z-index="3" size="15 5" text="01:30.238" valign="center2" textfont="GameFontSemiBold" textsize="1" textprefix="$i$t" halign="center"/>

            <quad z-index="2" pos="9 0" size="50 5" image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga" opacity="1" modulatecolor="484a5a" />
        </framemodel>
    </template>
</component>