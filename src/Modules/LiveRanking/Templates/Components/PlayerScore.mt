<component>
    <property type="string" name="ID" default="player1"/>
    <property type="string" name="PlayerName" default="Playername12345"/>
    <property type="string" name="CurrentPoints" default="0" />
    <property type="string" name="PointsGainHidden" default="0" />
    <property type="string" name="PointsGain" default="0"/>
    <property type="string" name="X" default="-135" />
    <property type="string" name="Y" default="80"/>
    <template>
        <frame id="window_{{ID}}" pos="{{X}} {{Y}}">
            <frame id="textFrame_{{ID}}" size="45 10" pos="0 0" valign="center2" halign="left" z-index="0">
                <label id="text_{{ID}}" size="35 10" pos="19 -0.5" valign="center2" halign="center" textsize="3" 
                       text="{{PlayerName}}" textfont="GameFontExtraBold" textcolor="fff" z-index="1" textprefix="$t$s"
                />
            </frame>
            <quad id="bgQuad_{{ID}}" size="45 8" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="111"
                  opacity="0.5" pos="0 0" valign="center" z-index="-1"/>
            <label size="8 8" valign="center" halign="center" textsize="3" text="150" textprefix="$i" textcolor="111" z-index="3"
                   pos="41 0" textfont="GameFontBlack"/>
            <quad size="8 8" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="fff" opacity="1"
                  pos="37 0" valign="center" z-index="2"/>
            <frame id="addPoints_{{ID}}" hidden="{{PointsGainHidden}}">
                <label size="7 7" valign="center" halign="center" textsize=".7" text="+{{PointsGain}}" textprefix="$i" textcolor="eee" z-index="3"
                       pos="40 -5.1" textfont="GameFontExtraBold"/>
                <quad size="5.5 3" image="file://Media/Manialinks/Nadeo/TMNext/Modes/Matchmaking/Matchmaking_score_plus_one.dds" colorize="1856d1" halign="center" opacity="1"
                      pos="40.1 -5.5" valign="center" z-index="1"/>
            </frame>
        </frame>
    </template>
</component>