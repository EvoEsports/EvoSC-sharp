<component>
  <using namespace="EvoSC.Modules.Official.MapListModule.Interfaces.Models" />
  
  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
  
  <property type="IMapListMap" name="map" />
  <property type="bool" name="canRemoveMaps" />
  
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
      <label class="text-primary" text="{{ map.Map.Name }}" valign="center" pos="1 {{ -height/2.0+0.2 }}" />
      
      <label class="text-primary" text="{{ map.Map.Author.NickName }} " pos="{{ width/2 }} {{ -height/2.0+0.2 }}" halign="center" valign="center" />

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
              icon="{{ Icons.PlayCircle }}" 
              y="{{ -(height-5)/2 }}" 
              x="{{ width-12 + (canRemoveMaps ? 0 : 6) }}" 
              id="btnQueue{{ index }}"
              action="MapListManialinkController/QueueMap/{{ map.Map.Uid }}" />
      
      <IconButton icon="{{ Icons.HeartO }}"
                  y="{{ -(height-5)/2 }}" 
                  x="{{ width-18 + (canRemoveMaps ? 0 : 6) }}" 
                  id="btnFavorite{{ index }}"
                  action="MapListManialinkController/FavoriteMap/{{ map.Map.Uid }}" />
    </Panel>
  </template>
</component>
