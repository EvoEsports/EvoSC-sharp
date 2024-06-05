<component>
  <property type="string" name="header" default="" />
  <property type="string" name="position" default="left" /> <!-- Values: left, right, top, bottom -->
  <property type="double" name="height" />
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="string" name="style" default="normal" /> <!-- Values: normal, unstyled -->
  <property type="bool" name="ingrid" default="true" /> <!-- If true, position and size acts on the grid system -->
  <property type="double" name="gridsize" default="12" /> <!-- If true, position and size acts on the grid system -->
  <property type="double" name="cellsize" default="10" /> <!-- If true, position and size acts on the grid system -->
  
  <template>
    <frame pos="">
      <frame if='header != ""'>
        <quad
                bgcolor="28212F"
                size="37 5"
                opacity="0.95"
                pos="0.8 0"
                if='style.ToLower() == "normal"'
        />
        <quad
                bgcolor="ff0058"
                size="0.8 5"
                pos="0 0"
                if='style.ToLower() == "normal"'
        />
        <label 
                text="{{ header.ToUpper() }}" 
                textfont="GameFontExtraBold"
                textsize="0.8"
                pos="18.5 -2.4"
                halign="center"
                valign="center"
                if='style.ToLower() == "normal"'
        />
        <slot name="header" />
      </frame>
      <frame pos='0 {{ -5 - (header == "" ? 0 : 0.3) }}'>
        <quad
                size="37 {{ height }}"
                bgcolor="2C2D34"
                opacity="0.9"
                pos="0.8 0"
                if='style.ToLower() == "normal"'
        />
        <quad
                size="0.8 {{ height }}"
                pos="0 0"
                bgcolor="ff0058"
                if='style.ToLower() == "normal"'
        />
        <slot name="body" />
      </frame>
    </frame>
  </template>
</component>
