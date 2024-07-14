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

        <frame pos="0 84.0">
            <BottomInfoBox y="-10.0"
                           text="test test test"
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

            <!-- TEAM 2 -->
            <EmblemBox x="22.0"
                       teamInfo="{{ team2 }}"
            />
            <PointsBox x="10.0"
                       points="5"
                       color="{{ team2.RGB }}"
            />
        </frame>
    </template>
</component>
