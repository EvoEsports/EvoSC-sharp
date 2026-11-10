<!--
Shows a window with a list of maps available in the current match settings, with
a sidebar for searching, filtering and sorting.
-->
<component>
  <using namespace="System" />
  <using namespace="System.Linq" />
  <using namespace="EvoSC.Modules.Official.MapListModule.Interfaces.Models" />
  <using namespace="EvoSC.Modules.Official.MapListModule.Models" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.ScrollBar" as="ScrollBar" />
  <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
  <import component="EvoSC.Controls.Tag" as="Tag" />
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="MapListModule.Parts.MapRow" as="MapRow" />
  
  <property type="MapListResult" name="result" />
  <property type="MapListFilter" name="filter" />
  <property type="bool" name="canRemoveMaps" />
  <property type="bool" name="canToggleMaps" />
  <property type="bool" name="canManageTags" />
  <property type="bool" name="canAddMaps" />
  
  <template>
    <UIStyle />
    
    <Window title="Maps" 
            icon="{{ Icons.Map }}"
            x="-100"
            y="45"
            width="200"
            height="90"
    >
      <!-- Sidebar -->
      <frame pos="0 0" id="maplist-sidebar">
        <TextInput id="Search" 
                   x="0" y="0" 
                   width="34" 
                   value="{{ filter.Search }}" 
                   placeholder="Search ..." />
        <Button id="btnSearch" text="Search" x="0" y="-6" width="34" action="MapListManialinkController/Search" />
        
        <Button id="btnFilterAll" text="All maps" x="0" y="-14" width="34" 
                type='{{ (!filter.FavoritesOnly && !filter.NoFinishOnly && !filter.DisabledOnly && filter.Tags.Count == 0 && filter.AuthorAccountId == null && filter.Search == "") ? "primary" : "secondary" }}'
                action="MapListManialinkController/ToggleFilter/all" />
        <frame pos="0 -20">
          <Rectangle id="chkFavorites" width="4.5" height="4.5" cornerRadius="0.5" bgColor="{{ Theme.UI_Checkbox_Default_Bg }}" action="MapListManialinkController/ToggleFilter/favorites" />
          <label if="filter.FavoritesOnly" text="{{ Icons.Check }}" textcolor="{{ Theme.UI_Checkbox_Default_CheckColor }}" textsize="1.5" size="4.5 4.5" halign="center" valign="center" pos="2.25 -2.25" action="MapListManialinkController/ToggleFilter/favorites" />
          <label class="text-primary" text="Favorites" textsize="0.8" size="26 4.5" valign="center" pos="6 -2.25" action="MapListManialinkController/ToggleFilter/favorites" />
        </frame>
        <frame pos="0 -26">
          <Rectangle id="chkNoFinish" width="4.5" height="4.5" cornerRadius="0.5" bgColor="{{ Theme.UI_Checkbox_Default_Bg }}" action="MapListManialinkController/ToggleFilter/nofinish" />
          <label if="filter.NoFinishOnly" text="{{ Icons.Check }}" textcolor="{{ Theme.UI_Checkbox_Default_CheckColor }}" textsize="1.5" size="4.5 4.5" halign="center" valign="center" pos="2.25 -2.25" action="MapListManialinkController/ToggleFilter/nofinish" />
          <label class="text-primary" text="No Finish" textsize="0.8" size="26 4.5" valign="center" pos="6 -2.25" action="MapListManialinkController/ToggleFilter/nofinish" />
        </frame>
        <frame if="canToggleMaps" pos="0 -32">
          <Rectangle id="chkDisabled" width="4.5" height="4.5" cornerRadius="0.5" bgColor="{{ Theme.UI_Checkbox_Default_Bg }}" action="MapListManialinkController/ToggleFilter/disabled" />
          <label if="filter.DisabledOnly" text="{{ Icons.Check }}" textcolor="{{ Theme.UI_Checkbox_Default_CheckColor }}" textsize="1.5" size="4.5 4.5" halign="center" valign="center" pos="2.25 -2.25" action="MapListManialinkController/ToggleFilter/disabled" />
          <label class="text-primary" text="Disabled" textsize="0.8" size="26 4.5" valign="center" pos="6 -2.25" action="MapListManialinkController/ToggleFilter/disabled" />
        </frame>
        
        <frame if="filter.AuthorAccountId != null" pos="0 {{ canToggleMaps ? -38 : -32 }}">
          <Rectangle id="chkAuthor" width="4.5" height="4.5" cornerRadius="0.5" bgColor="{{ Theme.UI_Checkbox_Default_Bg }}" action="MapListManialinkController/ToggleAuthor/{{ filter.AuthorAccountId }}" />
          <label text="{{ Icons.Check }}" textcolor="{{ Theme.UI_Checkbox_Default_CheckColor }}" textsize="1.5" size="4.5 4.5" halign="center" valign="center" pos="2.25 -2.25" action="MapListManialinkController/ToggleAuthor/{{ filter.AuthorAccountId }}" />
          <label class="text-primary" text='{{ filter.AuthorName ?? "Author" }}' textsize="0.8" size="26 4.5" valign="center" pos="6 -2.25" action="MapListManialinkController/ToggleAuthor/{{ filter.AuthorAccountId }}" />
        </frame>
        
        <frame if="!string.IsNullOrEmpty(filter.Search)" pos="0 {{ (canToggleMaps ? -38 : -32) - (filter.AuthorAccountId != null ? 6 : 0) }}">
          <Rectangle id="chkSearch" width="4.5" height="4.5" cornerRadius="0.5" bgColor="{{ Theme.UI_Checkbox_Default_Bg }}" action="MapListManialinkController/ToggleFilter/search" />
          <label text="{{ Icons.Check }}" textcolor="{{ Theme.UI_Checkbox_Default_CheckColor }}" textsize="1.5" size="4.5 4.5" halign="center" valign="center" pos="2.25 -2.25" action="MapListManialinkController/ToggleFilter/search" />
          <label class="text-primary" text='{{ filter.Search }}' textsize="0.8" size="26 4.5" valign="center" pos="6 -2.25" action="MapListManialinkController/ToggleFilter/search" />
        </frame>
        
        <label class="text-primary" text="Tags:" pos="0 {{ (canToggleMaps ? -40 : -34) - ((filter.AuthorAccountId != null ? 6 : 0) + (!string.IsNullOrEmpty(filter.Search) ? 6 : 0)) }}" valign="center" />
        <frame z-index="10">
          <Dropdown id="tagSelect" text="Add tag" x="0" y="{{ (canToggleMaps ? -44 : -38) - ((filter.AuthorAccountId != null ? 6 : 0) + (!string.IsNullOrEmpty(filter.Search) ? 6 : 0)) }}" width="34" type="secondary">
            <Button if="result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList().Count > 0"
                    id="btnAddTag0"
                    text="{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[0].Name }}"
                    x="0"
                    y="0"
                    width="34"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[0].Name }}" />
            <Button if="result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList().Count > 1"
                    id="btnAddTag1"
                    text="{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[1].Name }}"
                    x="0"
                    y="-5"
                    width="34"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[1].Name }}" />
            <Button if="result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList().Count > 2"
                    id="btnAddTag2"
                    text="{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[2].Name }}"
                    x="0"
                    y="-10"
                    width="34"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[2].Name }}" />
            <Button if="result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList().Count > 3"
                    id="btnAddTag3"
                    text="{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[3].Name }}"
                    x="0"
                    y="-15"
                    width="34"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[3].Name }}" />
            <Button if="result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList().Count > 4"
                    id="btnAddTag4"
                    text="{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[4].Name }}"
                    x="0"
                    y="-20"
                    width="34"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[4].Name }}" />
            <Button if="result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList().Count > 5"
                    id="btnAddTag5"
                    text="{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[5].Name }}"
                    x="0"
                    y="-25"
                    width="34"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ result.AllTags.Where(t => !filter.Tags.Contains(t.Name, StringComparer.OrdinalIgnoreCase)).ToList()[5].Name }}" />
          </Dropdown>
        </frame>
        <Tag foreach="string activeTag in filter.Tags.Take(3)"
             id="filterTag{{ __index }}"
             text="{{ activeTag }}"
             style="Round"
             severity="secondary"
             bgColor="{{ EvoSC.Common.Util.ColorUtils.Lighten(Theme.UI_BgHighlight, 8) }}"
             textColor="{{ Theme.UI_TextPrimary }}"
             height="3.5"
             textSize="0.4"
             width="28"
             x="0"
             y="{{ (canToggleMaps ? -50 : -44) - ((filter.AuthorAccountId != null ? 6 : 0) + (!string.IsNullOrEmpty(filter.Search) ? 6 : 0)) - __index*4.5 }}" />
        <IconButton foreach="string activeTag in filter.Tags.Take(3)"
                    icon="{{ Icons.Close }}"
                    id="btnRemoveTag{{ __index }}"
                    x="29"
                    y="{{ (canToggleMaps ? -50 : -44) - ((filter.AuthorAccountId != null ? 6 : 0) + (!string.IsNullOrEmpty(filter.Search) ? 6 : 0)) + 0.75 - __index*4.5 }}"
                    type="secondary"
                    action="MapListManialinkController/ToggleTag/{{ activeTag }}" />
        
        <label class="text-primary" 
               text="Showing {{ result.Maps.Count }} of {{ result.TotalCount }} total maps" 
               pos="0 -64" 
               textsize="0.8" />
      </frame>
      
      <!-- Sort bar -->
      <frame pos="38 0" id="maplist-sortbar">
        <label class="text-primary" text="Sort:" pos="0 -2.5" valign="center" />
        <Button id="btnSortName" 
                text='Name{{ filter.Sort == MapListSort.Name ? (filter.Descending ? " (desc)" : " (asc)") : "" }}' 
                x="10" width="22" 
                type='{{ filter.Sort == MapListSort.Name ? "primary" : "secondary" }}'
                action="MapListManialinkController/Sort/Name" />
        <Button id="btnSortAuthor" 
                text='Author{{ filter.Sort == MapListSort.Author ? (filter.Descending ? " (desc)" : " (asc)") : "" }}' 
                x="33" width="22" 
                type='{{ filter.Sort == MapListSort.Author ? "primary" : "secondary" }}'
                action="MapListManialinkController/Sort/Author" />
        <Button id="btnSortPb" 
                text='PB{{ filter.Sort == MapListSort.Pb ? (filter.Descending ? " (desc)" : " (asc)") : "" }}' 
                x="56" width="22" 
                type='{{ filter.Sort == MapListSort.Pb ? "primary" : "secondary" }}'
                action="MapListManialinkController/Sort/Pb" />
        <Button id="btnSortFavorite" 
                text='Favorite{{ filter.Sort == MapListSort.Favorite ? (filter.Descending ? " (desc)" : " (asc)") : "" }}' 
                x="79" width="22" 
                type='{{ filter.Sort == MapListSort.Favorite ? "primary" : "secondary" }}'
                action="MapListManialinkController/Sort/Favorite" />
        <Button if="canAddMaps"
                id="btnAddMap"
                text="Add map"
                x="130" width="22"
                action="MapListManialinkController/OpenAddMap" />
      </frame>
      
      <Container scrollable="true"
                 scrollHeight="{{ Math.Max(result.Maps.Count*8 - 62, 1) }}"
                 x="38"
                 y="-8"
                 width="152"
                 height="62"
                 id="maplistContainer"
      >
        <MapRow 
                foreach="IMapListMap map in result.Maps" 
                map="{{ map }}" 
                y="{{ -__index*8 }}" 
                width="150" 
                index="{{ __index }}" 
                canRemoveMaps="{{ canRemoveMaps }}"
                canToggleMaps="{{ canToggleMaps }}"
                canManageTags="{{ canManageTags }}"
        />
      </Container>
      
      <!-- Scroll track and slider -->
      <quad pos="191 -8" size="2 62" bgcolor="{{ Theme.UI_BgHighlight }}" />
      <ScrollBar id="maplistScrollbar" 
                 forFrame="maplistContainer" 
                 x="191" 
                 y="-8" 
                 max="{{ Math.Max(result.Maps.Count*8 - 62, 1) }}" 
                 length="62"
                 color="{{ Theme.UI_Window_Header_Separator }}" />
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
