<!--
A single row in the group list.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />

  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />

  <property type="GroupListItem" name="group" />
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
             size="58 4"
             valign="center"
      />
      <label class="text-primary" text="{{ group.MemberCount }}" pos="66 {{ -height/2.0+0.2 }}" halign="center" valign="center" />
      <label class="text-primary" text='{{ group.Unrestricted ? "all" : group.PermissionCount.ToString() }}' pos="84 {{ -height/2.0+0.2 }}" halign="center" valign="center" />

      <IconButton
              if="!group.IsBuiltIn"
              icon="{{ Icons.Trash }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-6 }}"
              id="btnDelete{{ index }}"
              type="secondary"
              action="Groups/DeleteGroup/{{ group.Id }}"
      />
      <IconButton
              icon="{{ Icons.Key }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-12 }}"
              id="btnPermissions{{ index }}"
              action="Groups/ShowGroupPermissions/{{ group.Id }}"
      />
      <IconButton
              icon="{{ Icons.Pencil }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-18 }}"
              id="btnEdit{{ index }}"
              action="Groups/EditGroup/{{ group.Id }}"
      />
    </Panel>
  </template>
</component>
