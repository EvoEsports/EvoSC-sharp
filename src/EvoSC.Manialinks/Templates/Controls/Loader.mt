<component>
  <!-- The ID of the component -->
  <property type="string" name="id" default="evosc-loader" />

  <!-- X location of the loader -->
  <property type="double" name="x" default="0.0" />

  <!-- Y location of the loader -->
  <property type="double" name="y" default="0.0" />

  <!-- Size (width and height) of the loader -->
  <property type="double" name="size" default="5.0" />

  <!-- Color of the loader -->
  <property type="string" name="color" default="FFFFFF" />
  
  <!-- The anchor point of the loader, can be either: TopLeft, TopMiddle, TopRight, CenterLeft, CenterMiddle, CenterRight, BottomLeft, BottomMiddle or BottomRight -->
  <property type="string" name="anchor" default="TopLeft" />
  
  <template>
    <quad pos="{{ x }} {{ y }}"
          size="{{ size }} {{ size }}"
          image="file://Media/Manialinks/Nadeo/CMGame/Utils/Icons/256x256/animated_loading_spinner.webm"
          colorize="{{ color }}"
          autosize="1"
          valign="{{ Util.VerticalAlign(anchor) }}"
          halign="{{ Util.HorizontalAlign(anchor) }}"
    />
  </template>
</component>
