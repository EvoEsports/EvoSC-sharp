<component>
    <using namespace="GbxRemoteNet.Structs"/>
    <using namespace="EvoSC.Modules.Official.TeamInfoModule.Config"/>

    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="TeamInfoModule.Components.RoundCounter" as="RoundCounter"/>
    <import component="TeamInfoModule.Components.PointsBox" as="PointsBox"/>
    <import component="TeamInfoModule.Components.BottomInfoBox" as="BottomInfoBox"/>
    <import component="TeamInfoModule.Components.GainedPoints" as="GainedPoints"/>
    <import component="TeamInfoModule.Components.EmblemBox" as="EmblemBox"/>
    <import component="TeamInfoModule.Components.MatchPointBox" as="MatchPointBox"/>
    <import component="TeamInfoModule.Components.TeamFullName" as="TeamFullName"/>

    <property type="ITeamInfoSettings" name="settings"/>
    <property type="string" name="neutralEmblemUrl" default=""/>
    <property type="TmTeamInfo" name="team1"/>
    <property type="TmTeamInfo" name="team2"/>
    <property type="string?" name="infoBoxText"/>
    <property type="bool" name="executeManiaScript"/>
    <property type="int" name="roundNumber" default="-1"/>
    <property type="int" name="team1Points" default="0"/>
    <property type="int" name="team2Points" default="0"/>
    <property type="bool" name="team1MatchPoint" default="false"/>
    <property type="bool" name="team2MatchPoint" default="false"/>
    <property type="int" name="team1GainedPoints" default="0"/>
    <property type="int" name="team2GainedPoints" default="0"/>

    <template>
        <UIStyle/>

        <frame pos="{{ settings.X }} {{ settings.Y }}" scale="{{ settings.Scale }}">
            <BottomInfoBox if="infoBoxText != null"
                           y="-10"
                           text="{{ infoBoxText }}"
            />

            <RoundCounter roundNumber="{{ roundNumber }}"/>

            <!-- TEAM 1 -->
            <PointsBox id="pointsBox1"
                       x="-10.0"
                       points="{{ team1Points }}"
                       color="{{ team1.RGB }}"
                       halign="right"
            />
            <TeamFullName if="!settings.CompactMode"
                          x="-22.0"
                          teamInfo="{{ team1 }}"
                          halign="right"
            />
            <frame pos="{{ settings.CompactMode ? 0.0 : -45.0 }} 0">
                <EmblemBox if="!string.IsNullOrEmpty(team1.EmblemUrl + neutralEmblemUrl)"
                           x="-22.0"
                           teamInfo="{{ team1 }}"
                           neutralEmblemUrl="{{ neutralEmblemUrl }}"
                           halign="right"
                />
                <MatchPointBox if="team1MatchPoint"
                               x="-34"
                />
                <GainedPoints if="team1GainedPoints > 0"
                              x="-34"
                              color="{{ team1.RGB }}"
                              gained="{{ team1GainedPoints }}"
                />
            </frame>

            <!-- TEAM 2 -->
            <PointsBox id="pointsBox2"
                       x="10.0"
                       points="{{ team2Points }}"
                       color="{{ team2.RGB }}"
            />
            <TeamFullName if="!settings.CompactMode"
                          x="22.0"
                          teamInfo="{{ team2 }}"
            />
            <frame pos="{{ settings.CompactMode ? 0.0 : 45.0 }} 0">
                <EmblemBox if="!string.IsNullOrEmpty(team2.EmblemUrl + neutralEmblemUrl)"
                           x="22.0"
                           teamInfo="{{ team2 }}"
                           neutralEmblemUrl="{{ neutralEmblemUrl }}"
                />
                <MatchPointBox
                               x="34"
                               halign="right"
                />
                <GainedPoints if="team2GainedPoints > 0"
                              x="34"
                              color="{{ team2.RGB }}"
                              gained="{{ team1GainedPoints }}"
                              halign="right"
                />
            </frame>
        </frame>
    </template>

    <script>
        <!--
        main() {
            if({{ !executeManiaScript ? "True" : "False" }}){
                return;
            }
            
            declare pointsBox1Label <=> (Page.MainFrame.GetFirstChild("pointsBox1") as CMlLabel);
            declare pointsBox2Label <=> (Page.MainFrame.GetFirstChild("pointsBox2") as CMlLabel);
            
            while(True){
                pointsBox1Label.Value = ClanScores[1] ^ "";
                pointsBox2Label.Value = ClanScores[2] ^ "";
                sleep(50);
            }
        }
        -->
    </script>
</component>
