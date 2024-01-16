<component>
  <import component="EvoSC.Drawing.QuarterCircle" as="QuarterCircle" />
  
  <property type="string" name="id" default="evosc_panel" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" />
  <property type="double" name="height" />
  <property type="double" name="padding" default="0.0" />
  <property type="string" name="bgColor" default="00000000" />
  <property type="double" name="cornerRadius" default="0.0" />
  <property type="double" name="zIndex" default="0" />
  <property type="string" name="className" default="0" />
  <property type="double" name="rotate" default="0" />
  <property type="bool" name="hidden" default="false" />
  <property type="bool" name="scriptEvents" default="false" />
  <property type="double" name="border" default="0" />
  <property type="string" name="borderColor" default="ffffff" />
  <property type="string" name="data" default="" />
  
  <template>
    <frame if="border > 0" 
           pos="{{ x-border }} {{ y+border }}"
           size="{{ width+border*2 }} {{ height+border*2 }}"
           z-index="{{ zIndex-1 }}"
    >
      <QuarterCircle quadrant="TopLeft" radius="{{ border+cornerRadius }}" x="0" y="0" color="{{ borderColor }}" />
      <QuarterCircle quadrant="TopRight" radius="{{ border+cornerRadius }}" x="{{ width+border-cornerRadius }}" y="0" color="{{ borderColor }}" />
      <QuarterCircle quadrant="BottomLeft" radius="{{ border+cornerRadius }}" x="0" y="-{{ height+border-cornerRadius }}" color="{{ borderColor }}" />
      <QuarterCircle quadrant="BottomRight" radius="{{ border+cornerRadius }}" x="{{ width+border-cornerRadius }}" y="-{{ height+border-cornerRadius }}" color="{{ borderColor }}" />

      <quad bgcolor="{{ borderColor }}" pos="{{ cornerRadius+border }} 0" size="{{ width-cornerRadius*2 }} {{ border }}" /> <!-- Top -->
      <quad bgcolor="{{ borderColor }}" pos="0 -{{ cornerRadius+border }}" size="{{ border }} {{ height-cornerRadius*2 }}" /> <!-- Left -->
      <quad bgcolor="{{ borderColor }}" pos="{{ cornerRadius+border }} -{{ height+border }}" size="{{ width-cornerRadius*2 }} {{ border }}" /> <!-- Bottom -->
      <quad bgcolor="{{ borderColor }}" pos="{{ width+border }} -{{ cornerRadius+border }}" size="{{ border }} {{ height-cornerRadius*2 }}" /> <!-- Right -->
    </frame>
    
    <frame id="{{ id }}"
           pos="{{ x }} {{ y }}" 
           size="{{ width }} {{ height }}" 
           z-index="{{ zIndex }}" 
           class="{{ className }}"
           rot="{{ rotate }}"
           hidden='{{ hidden ? "1" : "0" }}'
           scriptevents='{{ scriptEvents ? "1" : "0" }}'
           data-data="{{ data }}"
    >
      <frame>
        <frame if="cornerRadius > 0">
          <QuarterCircle quadrant="TopLeft" radius="{{ cornerRadius }}" x="0" y="0" color="{{ bgColor }}" />
          <QuarterCircle quadrant="TopRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="0" color="{{ bgColor }}" />
          <QuarterCircle quadrant="BottomLeft" radius="{{ cornerRadius }}" x="0" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" />
          <QuarterCircle quadrant="BottomRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" />
        </frame>

        <quad bgcolor="{{ bgColor }}" pos="{{ cornerRadius }} 0" size="{{ width-cornerRadius*2 }} {{ cornerRadius }}" /> <!-- Top -->
        <quad bgcolor="{{ bgColor }}" pos="0 -{{ cornerRadius }}" size="{{ cornerRadius }} {{ height-cornerRadius*2 }}" /> <!-- Left -->
        <quad bgcolor="{{ bgColor }}" pos="{{ cornerRadius }} -{{ height-cornerRadius }}" size="{{ width-cornerRadius*2 }} {{ cornerRadius }}" /> <!-- Bottom -->
        <quad bgcolor="{{ bgColor }}" pos="{{ width-cornerRadius }} -{{ cornerRadius }}" size="{{ cornerRadius }} {{ height-cornerRadius*2 }}" /> <!-- Right -->
      </frame>
      
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
