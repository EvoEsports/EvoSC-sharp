<component>
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
  <property type="double" name="width" default="170" />
  
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
          <label text="FMS - Bloom ft Aethal" textsize="1.8" textfont="{{ Font.Regular }}" size="30 5" />
          <label text="{{ Icons.User }} evo | Chroma" textsize="0.7" pos="0 -4.5" textfont="{{ Font.Thin }}"/>
      </frame>

        <!-- PB and AT -->
      <frame pos="40 -1.5">
          <label text="PB:" textsize="0.7" class="text" />
          <label text="$AAA0:$g53.360" textsize="0.7" pos="5 0" class="text" />
          <Tag text="888" x="16" style="Round" width="5" bgColor="888888" />

          <label text="AT:" textsize="0.7"  pos="0 -4.5" class="text"/>
          <label text="$AAA0:$g53.360" textsize="0.7"  pos="5 -4.5" class="text" />
      </frame>

        <!-- Tags and links -->
        <frame pos="63 -1.5">
            <label class="text" text="Tags: " x="1" />
            <Tag style="Square" text="Magnet" x="9" bgColor="{{ Theme.MapListModule_MapListRow_Default_TagBg }}" />
            <Tag style="Square" text="Speed Drift" x="25" bgColor="{{ Theme.MapListModule_MapListRow_Default_TagBg }}" />
            <Tag style="Square" text=".." x="41" width="3" bgColor="{{ Theme.MapListModule_MapListRow_Default_TagBg }}" />
            
            <label class="text" text="Links: " pos="0 -4.5" />
            <LinkButton text="TMX" url="https://trackmania.exchange" x="9" y="-4.5" id="linkTmx" bgColor="{{ Theme.UI_BgSecondary }}" width="8" />
            <LinkButton text="TM.IO" url="https://trackmania.exchange" x="18" y="-4.5" id="linkTmx" bgColor="{{ Theme.UI_BgSecondary }}" width="10" />
        </frame>
        
        <!-- Map rating -->
        <frame pos="110 -4.3">
            <Rating />
        </frame>
        
        <!-- Action buttons -->
      <frame pos="{{ width - 34 }} -2.5">
          <Button text="{{ Icons.HeartO }}" id="btnFavorite-{{ key }}" width="5" />
          <Button text="{{ Icons.PlayCircle }}" id="btnQueue-{{ key }}" width="5" x="6"/>
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
