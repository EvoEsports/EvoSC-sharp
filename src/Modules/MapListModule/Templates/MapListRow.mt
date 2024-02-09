<component>
  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Controls.Text" as="Text" />
  <import component="EvoSC.Controls.Tag" as="Tag" />
  
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  
  <template>
    <Panel width="140" 
           height="10" 
           bgColor="{{ Theme.MapListModule_MapListRow_Default_Bg }}"
           x="{{ x }}"
           y="{{ y }}"
    >
      <Checkbox id="chkSelected" x="2.5" y="-3.5" />
      
      <frame pos="7.5 -1.5">
        <label text="FMS - Bloom ft Aethal" textsize="1.8" textfont="{{ Font.Regular }}" />
        <label text="{{ Icons.User }} evo | Chroma" textsize="0.7" pos="0 -4.5" textfont="{{ Font.Thin }}"/>
      </frame>
      
      <frame pos="20 -1.5">
        <label text="PB: 0:53.360" />
        <label text="AT: 0:53.360" />
        
      </frame>
    </Panel>
  </template>
</component>
