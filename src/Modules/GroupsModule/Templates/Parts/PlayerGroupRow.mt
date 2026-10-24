<!--
A single group in the player groups window, with a button to add the player to it or remove them from it.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />

  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.Button" as="Button" />

  <property type="GroupToggle" name="group" />
  <property type="string" name="accountId" />
  <property type="int" name="page" default="0" />
  <property type="int" name="index" default="0" />

  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="50" />
  <property type="double" name="height" default="6" />

  <template>
    <Panel x="{{ x }}" y="{{ y }}"
           width="{{ width }}"
           height="{{ height }}"
           bgColor="{{ Theme.UI_BgHighlight }}"
           overflow="true"
    >
      <label class="text-primary"
             text='{{ "$" + group.Color + group.Icon + " " + group.Title + (group.Unrestricted ? " $z(unrestricted)" : "") }}'
             pos="1 {{ -height/2.0+0.2 }}"
             size="{{ width-28 }} 4"
             valign="center"
      />

      <Button id="btnToggleGroup{{ index }}"
              text='{{ group.Member ? "Member" : "Add" }}'
              type='{{ group.Member ? "primary" : "secondary" }}'
              width="22"
              x="{{ width-24 }}"
              y="{{ -(height-5)/2 }}"
              action="PlayerRoles/TogglePlayerGroup/{{ accountId }}/{{ group.Id }}/{{ page }}"
      />
    </Panel>
  </template>
</component>
