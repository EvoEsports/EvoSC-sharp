<!--
A single player in the player list.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />

  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />

  <property type="PlayerListItem" name="player" />
  <property type="int" name="page" default="0" />
  <property type="int" name="index" default="0" />

  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="50" />
  <property type="double" name="height" default="5.5" />

  <template>
    <Panel x="{{ x }}" y="{{ y }}"
           width="{{ width }}"
           height="{{ height }}"
           bgColor="{{ Theme.UI_BgHighlight }}"
           overflow="true"
    >
      <label class="text-primary" text="{{ player.NickName }}" pos="1 {{ -height/2.0+0.2 }}" size="43 4" valign="center" />
      <label class="text-primary" text="{{ player.GroupTitles }}" pos="46 {{ -height/2.0+0.2 }}" size="{{ width-58 }} 4" valign="center" />

      <IconButton
              icon="{{ Icons.Pencil }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-6 }}"
              id="btnEditPlayer{{ index }}"
              action="PlayerRoles/EditPlayerGroups/{{ player.AccountId }}/{{ page }}"
      />
    </Panel>
  </template>
</component>
