<!--
Generic tag that can be used to display additional info, values or status.
-->
<component>
  <import component="EvoSC.Controls.Panel" as="Panel" />

  <!-- Unique identifier of the tag -->
  <property type="string" name="id" default="evosc_tag" />

  <!-- Text to show on the tag -->
  <property type="string" name="text" default="" />

  <!-- X position of the tag -->
  <property type="double" name="x" default="0.0" />

  <!-- Y position of the tag -->
  <property type="double" name="y" default="0.0" />

  <!-- Width of the tag -->
  <property type="double" name="width" default="15" />

  <!-- Height of the tag -->
  <property type="double" name="height" default="3" />

  <!-- Layout style of the tag, can be: Round or Square -->
  <property type="string" name="style" default="Square" /> <!-- Styles: Round, Square -->

  <!-- Whether the tag is closable -->
  <property type="string" name="closable" default="false" />

  <!-- Whether the tag is hidden by default -->
  <property type="bool" name="hidden" default="false" />

  <!-- Severity color of the tag, can be: primary, secondary, success, info, warning or danger -->
  <property type="string" name="severity" default="primary" /> <!-- primary, secondary, success, info, warning, danger -->

  <!-- Custom background color of the tag -->
  <property type="string?" name="bgColor" default="" />

  <!-- Custom text color of the tag -->
  <property type="string?" name="textColor" default="" />
  
  <!-- Whether to center the text on the tag background -->
  <property type="bool" name="centerText" default="true" />
  
  <template>
    <Panel
            x="{{ x }}"
            y="{{ y }}"
            id="{{ id }}"
            width="{{ width }}"
            height="{{ height }}"
            cornerRadius='{{ style == "Round" ? 1.5 : 0 }}'
            bgColor='{{ string.IsNullOrEmpty(bgColor) ? Util.TypeToColorBg(severity) : bgColor }}'
            data="{{ closable }}"
            hidden="{{ hidden }}"
    >
      <label
              class="text-primary"
              pos="{{ centerText ? width/2.0 : 0.5 }} {{ -height/2+0.2 }}"
              textcolor="{{ string.IsNullOrEmpty(textColor) ? Util.TypeToColorText(severity) : textColor }}"
              size="{{ width }} 5"
              text="{{ text }}"
              textsize="0.4"
              valign="center"
              halign='{{ centerText ? "center" : "left" }}'
      />

      <slot name="content" />
    </Panel>
  </template>
</component>
