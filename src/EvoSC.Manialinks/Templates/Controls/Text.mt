<!--
General component to show text in the default style.
-->
<component>
  <!-- Text to show -->
  <property type="string" name="text" />

  <!-- Styling class to use for this text -->
  <property type="string" name="className" default="text-primary" />

  <!-- X location of the text -->
  <property type="double" name="x" default="0" />

  <!-- Y position of the text -->
  <property type="double" name="y" default="0" />
  
  <!-- Whether the text is bold -->
  <property type="bool" name="bold" default="false" />
  
  <template>
    <label
            pos="{{ x }} {{ y }}"
            text='{{ bold ? "$o" : "" }} {{ text }}'
            class="{{ className }}"
    />
  </template>
</component>
