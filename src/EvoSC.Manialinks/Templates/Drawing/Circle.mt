<!--
Draws a circle.
-->
<component>
  <!-- Unique identifier of the circle -->
  <property type="string" name="id" default="evosc_circle" />

  <!-- Radius of the circle -->
  <property type="double" name="radius" />

  <!-- X location of the circle -->
  <property type="double" name="x" default="0.0" />

  <!-- Y location of the circle -->
  <property type="double" name="y" default="0.0" />

  <!-- Enable/disable script events for the circle -->
  <property type="bool" name="scriptEvents" default="false" />

  <!-- Background color of the circle -->
  <property type="string" name="bgColor" default="00000000" />

  <!-- Z index of the circle -->
  <property type="double" name="zIndex" default="0" />

  <!-- Whether to hide the circle by default -->
  <property type="bool" name="hidden" default="false" />

  <!-- Styling class for the circle -->
  <property type="string" name="className" default="" />
  
  <!-- ID data attribute to pass to the circle -->
  <property type="string" name="dataId" default="" />

  <!-- Opacity of the background color -->
  <property type="double" name="opacity" default="1" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" 
           size="{{ radius*2 }} {{ radius*2 }}"
           id="{{ id }}"
           z-index="{{ zIndex }}"
           class="{{ className }}"
           hidden='{{ hidden ? "1" : "0" }}'
           scriptevents='{{ scriptEvents ? "1" : "0" }}'
           data-id="{{ dataId }}"
    >
        <quad 
                size="{{ radius*2 }} {{ radius*2 }}"
                pos="0 0"
                image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                modulatecolor="{{ bgColor }}"
                class="{{ className }}"
                opacity="{{ opacity }}"
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
        />
    </frame>
  </template>
</component>
