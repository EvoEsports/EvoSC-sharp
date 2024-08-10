<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Config"/>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <using namespace="EvoSC.Common.Models.Callbacks"/>
    <using namespace="System.Linq"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="EvoSC.Controls.Panel" as="Panel"/>
    <import component="LiveRankingModule.Components.LiveRankingRecordRow" as="LiveRankingRecordRow"/>
    <import component="LiveRankingModule.Components.LiveRankingStyles" as="LiveRankingStyles"/>
    <import component="LiveRankingModule.Components.NoFinishesBox" as="NoFinishesBox"/>

    <property type="ILiveRankingSettings" name="settings"/>
    <property type="IEnumerable<LiveRankingPosition>" name="scores"/>
    <property type="bool" name="isPointsBased"/>

    <template>
        <LiveRankingStyles/>
        <Widget header="Live Ranking"
                height="10"
                bodyStyle="unstyled"
                position="{{ settings.Position }}"
                y="{{ settings.Y }}"
        >
            <template slot="body">
                <LiveRankingRecordRow foreach="LiveRankingPosition score in scores"
                           y="{{ -__index*(4+0.3) }}"
                           pos="{{ __index + 1 }}"
                           name="{{ score.Name }}"
                           time="{{ RaceTime.FromMilliseconds(score.Time) }}"
                           points="{{ score.Points }}"
                           usePoints="{{ isPointsBased }}"
                />
                <NoFinishesBox if="scores.Count() == 0" 
                               width="{{ settings.Width }}"
                               position="{{ settings.Position }}"
                />
            </template>
        </Widget>
    </template>
</component>
