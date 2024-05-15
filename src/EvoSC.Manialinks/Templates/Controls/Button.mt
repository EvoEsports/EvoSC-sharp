<!--
    Simple clickable button.
-->
<component>
  <import component="EvoSC.Drawing.QuarterCircle" as="QuarterCircle" />
  
  <!-- The ID of the button. -->
  <property type="string" name="id" />
  
  <!-- Text to display in the button -->
  <property type="string" name="text" default="" />
  
  <!-- X position of the button. -->
  <property type="double" name="x" default="0.0" />
  
  <!-- Y position of the button. -->
  <property type="double" name="y" default="0.0" />
  
  <!-- Width of the button background. -->
  <property type="double" name="width" default="17.0" />
  
  <!-- Height of the button background. -->
  <property type="double" name="height" default="5.0" />
  
  <!-- The button style type, can be primary or secondary. -->
  <property type="string" name="type" default="primary" />
  
  <property type="string?" name="bgColor" default="null" />
  
  <!-- The action to call when clicking the button. This disables script events. -->
  <property type="string" name="action" default="" />
  
  <!-- Whether the button is disabled or not. If disabled, the button wont fire events. -->
  <property type="bool" name="disabled" default="false" />
  
  <!-- Custom style -->
  <property type="string" name="className" default="evosc-button" />
  
  <template>
    <frame
            id="{{ id }}-frame"
            pos="{{ x }} {{ y }}"
            data-type="{{ type }}"
    >
      <!-- Background -->
      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ width-1 }} {{ height }}"
              pos="0.5 0"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
      />

      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ 0.5 }} {{ height-1 }}"
              pos="0 -0.5"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
      />

      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ 0.5 }} {{ height-1 }}"
              pos="{{ width-0.5 }} -0.5"
              data-id="{{ id }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
      />

      <QuarterCircle
              className="{{ id }}-surface"
              radius="0.5"
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : (type == "primary" ? Theme.UI_SurfaceBgSecondary : Theme.UI_SurfaceBgPrimary) }}'
              quadrant="TopLeft"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
              enableScriptEvents="true"
      />

      <QuarterCircle
              className="{{ id }}-surface"
              radius="0.5"
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : (type == "primary" ? Theme.UI_SurfaceBgSecondary : Theme.UI_SurfaceBgPrimary) }}'
              quadrant="TopRight"
              x="{{ width-0.5 }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
              enableScriptEvents="true"
      />
      <QuarterCircle
              className="{{ id }}-surface"
              radius="0.5"
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : (type == "primary" ? Theme.UI_SurfaceBgSecondary : Theme.UI_SurfaceBgPrimary) }}'
              quadrant="BottomLeft"
              y="{{ -height+0.5 }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
              enableScriptEvents="true"
      />
      <QuarterCircle
              className="{{ id }}-surface"
              radius="0.5"
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : (type == "primary" ? Theme.UI_SurfaceBgSecondary : Theme.UI_SurfaceBgPrimary) }}'
              quadrant="BottomRight"
              x="{{ width-0.5 }}"
              y="{{ -height+0.5 }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
              enableScriptEvents="true"
      />

      <!-- Text -->
      <label
              focusareacolor1="00000000"
              focusareacolor2="00000000"
              class='btn-part btn-btn btn-text-{{ disabled ? "disabled" : type }} {{ className }}-btn'
              textfont="{{ Font.Regular }}"
              text="{{ text.ToUpper() }}"
              valign="center"
              halign="center"
              size="{{ width }} {{ height }}"
              pos="{{ width/2 }} {{ -height/2+0.3 }}"
              data-id="{{ id }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              action="{{ action }}"
              id="{{ id }}"
      />
    </frame>
  </template>

  <script resource="EvoSC.Scripts.Button" once="true" />
</component>
