<component>
  <using namespace="EvoSC.Modules.Official.MapListModule.Interfaces.Models" />
  <using namespace="EvoSC.Common.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Controls.Text" as="Text" />
  <import component="EvoSC.Controls.Tag" as="Tag" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.LinkButton" as="LinkButton" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
  <import component="EvoSC.Controls.Rating" as="Rating" />
    
  <property type="int" name="key" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="185" />
  
  <property type="IMapListMap" name="map" />
  
  <template>
    <Panel width="{{ width }}" 
           height="10" 
           bgColor="{{ Theme.MapListModule_MapListRow_Default_Bg }}"
           x="{{ x }}"
           y="{{ y }}"
           zIndex="{{ 100-key }}"
    >
      <Checkbox id="evosc-maplistrow-chkselected-{{ key }}" x="2.5" y="-3.5" />

        <!-- Map name and author -->
      <frame pos="7.5 -1.5">
          <label text="{{ map.Map.Name }}" textsize="1.8" textfont="{{ Font.Regular }}" size="34 5" />
          <label text="{{ Icons.User }} {{ map.Map.Author.NickName }}" textsize="0.7" pos="0 -4.5" textfont="{{ Font.Thin }}"/>
      </frame>

        <!-- PB and AT -->
      <frame pos="48 -1.5">
          <label text="PB:" textsize="0.7" class="text" if="map.HasPb" />
          <label 
                  text="{{ RaceTime.FormatFromMilliseconds(map.Pb?.Score ?? 0) }}" 
                  textsize="0.7" 
                  pos="5 0" 
                  class="text"
                  if="map.HasPb"
          />
          <Tag text="888" x="16" style="Round" width="5" bgColor="888888" if="map.HasPb" />

          <label text="AT:" textsize="0.7"  pos="0 -4.5" class="text"/>
          <label text="{{ map.Map.Details.AuthorTime.ToString() }}" textsize="0.7"  pos="5 -4.5" class="text" />
      </frame>

        <!-- Tags and links -->
        <frame pos="75 -1.5">
            <label class="text" text="Tags: " x="1" textsize="0.7" />
            <Tag style="Square" text="Magnet" x="9" bgColor="{{ Theme.MapListModule_MapListRow_Default_TagBg }}" />
            <Tag style="Square" text="Speed Drift" x="25" bgColor="{{ Theme.MapListModule_MapListRow_Default_TagBg }}" />
            <Tag style="Square" text=".." x="41" width="3" bgColor="{{ Theme.MapListModule_MapListRow_Default_TagBg }}" />
            
            <label class="text" text="Open: " pos="0 -4.5" textsize="0.7" />
            <LinkButton text="TMX" url="https://trackmania.exchange" x="9" y="-4.5" id="linkTmx" bgColor="{{ Theme.UI_BgHighlight }}" width="8" className="maplistrow-linkbtn" />
            <LinkButton text="TM.IO" url="https://trackmania.exchange" x="18" y="-4.5" id="linkTmx" bgColor="{{ Theme.UI_BgHighlight }}" width="10" className="maplistrow-linkbtn" />
        </frame>
        
        <!-- Map rating -->
        <frame pos="122 -4.3">
          <Rating value="50" />
          <label text="$888($aaa321$888)" pos="20 -0.5" textsize="0.7" />
        </frame>
        
        <!-- Action buttons -->
      <frame pos="{{ width - 34 }} -2.5">
          <Button text="{{ Icons.HeartO }}" id="btnFavorite-{{ key }}" width="5" />
          <Button text="{{ Icons.PlayCircle }}" id="btnQueue-{{ key }}" width="5" x="6" action="MapListManialinkController/QueueMap/{{ map.Map.Uid }}"/>
          <Dropdown text="Actions" x="12" id="dpActions-{{ key }}">
              <Button text="Disable" y="0" id="btnDisable-{{ key }}" />
              <Button text="Remove" y="-5" id="btnRemove-{{ key }}"/>
              <Separator length="17" y="-10" />
              <Button text="Update" y="-10.2" id="btnUpdate-{{ key }}"/>
          </Dropdown>
      </frame>
    </Panel>
  </template>
</component>
