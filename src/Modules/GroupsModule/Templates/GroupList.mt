<!--
Shows a window with all groups, where they can be created, edited and deleted.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />
  <using namespace="System.Linq" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="GroupsModule.Parts.GroupRow" as="GroupRow" />

  <property type="IEnumerable<GroupListItem>" name="groups" />

  <template>
    <UIStyle />

    <Window title="Groups"
            icon="{{ Icons.Users }}"
            width="120"
            height="84"
            x="-60"
            y="42"
    >
      <label class="text-muted" text="Group" pos="1 -3" valign="center" />
      <label class="text-muted" text="Players" pos="66 -3" halign="center" valign="center" />
      <label class="text-muted" text="Permissions" pos="84 -3" halign="center" valign="center" />

      <Container scrollable="true"
                 scrollHeight="{{ ListLayout.ScrollHeight(groups.Count(), ListLayout.GroupRowPitch) }}"
                 y="-6"
                 width="114"
                 height="{{ ListLayout.ListHeight }}"
                 id="grouplist-container"
      >
        <GroupRow
                foreach="GroupListItem group in groups"
                group="{{ group }}"
                index="{{ __index }}"
                y="{{ -__index*ListLayout.GroupRowPitch }}"
                width="110"
        />
      </Container>

      <frame pos="0 -62">
        <Button id="btnNewGroup" text="New group" width="30" action="Groups/NewGroup" />
        <Button id="btnPlayers" text="Players" type="secondary" width="30" x="33" action="PlayerRoles/ShowPlayerList" />
      </frame>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
