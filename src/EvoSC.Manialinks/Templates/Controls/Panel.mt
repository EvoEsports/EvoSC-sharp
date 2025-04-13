<!--
Customizable background panel which can contain components.
Features multiple properties such as background, border, rounded corners, and transforms.
-->
<component>
  <import component="EvoSC.Drawing.QuarterCircle" as="QuarterCircle" />

  <!-- Unique identifier of the panel. -->
  <property type="string" name="id" default="evosc_panel" />

  <!-- X position of the panel. -->
  <property type="double" name="x" default="0.0" />

  <!-- Y position of the panel. -->
  <property type="double" name="y" default="0.0" />

  <!-- Width of the panel. -->
  <property type="double" name="width" />

  <!-- Height of the panel -->
  <property type="double" name="height" />

  <!-- Padding from the outside border to the panel content. -->
  <property type="double" name="padding" default="0.0" />

  <!-- Background color of the panel. -->
  <property type="string" name="bgColor" default="00000000" />

  <!-- Radius of the corners for the panel, used for rounded corners. -->
  <property type="double" name="cornerRadius" default="0.0" />

  <!-- The Z index of the panel. -->
  <property type="double" name="zIndex" default="0" />

  <!-- Custom style class of the panel. -->
  <property type="string" name="className" default="0" />

  <!-- Rotation of the panel in degrees -->
  <property type="double" name="rotate" default="0" />

  <!-- Whether the panel is hidden or not by default -->
  <property type="bool" name="hidden" default="false" />

  <!-- Enable/disable script events on the panel's components. -->
  <property type="bool" name="scriptEvents" default="false" />

  <!-- The thickness of the panel's border. -->
  <property type="double" name="border" default="0" />

  <!-- Color of the panel's border. -->
  <property type="string" name="borderColor" default="ffffff" />

  <!-- Custom data attribute to set. -->
  <property type="string" name="data" default="a" />

  <!-- ID data attribute to set. -->
  <property type="string" name="dataId" default="" />
  
  <!-- Whether the panel allows contents to overflow its boundaries. If disabled, contents is cut off. -->
  <property type="bool" name="overflow" default="false" />
  
  <template>
    <frame if="border > 0" 
           pos="{{ x-border }} {{ y+border }}"
           size='{{ overflow ? "" : $"{width+border*2} {height+border*2}" }}'
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
           z-index="{{ zIndex }}" 
           class="{{ className }}"
           rot="{{ rotate }}"
           hidden='{{ hidden ? "1" : "0" }}'
           scriptevents="{{ scriptEvents ? 1 : 0 }}"
           data-data="{{ data }}"
           data-id="{{ dataId }}"
    >
      <frame>
        <frame if="cornerRadius > 0">
          <QuarterCircle quadrant="TopLeft" radius="{{ cornerRadius }}" x="0" y="0" color="{{ bgColor }}" />
          <QuarterCircle quadrant="TopRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="0" color="{{ bgColor }}" />
          <QuarterCircle quadrant="BottomLeft" radius="{{ cornerRadius }}" x="0" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" />
          <QuarterCircle quadrant="BottomRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" />
        </frame>

        <quad bgcolor="{{ bgColor }}"
              pos="{{ cornerRadius }} 0" 
              size="{{ width-cornerRadius*2 }} {{ cornerRadius }}"
              scriptevents="{{ scriptEvents ? 1 : 0 }}"
              data-data="{{ data }}"
              data-id="{{ dataId }}"
              class="{{ className }}"
        /> <!-- Top -->
        
        <quad bgcolor="{{ bgColor }}" 
              pos="0 -{{ cornerRadius }}" 
              size="{{ cornerRadius }} {{ height-cornerRadius*2 }}"
              scriptevents="{{ scriptEvents ? 1 : 0 }}"
              data-data="{{ data }}"
              data-id="{{ dataId }}"
              class="{{ className }}"
        /> <!-- Left -->
        
        <quad bgcolor="{{ bgColor }}" 
              pos="{{ cornerRadius }} -{{ height-cornerRadius }}" 
              size="{{ width-cornerRadius*2 }} {{ cornerRadius }}"
              scriptevents="{{ scriptEvents ? 1 : 0 }}"
              data-data="{{ data }}"
              data-id="{{ dataId }}"
              class="{{ className }}"
        /> <!-- Bottom -->
        
        <quad bgcolor="{{ bgColor }}" 
              pos="{{ width-cornerRadius }} -{{ cornerRadius }}"
              size="{{ cornerRadius }} {{ height-cornerRadius*2 }}"
              scriptevents="{{ scriptEvents ? 1 : 0 }}"
              data-data="{{ data }}"
              data-id="{{ dataId }}"
              class="{{ className }}"
        /> <!-- Right -->
      </frame>
      
      <quad
              pos="{{ cornerRadius }} {{ -cornerRadius }}" 
              size="{{ width-cornerRadius*2 }} {{ height-cornerRadius*2 }}"
              bgcolor="{{ bgColor }}"
              scriptevents="{{ scriptEvents ? 1 : 0 }}"
              data-data="{{ data }}"
              data-id="{{ dataId }}"
              class="{{ className }}"
      />
      <frame pos="{{ padding }} {{ -padding }}">
        <slot />
      </frame>
    </frame>
  </template>
</component>
