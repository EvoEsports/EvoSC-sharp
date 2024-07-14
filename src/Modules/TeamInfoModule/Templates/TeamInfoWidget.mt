<component>
    <using namespace="GbxRemoteNet.Structs"/>

    <import component="TeamInfoModule.Components.RoundCounter" as="RoundCounter"/>
    <import component="TeamInfoModule.Components.PointsBox" as="PointsBox"/>
    <import component="TeamInfoModule.Components.BottomInfoBox" as="BottomInfoBox"/>
    <import component="TeamInfoModule.Components.EmblemBox" as="EmblemBox"/>

    <property type="TmTeamInfo" name="team1"/>
    <property type="TmTeamInfo" name="team2"/>
    <property type="int" name="roundNumber" default="15"/>

    <template>
        <UIStyle/>

        <frame pos="0 80.0">
            <BottomInfoBox y="-10.0"
                           text="test test test"
            />

            <RoundCounter roundNumber="{{ roundNumber }}"/>

            <!-- TEAM 1 -->
            <EmblemBox x="-21.0"
                       halign="right"
            />
            <PointsBox x="-10.0"
                       color="{{ team1.RGB }}"
                       halign="right"
                       points="7"
            />

            <!-- TEAM 2 -->
            <EmblemBox x="21.0"/>
            <PointsBox x="10.0"
                       color="f06"
                       points="5"
            />
        </frame>
    </template>
</component>
