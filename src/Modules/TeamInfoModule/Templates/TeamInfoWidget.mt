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

    <template>
        <UIStyle/>

        <frame pos="0 82.0">
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
                       points="7"
                       color="{{ team1.RGB }}"
                       halign="right"
            />
            <MatchPointBox x="-34"/>
<!--            <GainedPoints x="-34"-->
<!--                          color="{{ team1.RGB }}"-->
<!--                          gained="0"-->
<!--            />-->

            <!-- TEAM 2 -->
            <EmblemBox x="22.0"
                       teamInfo="{{ team2 }}"
            />
            <PointsBox x="10.0"
                       points="5"
                       color="{{ team2.RGB }}"
            />
<!--            <MatchPointBox x="34"-->
<!--                           halign="right"-->
<!--            />-->
            <GainedPoints x="34"
                          color="{{ team2.RGB }}"
                          gained="1"
                          halign="right"
            />
        </frame>
    </template>
</component>
