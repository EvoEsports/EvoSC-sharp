<!--
Draws a quarter of a circle from different quadrants.
-->
<component>
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
  
  <!-- Styling class to pass to the quarter -->
  <property type="string" name="className" default="" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" size="{{ radius }} {{ radius }}" >
      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="0 0"
              if='quadrant == "TopLeft"'
              opacity="{{ Util.ColorOpacity(color) }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="-{{ radius }} 0"
              if='quadrant == "TopRight"'
              opacity="{{ Util.ColorOpacity(color) }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="0 {{ radius }}"
              if='quadrant == "BottomLeft"'
              opacity="{{ Util.ColorOpacity(color) }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="-{{ radius }} {{ radius }}"
              if='quadrant == "BottomRight"'
              opacity="{{ Util.ColorOpacity(color) }}"
              class="{{ className }}"
              scriptevents="{{ scriptevents ? 1 : 0 }}"
      />
    </frame>
  </template>
</component>