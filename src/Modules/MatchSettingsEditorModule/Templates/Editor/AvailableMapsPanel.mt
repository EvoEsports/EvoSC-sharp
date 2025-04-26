<component>
  
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  
  <property type="double" name="width" />
  <property type="double" name="height" />
  <property type="double" name="x" />
  <property type="double" name="y" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" size="{{ width }} {{ height }}" id="amp-frame" hidden="1">
      <Rectangle width="{{ width }}" 
                 height="{{ height }}" 
                 bgColor="{{ Theme.UI_SurfaceBgPrimary }}"
                 cornerRadius="1"
                 corners="TopRight,BottomRight"
      />
    </frame>
  </template>
</component>
