<component>
    <using namespace="EvoSC.Common.Interfaces.Util"/>
    <using namespace="EvoSC.Modules.Official.RoundRankingModule.Models"/>

    <import component="EvoSC.Controls.Panel" as="Panel"/>

    <property type="double" name="width" default="36"/>
    <property type="double" name="height" default="4"/>
    <property type="double" name="y" default="0"/>
    <property type="CheckpointData" name="checkpoint"/>
    <property type="int" name="pos"/>
    <property type="bool" name="self" default="false"/>

    <template>
        <frame pos="0 {{ y }}">
            <quad
                    bgcolor="{{ Theme.UI_RoundRankingModule_Widget_AccentColor(pos) }}"
                    size="0.7 {{ height }}"
                    pos="0 0"
            />
            <quad
                    bgcolor="{{ Theme.UI_AccentSecondary }}"
                    size="{{ height }} {{ height }}"
                    opacity="0.9"
                    pos="0.7 0"
            />
            <label
                    text="{{ pos }}"
                    class="text-primary"
                    pos="{{ 0.6 + height/2 }} {{ -height/2.0 + 0.2 }}"
                    size="{{ height }} {{ height }}"
                    valign="center"
                    halign="center"
                    textcolor="{{ Theme.Black }}"
            />

            <Panel
                    width="{{ width-4 }}"
                    height="4"
                    className='{{ self ? "lr-body-highlight" : "lr-body-primary" }}'
                    x="4.7"
                    bgColor=""
            >
                <label
                        text="{{ checkpoint.Player.NickName }}"
                        class="text-primary"
                        textsize="0.5"
                        pos="1 {{ -height/2.0 + 0.2 }}"
                        valign="center"
                        size="{{ width-20 }} {{ height }}"
                />
                <label
                        text="{{ checkpoint.FormattedTime() }}"
                        textprefix='{{ checkpoint is { TimeDifference: not null, IsDNF: false } ? "+" : "" }}'
                        class="text-primary"
                        textsize="0.5"
                        pos="{{ width-4-(height * 1.25)-1.0 }} {{ -height/2.0 + 0.2 }}"
                        valign="center"
                        halign="right"
                        size="9 {{ height }}"
                        opacity="{{ checkpoint is { TimeDifference: null, IsDNF: false } ? 1.0 : 0.75 }}"
                />
                <frame>
                    <!-- CHECKPOINT ID & GAINED POINTS BOX -->
                    <quad
                            size="{{ height * 1.25 }} {{ height }}"
                            pos="{{ width-4 }}"
                            class='lr-body-highlight'
                            bgcolor="{{ checkpoint.AccentColor != null ? checkpoint.AccentColor : Theme.UI_RoundRankingModule_Widget_RowBgHighlight }}"
                            halign="right"
                    />
                    <label
                            if="checkpoint.GainedPoints == 0"
                            class="text-primary"
                            text='{{ checkpoint.IndexText() }}'
                            textsize="0.5"
                            pos="{{ width-4-(height*1.25/2.0) }} {{ -height/2.0 + 0.2 }}"
                            valign="center"
                            halign="center"
                            opacity="0.8"
                    />
                    <label
                            if="checkpoint.GainedPoints > 0"
                            class="text-primary"
                            text='{{ checkpoint.GainedPoints }}'
                            textprefix="+"
                            textsize="0.5"
                            textfont="{{ Font.Bold }}"
                            pos="{{ width-4-(height*1.25/2.0) }} {{ -height/2.0 + 0.2 }}"
                            size="3.5 {{ height }}"
                            valign="center"
                            halign="center"
                            opacity="0.8"
                    />
                </frame>
            </Panel>
        </frame>
    </template>
</component>
