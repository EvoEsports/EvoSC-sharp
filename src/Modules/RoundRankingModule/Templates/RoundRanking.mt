<component>
    <using namespace="EvoSC.Modules.Official.RoundRankingModule.Config"/>
    <using namespace="EvoSC.Modules.Official.RoundRankingModule.Models"/>
    <using namespace="EvoSC.Common.Util.Manialinks"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="RoundRankingModule.Components.RoundRankingStyles" as="RoundRankingStyles"/>
    <import component="RoundRankingModule.Components.RoundRankingRow" as="RoundRankingRow"/>

    <property type="IRoundRankingSettings" name="settings"/>
    <property type="IEnumerable<CheckpointData>" name="bestCheckpoints"/>

    <template>
        <RoundRankingStyles/>
        <Widget header="Round Ranking"
                height="10"
                bodyStyle="unstyled"
                position='{{ settings.Position == WidgetPosition.Right ? "right" : "left" }}'
                y="{{ settings.Y }}"
        >
            <template slot="body">
                <RoundRankingRow foreach="CheckpointData data in bestCheckpoints"
                                 y="{{ -__index*(4+0.3) }}"
                                 pos="{{ __index + 1 }}"
                                 checkpoint="{{ data }}"
                />
            </template>
        </Widget>
    </template>
</component>