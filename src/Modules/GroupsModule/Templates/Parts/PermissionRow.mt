<!--
A single permission in the permissions window, with a button to switch it on or off for the group.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />

  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.Button" as="Button" />

  <property type="PermissionToggle" name="permission" />
  <property type="int" name="groupId" />
  <property type="int" name="index" default="0" />

  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="50" />
  <property type="double" name="height" default="7" />

  <template>
    <Panel x="{{ x }}" y="{{ y }}"
           width="{{ width }}"
           height="{{ height }}"
           bgColor="{{ Theme.UI_BgHighlight }}"
           overflow="true"
    >
      <label class="text-primary"
             text="{{ permission.Name }}"
             pos="1 {{ string.IsNullOrEmpty(permission.Description) ? -height/2.0+0.2 : -2.2 }}"
             size="{{ width-26 }} 4"
             valign="center"
      />
      <label if="!string.IsNullOrEmpty(permission.Description)"
             class="text-muted"
             text="{{ permission.Description }}"
             pos="1 -5"
             size="{{ width-26 }} 3"
             textsize="0.75"
             valign="center"
      />

      <Button id="btnToggle{{ index }}"
              text='{{ permission.Granted ? "Granted" : "Denied" }}'
              type='{{ permission.Granted ? "primary" : "secondary" }}'
              width="22"
              x="{{ width-24 }}"
              y="{{ -(height-5)/2 }}"
              action="Groups/ToggleGroupPermission/{{ groupId }}/{{ permission.Id }}"
      />
    </Panel>
  </template>
</component>
