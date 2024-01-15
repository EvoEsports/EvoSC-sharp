<component>
  <property type="double" name="radius" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="string" name="color" />
  
  <!-- Possible values: TopLeft, TopRight, BottomLeft, BottomRight -->
  <property type="string" name="quadrant" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" size="{{ radius }} {{ radius }}" >
      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="0 0"
              if='quadrant == "TopLeft"'
              opacity="{{ Util.ColorOpacity(color) }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="-{{ radius }} 0"
              if='quadrant == "TopRight"'
              opacity="{{ Util.ColorOpacity(color) }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="0 {{ radius }}"
              if='quadrant == "BottomLeft"'
              opacity="{{ Util.ColorOpacity(color) }}"
      />

      <quad
              size="{{ radius*2 }} {{ radius*2 }}"
              modulatecolor="{{ color }}"
              image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
              pos="-{{ radius }} {{ radius }}"
              if='quadrant == "BottomRight"'
              opacity="{{ Util.ColorOpacity(color) }}"
      />
    </frame>
  </template>
</component>