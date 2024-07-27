<component>
    <using namespace="GbxRemoteNet.Structs"/>

    <import component="TeamInfoModule.Components.RoundCounter" as="RoundCounter"/>
    <import component="TeamInfoModule.Components.PointsBox" as="PointsBox"/>
    <import component="TeamInfoModule.Components.BottomInfoBox" as="BottomInfoBox"/>
    <import component="TeamInfoModule.Components.GainedPoints" as="GainedPoints"/>
    <import component="TeamInfoModule.Components.EmblemBox" as="EmblemBox"/>
    <import component="TeamInfoModule.Components.MatchPointBox" as="MatchPointBox"/>

    <property type="TmTeamInfo" name="team1"/>
    <property type="TmTeamInfo" name="team2"/>
    <property type="string?" name="infoBoxText"/>
    <property type="int" name="roundNumber" default="-1"/>
    <property type="int" name="matchPoint" default="0"/>
    <property type="int" name="team1Points" default="0"/>
    <property type="int" name="team2Points" default="0"/>
    <property type="int" name="team1GainedPoints" default="0"/>
    <property type="int" name="team2GainedPoints" default="0"/>

    <template>
        <UIStyle/>

        <frame pos="0 80.0">
            <BottomInfoBox if="infoBoxText != null"
                           y="-10.3"
                           text="{{ infoBoxText }}"
            />

            <RoundCounter roundNumber="{{ roundNumber }}"/>

            <!-- TEAM 1 -->
            <EmblemBox x="-22.0"
                       teamInfo="{{ team1 }}"
                       halign="right"
            />
            <PointsBox x="-10.0"
                       points="{{ team1Points }}"
                       color="{{ team1.RGB }}"
                       halign="right"
            />
            <MatchPointBox if="matchPoint == 1"
                           x="-34"
            />
            <GainedPoints if="team1GainedPoints > 0"
                          x="-34"
                          color="{{ team1.RGB }}"
                          gained="{{ team1GainedPoints }}"
            />

            <!-- TEAM 2 -->
            <EmblemBox x="22.0"
                       teamInfo="{{ team2 }}"
            />
            <PointsBox x="10.0"
                       points="{{ team2Points }}"
                       color="{{ team2.RGB }}"
            />
            <MatchPointBox if="matchPoint == 2"
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
    </template>
</component>
