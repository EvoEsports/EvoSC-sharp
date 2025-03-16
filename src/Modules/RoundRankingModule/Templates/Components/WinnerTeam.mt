<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>

    <property type="double" name="width" default="36.0"/>
    <property type="double" name="height" default="4.0"/>
    <property type="bool" name="isDraw" default="false"/>
    <property type="string" name="color" default="000000"/>
    <property type="string" name="winnerTeamName"/>

    <template>
        <frame>
            <quad bgcolor="{{ color }}"
                  size="0.7 {{ height }}"
                  pos="0 0"
            />
            <quad pos="0.7 0"
                  size="36 {{ height }}"
                  class="lr-body-primary"
            />
            <label
                    text="{{ winnerTeamName }}"
                    textprefix='{{ isDraw ? "" : Icons.Trophy+" " }}'
                    class="text-primary"
                    textsize="0.5"
                    size="{{ width-2.0 }} {{ height }}"
                    pos="{{ 0.7+(width/2.0) }} {{ -height/2.0 }}"
                    valign="center2"
                    halign="center"
            />
        </frame>
    </template>
</component>