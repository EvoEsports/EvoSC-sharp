<component>
    <using namespace="EvoSC.Modules.Official.RoundRankingModule.Config"/>
    <using namespace="EvoSC.Modules.Official.RoundRankingModule.Models"/>
    <using namespace="EvoSC.Common.Util.Manialinks"/>
    <using namespace="EvoSC.Common.Interfaces.Models"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="RoundRankingModule.Components.RoundRankingStyles" as="RoundRankingStyles"/>
    <import component="RoundRankingModule.Components.RoundRankingRow" as="RoundRankingRow"/>
    <import component="RoundRankingModule.Components.WinnerTeam" as="WinnerTeam"/>

    <property type="IRoundRankingSettings" name="settings"/>
    <property type="IEnumerable<CheckpointData>" name="bestCheckpoints"/>
    <property type="bool" name="showWinnerTeam"/>
    <property type="string" name="winnerTeamColor"/>
    <property type="string" name="winnerTeamName"/>
    <property type="PlayerTeam" name="winnerTeam" default="PlayerTeam.Unknown"/>

    <property type="double" name="rowHeight" default="4.0"/>
    <property type="double" name="rowSpacing" default="0.3"/>

    <template>
        <RoundRankingStyles/>
        <Widget header="Round Ranking"
                height="10"
                bodyStyle="unstyled"
                position='{{ settings.Position == WidgetPosition.Right ? "right" : "left" }}'
                y="{{ settings.Y }}"
        >
            <template slot="body">
                <WinnerTeam if="showWinnerTeam"
                            isDraw="{{ winnerTeam == PlayerTeam.Unknown }}"
                            winnerTeamName="{{ winnerTeamName }}"
                            color="{{ winnerTeamColor }}"
                />
                <RoundRankingRow foreach="CheckpointData data in bestCheckpoints"
                                 y="{{ (__index + (showWinnerTeam ? 1 : 0)) * -(rowHeight+rowSpacing) }}"
                                 pos="{{ __index + 1 }}"
                                 checkpoint="{{ data }}"
                />
            </template>
        </Widget>
    </template>
</component>