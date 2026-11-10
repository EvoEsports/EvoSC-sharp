<!--
Dialog for searching Trackmania Exchange and adding a map to the server,
with a preview of the selected map.
-->
<component>
  <using namespace="System" />
  <using namespace="System.Linq" />
  <using namespace="EvoSC.Modules.Official.MapListModule.Models" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.Tag" as="Tag" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <property type="TmxSearchState" name="tmx" />
  <property type="TmxMapInfo?" name="selected" />
  <property type="int" name="pageSize" />
  
  <template>
    <UIStyle />
    
    <Window id="addMapWindow"
            title="Add map from Trackmania Exchange"
            icon="{{ Icons.Exchange }}"
            x="-90"
            y="46"
            width="180"
            height="100"
    >
      <!-- Search -->
      <TextInput id="TmxSearch" x="0" y="0" width="70" value="{{ tmx.Query }}" placeholder="Search maps by name ..." />
      <Button id="btnTmxSearch" text="Search" x="72" y="0" width="20" action="MapListManialinkController/TmxSearch" />
      
      <label if="tmx.Result.Maps.Count == 0" class="text-primary" text="No maps found." pos="2 -14" textsize="0.8" />
      
      <!-- Results -->
      <Button foreach="TmxMapInfo m in tmx.Result.Maps"
              id="tmxRow{{ __index }}"
              text=""
              size="custom"
              height="8"
              x="0"
              y="{{ -8 - __index*9 }}"
              width="92"
              type='{{ m.Id == tmx.SelectedId ? "accent" : "secondary" }}'
              action="MapListManialinkController/TmxSelect/{{ m.Id }}" />
      <label foreach="TmxMapInfo m in tmx.Result.Maps"
             class="text-primary"
             text="{{ m.Name }}"
             pos="2 {{ -8 - __index*9 - 2.5 }}"
             size="62 4"
             textsize="0.8"
             valign="center" />
      <label foreach="TmxMapInfo m in tmx.Result.Maps"
             class="text-primary"
             text="by {{ m.Authors }}"
             pos="2 {{ -8 - __index*9 - 5.8 }}"
             size="62 4"
             textsize="0.4"
             valign="center" />
      <label foreach="TmxMapInfo m in tmx.Result.Maps"
             class="text-primary"
             text='{{ m.Length.ToString(@"m\:ss\.fff") }}'
             pos="90 {{ -8 - __index*9 - 2.5 }}"
             textsize="0.5"
             halign="right"
             valign="center" />
      <label foreach="TmxMapInfo m in tmx.Result.Maps"
             class="text-primary"
             text="{{ m.Difficulty }}"
             pos="90 {{ -8 - __index*9 - 5.8 }}"
             textsize="0.4"
             halign="right"
             valign="center" />
      
      <!-- Paging -->
      <Button id="btnTmxPrev" text="Previous" x="0" y="-72" width="24" type="secondary"
              disabled="{{ tmx.Page == 0 }}"
              action="MapListManialinkController/TmxPage/{{ tmx.Page - 1 }}" />
      <label class="text-primary" text="Page {{ tmx.Page + 1 }}" pos="46 -74.5" halign="center" valign="center" textsize="0.6" />
      <Button id="btnTmxNext" text="Next" x="68" y="-72" width="24" type="secondary"
              disabled="{{ !tmx.Result.HasMore }}"
              action="MapListManialinkController/TmxPage/{{ tmx.Page + 1 }}" />
      
      <!-- Preview -->
      <quad if="selected != null" 
            pos="116 -7" 
            size="38 38" 
            image="{{ selected.ThumbnailUrl }}" 
            keepratio="Fit" />
      <label if="selected != null" 
             class="text-primary" 
             text="{{ selected.Name }}" 
             pos="98 -46" 
             size="74 5" 
             textsize="1.2" />
      <label if="selected != null" 
             class="text-primary" 
             text="by {{ selected.Authors }}" 
             pos="98 -51" 
             size="74 4" 
             textsize="0.6" />
      <label if="selected != null" 
             class="text-primary" 
             text='{{ selected.Difficulty }}  |  {{ selected.Length.ToString(@"m\:ss\.fff") }}  |  {{ selected.Awards }} awards' 
             pos="98 -56" 
             textsize="0.5" />
      <label if="selected != null" 
             class="text-primary" 
             text='Uploaded {{ selected.UploadedAt?.ToString("yyyy-MM-dd") ?? "-" }}  |  TMX #{{ selected.Id }}' 
             pos="98 -60" 
             textsize="0.4" />
      <label if="selected != null" 
             class="text-primary" 
             text='Tags: {{ selected.Tags.Count > 0 ? string.Join(", ", selected.Tags.Take(6)) + (selected.Tags.Count > 6 ? " +" + (selected.Tags.Count - 6) : "") : "-" }}' 
             pos="98 -64" 
             size="74 4" 
             textsize="0.4" />
      
      <Button if="selected != null" 
              id="btnTmxAdd" 
              text="Add to server" 
              x="98" y="-70" 
              width="36"
              action="MapListManialinkController/TmxAdd/{{ selected.Id }}" />
      <Button id="btnTmxClose" text="Close" x="136" y="-70" width="36" type="secondary" action="MapListManialinkController/CloseAddMap" />
      
      <label if="tmx.Message != null" 
             class="text-primary" 
             text="{{ tmx.Message }}" 
             pos="98 -78" 
             size="74 4" 
             textsize="0.5" />
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
