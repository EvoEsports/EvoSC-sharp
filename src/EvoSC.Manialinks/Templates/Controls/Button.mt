<!--
    Generic clickable button.
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
  
  <!-- Height of the button background, size must be set to "custom" to take effect. -->
  <property type="double" name="height" default="5.0" />
  
  <!-- Size of the button can be normal, small, big or custom. -->
  <property type="string" name="size" default="normal" />
  
  <!-- The button style type, can be primary, secondary or accent. -->
  <property type="string" name="type" default="primary" />
  
  <!-- Possible values: normal, round -->
  <property type="string" name="style" default="normal" />
  
  <!-- The action to call when clicking the button. This disables script events. -->
  <property type="string" name="action" default="" />
  
  <!-- Whether the button is disabled or not. If disabled, the button wont fire events. -->
  <property type="bool" name="disabled" default="false" />
  
  <!-- Custom style -->
  <property type="string" name="className" default="evosc-button" />
  
  <!-- Custom data passed to the component -->
  <property type="string" name="data" default="" />
  
  <template>
    <frame
            id="{{ id }}-frame"
            pos="{{ x }} {{ y }}"
            data-type="{{ type }}"
    >
      <!-- Background -->
      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ width-1 }} {{ Theme.UI_Button_Size(size, height) }}"
              pos="0.5 0"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
              if='style == "normal"'
      />

      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ 0.5 }} {{ Theme.UI_Button_Size(size, height)-1 }}"
              pos="0 -0.5"
              scriptevents="{{ disabled ? 0 : 1 }}"
              data-id="{{ id }}"
              if='style == "normal"'
      />

      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ 0.5 }} {{ Theme.UI_Button_Size(size, height)-1 }}"
              pos="{{ width-0.5 }} -0.5"
              data-id="{{ id }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              if='style == "normal"'
      />

      <quad
              class='btn-bg-{{ disabled ? "disabled" : type }} {{ id }}-surface'
              size="{{ width - Theme.UI_Button_Size(size, height) }} {{ Theme.UI_Button_Size(size, height) }}"
              pos="{{ Theme.UI_Button_Size(size, height)/2.0 }} 0"
              data-id="{{ id }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              if='style == "round"'
      />

      <QuarterCircle
              className="{{ id }}-surface"
              radius='{{ style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2 }}'
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : Theme.UI_Button_Bg(type) }}'
              quadrant="TopLeft"
              scriptevents="{{ !disabled }}"
              id="{{ id }}"
      />

      <QuarterCircle
              className="{{ id }}-surface"
              radius='{{ style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2 }}'
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : Theme.UI_Button_Bg(type) }}'
              quadrant="TopRight"
              x='{{ width-(style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2) }}'
              scriptevents="{{ !disabled }}"
              id="{{ id }}"
      />
      
      <QuarterCircle
              className="{{ id }}-surface"
              radius='{{ style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2 }}'
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : Theme.UI_Button_Bg(type) }}'
              quadrant="BottomLeft"
              y='{{ -Theme.UI_Button_Size(size, height)+(style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2) }}'
              scriptevents="{{ !disabled }}"
              id="{{ id }}"
      />
      
      <QuarterCircle
              className="{{ id }}-surface"
              radius='{{ style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2 }}'
              color='{{ disabled ? Theme.UI_Button_Disabled_Bg : Theme.UI_Button_Bg(type) }}'
              quadrant="BottomRight"
              x='{{ width-(style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2) }}'
              y='{{ -Theme.UI_Button_Size(size, height)+(style == "normal" ? 0.5 : Theme.UI_Button_Size(size, height)/2) }}'
              scriptevents="{{ !disabled }}"
              id="{{ id }}"
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
              size="{{ width }} {{ Theme.UI_Button_Size(size, height) }}"
              pos="{{ width/2 }} {{ -Theme.UI_Button_Size(size, height)/2+0.3 }}"
              data-id="{{ id }}"
              data-data="{{ data }}"
              scriptevents="{{ disabled ? 0 : 1 }}"
              action="{{ action }}"
              id="{{ id }}"
      />
    </frame>
  </template>

  <script resource="EvoSC.Scripts.Button" once="true" />
</component>
