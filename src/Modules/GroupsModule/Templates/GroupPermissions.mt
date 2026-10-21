<!--
Shows all permissions of the server, where the ones a group has can be switched on and off.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />
  <using namespace="System.Linq" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="GroupsModule.Parts.PermissionRow" as="PermissionRow" />

  <property type="int" name="groupId" />
  <property type="string" name="groupTitle" />
  <property type="bool" name="unrestricted" />
  <property type="IEnumerable<PermissionToggle>" name="permissions" />

  <template>
    <UIStyle />

    <Window title="Permissions - {{ groupTitle }}"
            icon="{{ Icons.Key }}"
            width="110"
            height="84"
            x="-55"
            y="42"
    >
      <label if="unrestricted"
             class="text-muted"
             text="This group is unrestricted, so it has every permission regardless of the list below."
             pos="1 -3"
             valign="center"
      />

      <Container scrollable="true"
                 scrollHeight="{{ permissions.Count()*8 }}"
                 y="-6"
                 width="104"
                 height="54"
                 id="permissions-container"
      >
        <PermissionRow
                foreach="PermissionToggle permission in permissions"
                permission="{{ permission }}"
                groupId="{{ groupId }}"
                index="{{ __index }}"
                y="{{ -__index*8 }}"
                width="100"
        />
      </Container>

      <frame pos="0 -62">
        <Button id="btnDone" text="Done" action="Groups/ShowGroupList" />
      </frame>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
