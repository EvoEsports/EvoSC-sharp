<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Config"/>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models"/>
    <using namespace="EvoSC.Common.Models.Callbacks"/>
    <using namespace="System.Linq"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="EvoSC.Controls.Panel" as="Panel"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="LiveRankingModule.Components.RecordRow" as="RecordRow"/>

    <property type="ILiveRankingSettings" name="settings"/>
    <property type="IEnumerable<LiveRankingPosition>" name="scores"/>
    <property type="bool" name="isPointsBased"/>

    <template>
        <UIStyle/>
        <stylesheet>
            <style
                    class="lr-body-primary"
                    bgcolor="{{ Theme.UI_LiveRankingModule_Widget_RowBg }}"
                    opacity="0.9"
            />

            <style
                    class="lr-body-highlight"
                    bgcolor="{{ Theme.UI_LiveRankingModule_Widget_RowBgHighlight }}"
                    opacity="0.9"
            />
        </stylesheet>
        <Widget header="Live Ranking"
                height="10"
                bodyStyle="unstyled"
                position="{{ settings.Position }}"
                y="{{ settings.Y }}"
        >
            <template slot="body">
                <RecordRow foreach="LiveRankingPosition score in scores"
                           y="{{ -__index*(4+0.3) }}"
                           pos="{{ __index + 1 }}"
                           name="{{ score.Name }}"
                           time="{{ RaceTime.FromMilliseconds(score.Time) }}"
                           points="{{ score.Points }}"
                           usePoints="{{ isPointsBased }}"
                />

                <frame if="scores.Count() == 0">
                    <quad pos='{{ settings.Position == "right" ? settings.Width + 0.7 : 0 }}'
                          bgcolor="{{ Theme.UI_AccentPrimary }}"
                          size="0.7 9"
                          halign='{{ settings.Position }}'
                    />
                    <Panel
                            width="{{ settings.Width }}"
                            height="9"
                            x='{{ settings.Position == "right" ? 0 : 0.7}}'
                            className="lr-body-primary"
                            bgColor=""
                    >
                        <label
                                class="text-primary"
                                text="No finishes"
                                valign="center"
                                halign="center"
                                pos="18 -4.2"
                                size="36 9"
                        />
                    </Panel>
                </frame>
            </template>
        </Widget>
    </template>
</component>
