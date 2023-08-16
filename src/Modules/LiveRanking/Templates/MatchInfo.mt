<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <property type="MatchInfo" name="data"/>
    <template>
        <frame id="mapinfo" pos="-158 70">
            <quad id="live_map_bg_grad_l" size="55 12" pos="0 0" modulatecolor="ff1070" opacity="0.9" z-index="1" image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga" />
            <quad id="live_map_bg_grad_r" size="55 12" pos="0 0" modulatecolor="1856d1" opacity="0.9" z-index="1" image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga" rot="180" valign="bottom" halign="right"/>
            <label id="mapname" textfont="GameFontBlack" textprefix="$t$fff" pos="3 -4" text="{{data.MapName}}"  halign="left" valign="center2" z-index="2"/>
            <label id="track_round_counter" textfont="GameFontSemiBold" textprefix="$t$fff" pos="3 -7" text="Track {{data.NumTrack}} · Round {{data.NumRound}}" textsize="1" halign="left" valign="center2" z-index="2"/>
            <label id="wr_info" textfont="GameFontSemiBold" textprefix="$t$fff" pos="3 -9.5" text="WR: {{data.WrTime}} by {{data.WrHolderName}}" textsize="0.8" halign="left" valign="center2" z-index="2"/>
        </frame>

       
        <frame id="window" pos="-65 80">
            <frame id="textFrame" size="60 10" valign="center2" halign="left" pos="0 0" z-index="0">
                <label id="text" size="46 10" valign="center2" halign="center" textsize="3" text="PLAYERNAME12345"
                       textfont="GameFontExtraBold" pos="25 -0.5" textcolor="fff" z-index="1" textprefix="$t$s"/>
            </frame>
            <quad id="bgQuad" size="60 10" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="111"
                  opacity="0.5" pos="0 0" valign="center" z-index="-1"/>
            <label size="10 10" valign="center" halign="center" textsize="3" text="150" textprefix="$i" textcolor="111" z-index="3"
                   pos="55 0" textfont="GameFontBlack"/>
            <quad size="10 10" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="fff" opacity="1"
                  pos="50 0" valign="center" z-index="2"/>
            <frame id="add_points_player_playerID2" hidden="0">
                <label size="7 7" valign="center" halign="center" textsize=".8" text="+4" textprefix="$i" textcolor="eee" z-index="3"
                       pos="54 -6.5" textfont="GameFontExtraBold"/>

                <quad size="8 5" image="file://Media/Manialinks/Nadeo/TMNext/Modes/Matchmaking/Matchmaking_score_plus_one.dds" colorize="1856d1" halign="center" opacity="1"
                      pos="54.1 -6" valign="center" z-index="1"/>
            </frame>
        </frame>
        <frame id="window" pos="0 80">
            <frame id="textFrame" size="60 10" valign="center2" halign="left" pos="0 0" z-index="0">
                <label id="text" size="46 10" valign="center2" halign="center" textsize="3" text="PLAYERNAME12345"
                       textfont="GameFontExtraBold" pos="25 -0.5" textcolor="fff" z-index="1" textprefix="$t$s"/>
            </frame>
            <quad id="bgQuad" size="60 10" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="111"
                  opacity="0.5" pos="0 0" valign="center" z-index="-1"/>
            <label size="10 10" valign="center" halign="center" textsize="3" text="150" textprefix="$i" textcolor="111" z-index="3"
                   pos="55 0" textfont="GameFontBlack"/>
            <quad size="10 10" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="fff" opacity="1"
                  pos="50 0" valign="center" z-index="2"/>
            <frame id="add_points_player_playerID3" hidden="0">
                <label size="7 7" valign="center" halign="center" textsize=".8" text="+6" textprefix="$i" textcolor="eee" z-index="3"
                       pos="54 -6.5" textfont="GameFontExtraBold"/>

                <quad size="8 5" image="file://Media/Manialinks/Nadeo/TMNext/Modes/Matchmaking/Matchmaking_score_plus_one.dds" colorize="1856d1" halign="center" opacity="1"
                      pos="54.1 -6" valign="center" z-index="1"/>
            </frame>
        </frame>
        <frame id="window" pos="65 80">
            <frame id="textFrame" size="60 10" valign="center2" halign="left" pos="0 0" z-index="0">
                <label id="text" size="46 10" valign="center2" halign="center" textsize="3" text="PLAYERNAME12345"
                       textfont="GameFontExtraBold" pos="25 -0.5" textcolor="fff" z-index="1" textprefix="$t$s"/>
            </frame>
            <quad id="bgQuad" size="60 10" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="111"
                  opacity="0.5" pos="0 0" valign="center" z-index="-1"/>
            <label size="10 10" valign="center" halign="center" textsize="3" text="150" textprefix="$i" textcolor="111" z-index="3"
                   pos="55 0" textfont="GameFontBlack"/>
            <quad size="10 10" style="UICommon64_1" substyle="BgFrame1" halign="left" colorize="fff" opacity="1"
                  pos="50 0" valign="center" z-index="2"/>
            <frame id="add_points_player_playerID4" hidden="0">
                <label size="7 7" valign="center" halign="center" textsize=".8" text="+3" textprefix="$i" textcolor="eee" z-index="3"
                       pos="54 -6.5" textfont="GameFontExtraBold"/>

                <quad size="8 5" image="file://Media/Manialinks/Nadeo/TMNext/Modes/Matchmaking/Matchmaking_score_plus_one.dds" colorize="1856d1" halign="center" opacity="1"
                      pos="54.1 -6" valign="center" z-index="1"/>
            </frame>
        </frame>
    </template>
</component>