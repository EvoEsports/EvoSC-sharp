<!--
Shows all groups, where the ones a player is in can be switched on and off.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />
  <using namespace="System.Linq" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="GroupsModule.Parts.PlayerGroupRow" as="PlayerGroupRow" />

  <property type="string" name="accountId" />
  <property type="string" name="nickName" />
  <property type="IEnumerable<GroupToggle>" name="groups" />
  <property type="string" name="search" default="" />
  <property type="int" name="page" />

  <template>
    <UIStyle />

    <Window title="Groups - {{ nickName }}"
            icon="{{ Icons.User }}"
            width="100"
            height="84"
            x="-50"
            y="42"
    >
      <!-- remembers the search of the player list to go back to it -->
      <HiddenEntry name="Search" value="{{ search }}" />

      <Container scrollable="true"
                 scrollHeight="{{ ListLayout.ScrollHeight(groups.Count(), ListLayout.GroupRowPitch) }}"
                 width="94"
                 height="{{ ListLayout.ListHeight }}"
                 id="playergroups-container"
      >
        <PlayerGroupRow
                foreach="GroupToggle group in groups"
                group="{{ group }}"
                accountId="{{ accountId }}"
                page="{{ page }}"
                index="{{ __index }}"
                y="{{ -__index*ListLayout.GroupRowPitch }}"
                width="90"
        />
      </Container>

      <frame pos="0 -56">
        <Button id="btnBack" text="Back" action="PlayerRoles/ShowPlayerListPage/{{ page }}" />
      </frame>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
