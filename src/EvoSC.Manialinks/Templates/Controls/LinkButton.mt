<!--
Opens a link in the user's web browser when clicked.
-->
<component>
  <import component="EvoSC.Controls.Button" as="Button" />

  <!-- The ID of the button. -->
  <property type="string" name="id" />

  <!-- Text to display in the button -->
  <property type="string" name="text" />
  
  <!-- The URL to open in the player's default web browser -->
  <property type="string" name="url" />

  <!-- X position of the button. -->
  <property type="double" name="x" default="0.0" />

  <!-- Y position of the button. -->
  <property type="double" name="y" default="0.0" />

  <!-- Width of the button background. -->
  <property type="double" name="width" default="25.0" />

  <!-- Height of the button background. -->
  <property type="double" name="height" default="5.0" />

  <property type="string?" name="bgColor" default="null" />

  <!-- The button style type, can be primary or secondary. -->
  <property type="string" name="type" default="primary" />

  <!-- The action to call when clicking the button. This disables script events. -->
  <property type="string" name="action" default="" />

  <!-- Whether the button is disabled or not. If disabled, the button wont fire events. -->
  <property type="bool" name="disabled" default="false" />

  <!-- Class to assign to the button. -->
  <property type="string" name="className" default="evosc-linkbutton" />
  
  <template>
      <Button
              id="{{ id }}"
              text="{{ text }}"
              x="{{ x }}"
              y="{{ y }}"
              width="{{ width }}"
              height="{{ height }}"
              disabled="{{ disabled }}"
              className="{{ className }}"
              bgColor="{{ bgColor }}"
              type="{{ type }}"
      />
  </template>
  
  <script>
      <!--
      *** OnMouseClick ***
      ***
          if (Event.Control.ControlId == "{{ id }}") {
              OpenLink("{{ url }}", CMlScript::LinkType::ExternalBrowser);
          }
      ***
      -->
  </script>
</component>