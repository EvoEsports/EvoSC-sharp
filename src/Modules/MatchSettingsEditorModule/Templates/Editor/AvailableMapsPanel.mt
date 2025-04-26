<component>
  
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="EvoSC.Controls.Loader" as="Loader" />
  
  <property type="double" name="width" />
  <property type="double" name="height" />
  <property type="double" name="x" />
  <property type="double" name="y" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" size="{{ width }} {{ height }}" id="amp-frame" hidden="0">
      <Rectangle width="{{ width }}" 
                 height="{{ height }}" 
                 bgColor="{{ Theme.UI_BgPrimary }}"
                 cornerRadius="1"
                 corners="TopRight,BottomRight" />
      
      <label class="text-primary" 
             font="Font.Bold" 
             text="CHOSE MAPS"
             pos="2 -2.5"
             valign="center"
      />
      
      <Separator x="2" y="-7" length="{{ width - 4 }}" />
      
      <Loader x="{{ width/2 }}" y="{{ -height/2 }}" align="CenterMiddle" />
      
    </frame>
  </template>
</component>
