<!--
Can contain a set of components which can be positioned relative
to the container, along with various transforms and properties.
-->
<component>
  <!-- Unique identifier for the container -->
  <property type="string" name="id" default="evosc_container" />

  <!-- X location of the container -->
  <property type="double" name="x" default="0.0" />

  <!-- Y location of the container -->
  <property type="double" name="y" default="0.0" />

  <!-- Width of the container -->
  <property type="double" name="width" default="0.0" />

  <!-- Height of the container -->
  <property type="double" name="height" default="0.0" />

  <!-- Whether the contents of the container can be scrolled -->
  <property type="bool" name="scrollable" default="false" />

  <!-- Whether the scrolling will snap to a grid -->
  <property type="bool" name="scrollGridSnap" default="false" />

  <!-- Height of the scrollable area. This should be container <contents height> - <container height> -->
  <property type="double" name="scrollHeight" default="0.0" />

  <!-- Width of the scrollable area. This should be container <contents width> - <container width> -->
  <property type="double" name="scrollWidth" default="0.0" />

  <!-- Intervals to snap the scrolling to for the X axis -->
  <property type="double" name="scrollGridX" default="0.0" />

  <!-- Intervals to snap the scrolling to for the Y axis -->
  <property type="double" name="scrollGridY" default="0.0" />

  <!-- Z index of the container -->
  <property type="double" name="zIndex" default="0" />

  <!-- Rotation of the container in degrees -->
  <property type="double" name="rotate" default="0" />

  <!-- Scale of the container -->
  <property type="double" name="scale" default="1" />

  <!-- Styling class to pass to the container -->
  <property type="string" name="className" default="" />
  
  <!-- Whether the container is hidden by default -->
  <property type="bool" name="hidden" default="false" />
  
  <template>
    <frame 
            id='{{ Util.DefaultOrRandomId("evosc_container", id) }}'
            pos="{{ x }} {{ y }}"
            size="{{ width }} {{ height }}"
            scriptevents="1"
            z-index="{{ zIndex }}"
            rot="{{ rotate }}"
            scale="{{ scale }}"
            class="{{ className }}"
            hidden='{{ hidden ? "1" : "0" }}'
    >
      <quad size="9999 9999" pos="0 0" if="scrollable" scriptevents="1" />
      <slot />
    </frame>
  </template>

  <script resource="EvoSC.Scripts.Container" />
</component>
