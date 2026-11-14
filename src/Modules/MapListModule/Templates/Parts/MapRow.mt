<!--
Show a row with map information along with action buttons.
-->
<component>
  <using namespace="System" />
  <using namespace="System.Linq" />
  <using namespace="EvoSC.Modules.Official.MapListModule.Interfaces.Models" />
  
  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Controls.Tag" as="Tag" />
  
  <property type="IMapListMap" name="map" />
  <property type="bool" name="canRemoveMaps" />
  <property type="bool" name="canToggleMaps" />
  <property type="bool" name="canManageTags" />
  
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="50" />
  <property type="double" name="height" default="7" />
  <property type="int" name="index" default="0" />

  <template>
    <Panel x="{{ x }}" y="{{ y }}" 
           width="{{ width }}" 
           height="{{ height }}"
           bgColor="{{ Theme.UI_BgHighlight }}"
           overflow="true"
    >
      <label class="text-primary" text='{{ map.IsEnabled ? map.Map.Name : "$888" + map.Map.Name + " (disabled)" }}' pos="1 {{ -height/4.0 }}" size="{{ width*0.26 - 2 }} 4" valign="center" />
      
      <label class="text-primary" 
             text='{{ map.Map.Author?.NickName ?? "Unknown" }}' 
             pos="1 {{ -height*0.72 }}" 
             size="{{ width*0.26 - 2 }} 3" 
             textsize="0.6" 
             valign="center" />
      <IconButton
              if="map.Map.Author != null"
              icon="{{ Icons.Users }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width*0.26 }}"
              id="btnAuthor{{ index }}"
              type="secondary"
              action="MapListManialinkController/SetAuthor/{{ map.Map.Author.AccountId }}/true"
      />
      
      <label class="text-primary" 
             text='PB: {{ map.HasPb ? TimeSpan.FromMilliseconds(map.Pb.Score).ToString(@"m\:ss\.fff") : "-" }}' 
             pos="{{ width*0.37 }} {{ -height/4.0 }}" 
             textsize="0.3" 
             valign="center" />
      
      <label class="text-primary" 
             text='AT: {{ map.Map.Details != null ? TimeSpan.FromMilliseconds(map.Map.Details.AuthorTime.TotalMilliseconds).ToString(@"m\:ss\.fff") : "-" }}' 
             pos="{{ width*0.37 }} {{ -height*0.72 }}" 
             textsize="0.3" 
             valign="center" />
      
      <label class="text-primary" 
             text="Tags:" 
             pos="{{ width*0.5 }} {{ -height/2.0+0.2 }}" 
             textsize="0.3" 
             valign="center" />
      <Tag foreach="IMapTag tag in map.Tags.Where((t, i) => map.Tags.Take(i+1).Sum(x => x.Name.Length*1.0 + 5) <= (width-15-6*((canRemoveMaps ? 1 : 0) + (canToggleMaps ? 1 : 0) + (canManageTags ? 1 : 0)) - (width*0.5+8) - 6))"
           id="rowTag{{ index }}_{{ __index }}"
           text="{{ tag.Name }}"
           style="Round"
           severity="secondary"
           bgColor="{{ EvoSC.Common.Util.ColorUtils.Lighten(Theme.UI_BgHighlight, 8) }}"
           textColor="{{ Theme.UI_TextPrimary }}"
           height="3.5"
           textSize="0.3"
           width="{{ tag.Name.Length*1.0 + 4 }}"
           x="{{ width*0.5 + 8 + map.Tags.Take(__index).Sum(t => t.Name.Length*1.0 + 5) }}"
           y="{{ -(height-3.5)/2 }}" />
      <label if="map.Tags.Count() > map.Tags.Where((t, i) => map.Tags.Take(i+1).Sum(x => x.Name.Length*1.0 + 5) <= (width-15-6*((canRemoveMaps ? 1 : 0) + (canToggleMaps ? 1 : 0) + (canManageTags ? 1 : 0)) - (width*0.5+8) - 6)).Count()"
             class="text-primary"
             text="+{{ map.Tags.Count() - map.Tags.Where((t, i) => map.Tags.Take(i+1).Sum(x => x.Name.Length*1.0 + 5) <= (width-15-6*((canRemoveMaps ? 1 : 0) + (canToggleMaps ? 1 : 0) + (canManageTags ? 1 : 0)) - (width*0.5+8) - 6)).Count() }}"
             pos="{{ width*0.5 + 8 + map.Tags.Where((t, i) => map.Tags.Take(i+1).Sum(x => x.Name.Length*1.0 + 5) <= (width-15-6*((canRemoveMaps ? 1 : 0) + (canToggleMaps ? 1 : 0) + (canManageTags ? 1 : 0)) - (width*0.5+8) - 6)).Sum(t => t.Name.Length*1.0 + 5) }} {{ -height/2.0+0.2 }}"
             size="4 3"
             textsize="0.3"
             valign="center"
             focusareacolor1="0000"
             focusareacolor2="FFF3"
             action="MapListManialinkController/ViewTags/{{ map.Map.Uid }}"
             scriptevents="true" />
      <label if="!map.Tags.Any()" 
             class="text-primary" 
             text="-" 
             pos="{{ width*0.5 + 8 }} {{ -height/2.0+0.2 }}" 
             textsize="0.3" 
             valign="center" />

      <IconButton
              icon="{{ Icons.Close }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-6 }}"
              id="btnRemove{{ index }}"
              type="secondary"
              action="MapListManialinkController/DeleteMap/{{ map.Map.Uid }}"
              if="canRemoveMaps"
      />
      
      <IconButton
              icon="{{ map.IsEnabled ? Icons.EyeSlash : Icons.Eye }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-6 - (canRemoveMaps ? 6 : 0) }}"
              id="btnToggleEnabled{{ index }}"
              type="secondary"
              action="MapListManialinkController/SetMapEnabled/{{ map.Map.Uid }}/{{ !map.IsEnabled }}"
              if="canToggleMaps"
      />
      
      <IconButton
              icon="{{ Icons.Tags }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-6 - (canRemoveMaps ? 6 : 0) - (canToggleMaps ? 6 : 0) }}"
              id="btnTags{{ index }}"
              type="secondary"
              action="MapListManialinkController/OpenTags/{{ map.Map.Uid }}"
              if="canManageTags"
      />
      
      <IconButton 
              if="map.IsEnabled"
              icon="{{ Icons.PlayCircle }}" 
              y="{{ -(height-5)/2 }}" 
              x="{{ width-6 - 6*((canRemoveMaps ? 1 : 0) + (canToggleMaps ? 1 : 0) + (canManageTags ? 1 : 0)) }}" 
              id="btnQueue{{ index }}"
              action="MapListManialinkController/QueueMap/{{ map.Map.Uid }}" />
      
      <IconButton icon="{{ map.IsFavorite ? Icons.Heart : Icons.HeartO }}"
                  y="{{ -(height-5)/2 }}" 
                  x="{{ width-12 - 6*((canRemoveMaps ? 1 : 0) + (canToggleMaps ? 1 : 0) + (canManageTags ? 1 : 0)) }}" 
                  id="btnFavorite{{ index }}"
                  action="MapListManialinkController/SetFavorite/{{ map.Map.Uid }}/{{ !map.IsFavorite }}" />
    </Panel>
  </template>
</component>
