<!--
Draws a quarter of a circle from different quadrants.
-->
<component>
  <!-- The ID of the element. -->
  <property type="string" name="id" default="" />
    
  <!-- Radius of the quarter -->
  <property type="double" name="radius" />

  <!-- X location of the quarter -->
  <property type="double" name="x" default="0.0" />

  <!-- Y location of the quarter -->
  <property type="double" name="y" default="0.0" />

  <!-- Background color of the quarter -->
  <property type="string" name="color" />

  <!-- Enable/disable script events for the quarter -->
  <property type="bool" name="scriptevents" default="false" />

  <!-- Action to trigger when mouse is clicked on the quarter -->
  <property type="string" name="action" default="" />
  
  <!-- The quadrant of the quarter, can be: TopLeft, TopRight, BottomLeft or BottomRight -->
  <property type="string" name="quadrant" />

  <!-- Controls the opacity of the background -->
  <property type="double" name="opacity" default="1.0" />
  
  <!-- Styling class to pass to the quarter -->
  <property type="string" name="className" default="" />

  <!-- Data Id attribute to set -->
  <property type="string" name="dataId" default="" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" size="{{ radius }} {{ radius }}" data-id="{{ id }}">
      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="0 0"
              if='quadrant == "TopLeft"'
              opacity="{{ opacity }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
              id="{{ id }}-quarter-topleft"
              data-id="{{ dataId }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="-{{ radius }} 0"
              if='quadrant == "TopRight"'
              opacity="{{ opacity }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
              id="{{ id }}-quarter-topright"
              data-id="{{ dataId }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="0 {{ radius }}"
              if='quadrant == "BottomLeft"'
              opacity="{{ opacity }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
              id="{{ id }}-quarter-bottomleft"
              data-id="{{ dataId }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="-{{ radius }} {{ radius }}"
              if='quadrant == "BottomRight"'
              opacity="{{ opacity }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
              id="{{ id }}-quarter-bottomright"
              data-id="{{ dataId }}"
      />
    </frame>
  </template>
</component>