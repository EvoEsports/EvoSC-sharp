<component>
  <property type="string" name="header" default="" />
  <property type="string" name="position" default="left" /> <!-- Values: left, right, top, bottom -->
  <property type="double" name="height" />
  <property type="double" name="width" default="36" />
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="double" name="padding" default="1.5" />
  <property type="string" name="headerStyle" default="normal" /> <!-- Values: normal, unstyled -->
  <property type="string" name="bodyStyle" default="normal" /> <!-- Values: normal, unstyled -->
  <property type="bool" name="ingrid" default="true" /> <!-- If true, position and size acts on the grid system -->
  <property type="double" name="gridsize" default="12" /> <!-- If true, position and size acts on the grid system -->
  <property type="double" name="cellsize" default="10" /> <!-- If true, position and size acts on the grid system -->
  
  <template>
    <frame pos='{{ (position == "left" ? -160 + padding : 160-width - padding - 0.7 ) }} {{ y }}'>
      <frame if='header != ""'>
        <quad
                bgcolor="{{ Theme.UI_HeaderBg }}"
                size="{{ width }} 4.5"
                opacity="0.95"
                pos='{{ position == "left" ? 0.7 : 0 }} 0'
                if='headerStyle.ToLower() == "normal"'
        />
        <quad
                bgcolor="{{ Theme.UI_AccentPrimary }}"
                size="0.7 4.5"
                pos='{{ position == "left" ? 0 : width }} 0'
                if='headerStyle.ToLower() == "normal"'
        />
        <label 
                text="{{ header.ToUpper() }}" 
                textfont="{{ Font.Bold }}"
                textsize="0.5"
                pos="{{ width/2.0 + 1 }} -2.1"
                halign="center"
                valign="center"
                if='headerStyle.ToLower() == "normal"'
        />
        <slot name="header" />
      </frame>
      <frame pos='0 {{ -4.5 - (header == "" ? 0 : 0.3) }}'>
        <quad
                size="{{ width }} {{ height }}"
                bgcolor="{{ Theme.UI_BgPrimary }}"
                opacity="0.9"
                pos='{{ position == "left" ? 0.7 : 0 }} 0'
                if='bodyStyle.ToLower() == "normal"'
        />
        <quad
                size="0.7 {{ height }}"
                pos='{{ position == "left" ? 0 : width }} 0'
                bgcolor="{{ Theme.UI_AccentPrimary }}"
                if='bodyStyle.ToLower() == "normal"'
        />
        <slot name="body" />
      </frame>
    </frame>
  </template>
</component>
