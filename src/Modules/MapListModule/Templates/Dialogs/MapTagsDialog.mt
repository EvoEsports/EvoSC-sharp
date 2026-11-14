<!--
Dialog for viewing, adding and removing the tags of a map.
-->
<component>
  <using namespace="System" />
  <using namespace="System.Linq" />
  <using namespace="EvoSC.Modules.Official.MapListModule.Interfaces.Models" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.ScrollBar" as="ScrollBar" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <property type="string" name="mapName" />
  <property type="IEnumerable<IMapTag>" name="tags" />
  <property type="bool" name="readOnly" default="false" />
  
  <template>
    <UIStyle />
    
    <Window id="mapTagsWindow"
            title="Tags"
            icon="{{ Icons.Tags }}"
            x="-40"
            y="35"
            width="80"
            height="70"
    >
      <label class="text-primary" text="{{ mapName }}" pos="0 -2" valign="center" />
      
      <TextInput if="!readOnly" id="Tag" x="0" y="-6" width="52" placeholder="New tag ..." maxLength="32" />
      <Button if="!readOnly" id="btnAddTag" text="Add" x="54" y="-6" width="18" action="MapListManialinkController/AddTag" />
      
      <label if="!tags.Any()" class="text-primary" text="No tags yet." pos="1 -17" textsize="0.8" valign="center" />
      
      <!-- Tag list, scrolls when there are more than 5 tags -->
      <Container scrollable="true"
                 scrollHeight="{{ Math.Max(tags.Count()*6 - 30, 1) }}"
                 x="0"
                 y="-14"
                 width="68"
                 height="30"
                 id="mapTagsContainer"
      >
        <label foreach="IMapTag tag in tags" 
               class="text-primary" 
               text="{{ tag.Name }}" 
               pos="1 {{ -3 - __index*6 }}" 
               valign="center" />
        <IconButton foreach="IMapTag tag in tags"
                    if="!readOnly"
                    id="btnRemoveTag{{ __index }}"
                    icon="{{ Icons.Close }}"
                    type="secondary"
                    x="62"
                    y="{{ -__index*6 - 0.5 }}"
                    action="MapListManialinkController/RemoveTag/{{ tag.Name }}" />
      </Container>
      
      <quad pos="71 -14" size="2 30" bgcolor="{{ Theme.UI_BgHighlight }}" />
      <ScrollBar id="mapTagsScrollbar"
                 forFrame="mapTagsContainer"
                 x="71"
                 y="-14"
                 max="{{ Math.Max(tags.Count()*6 - 30, 1) }}"
                 length="30"
                 color="{{ Theme.UI_Window_Header_Separator }}" />
      
      <Button id="btnCloseTags" text="Close" x="0" y="-52" width="72" type="secondary" action="MapListManialinkController/CloseTags" />
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
