<component>
  <import component="EvoSC.Drawing.QuarterCircle" as="QuarterCircle" />
  
  <property type="string" name="id" default="evosc_panel" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" />
  <property type="double" name="height" />
  <property type="double" name="padding" default="0.0" />
  <property type="string" name="bgColor" />
  <property type="double" name="cornerRadius" default="0.0" />
  <property type="double" name="borderThickness" default="0.0" />
  <property type="string" name="borderColor" default="0.0" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" size="{{ width }} {{ height }}">
      <QuarterCircle quadrant="TopLeft" radius="{{ cornerRadius }}" x="0" y="0" color="{{ bgColor }}" />
      <QuarterCircle quadrant="TopRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="0" color="{{ bgColor }}" />
      <QuarterCircle quadrant="BottomLeft" radius="{{ cornerRadius }}" x="0" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" />
      <QuarterCircle quadrant="BottomRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" />

      <quad bgcolor="{{ bgColor }}" pos="{{ cornerRadius }} 0" size="{{ width-cornerRadius*2 }} {{ cornerRadius }}" /> <!-- Top -->
      <quad bgcolor="{{ bgColor }}" pos="0 -{{ cornerRadius }}" size="{{ cornerRadius }} {{ height-cornerRadius*2 }}" /> <!-- Left -->
      <quad bgcolor="{{ bgColor }}" pos="{{ cornerRadius }} -{{ height-cornerRadius }}" size="{{ width-cornerRadius*2 }} {{ cornerRadius }}" /> <!-- Bottom -->
      <quad bgcolor="{{ bgColor }}" pos="{{ width-cornerRadius }} -{{ cornerRadius }}" size="{{ cornerRadius }} {{ height-cornerRadius*2 }}" /> <!-- Right -->
      
      <quad
              pos="{{ cornerRadius }} {{ -cornerRadius }}" 
              size="{{ width-cornerRadius*2 }} {{ height-cornerRadius*2 }}"
              bgcolor="{{ bgColor }}"
      />
      <frame pos="{{ padding }} {{ -padding }}" size="{{ width-padding*2 }} {{ height-padding*2 }}">
        <slot />
      </frame>
    </frame>
  </template>
</component>