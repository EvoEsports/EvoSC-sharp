<!--
Shows the players that have been on the server and the groups they are in. Players can be searched by name.
-->
<component>
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="GroupsModule.Parts.PlayerRow" as="PlayerRow" />

  <property type="IEnumerable<PlayerListItem>" name="players" />
  <property type="string" name="search" default="" />
  <property type="int" name="page" />
  <property type="int" name="pageCount" />
  <property type="int" name="total" />

  <template>
    <UIStyle />

    <Window title="Player groups"
            icon="{{ Icons.User }}"
            width="120"
            height="94"
            x="-60"
            y="47"
    >
      <TextInput id="Search" value="{{ search }}" width="60" />
      <Button id="btnSearch" text="Search" width="22" x="63" action="PlayerRoles/ShowPlayerList" />
      <Button id="btnGroups" text="Groups" type="secondary" width="22" x="88" action="Groups/ShowGroupList" />

      <label class="text-muted" text="Player" pos="1 -9" valign="center" />
      <label class="text-muted" text="Groups" pos="46 -9" valign="center" />

      <frame pos="0 -12">
        <PlayerRow
                foreach="PlayerListItem player in players"
                player="{{ player }}"
                page="{{ page }}"
                index="{{ __index }}"
                y="{{ -__index*6 }}"
                width="114"
        />
      </frame>

      <frame pos="0 -73">
        <Button id="btnPrev" text="Previous" width="22" disabled="{{ page <= 0 }}" action="PlayerRoles/ShowPlayerListPage/{{ page-1 }}" />
        <label class="text-muted"
               text="Page {{ page+1 }} of {{ pageCount }} ({{ total }} players)"
               pos="57 -2.5"
               halign="center"
               valign="center"
        />
        <Button id="btnNext" text="Next" width="22" x="92" disabled="{{ page+1 >= pageCount }}" action="PlayerRoles/ShowPlayerListPage/{{ page+1 }}" />
      </frame>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
