<!--
Chips can be used as a way to visualize multiple values in a field.
-->
<component>
  <import component="EvoSC.Controls.Tag" as="Tag" />
  
  <!-- Unique identifier for the chip -->
  <property type="string" name="id" default="evosc_chip" />

  <!-- Text to show on the chip -->
  <property type="string" name="text" default="" />

  <!-- X Position of the chip -->
  <property type="double" name="x" default="0.0" />

  <!-- Y position of the chip -->
  <property type="double" name="y" default="0.0" />

  <!-- Width of the chip -->
  <property type="double" name="width" default="15" />

  <!-- The layout style, can be: Round or Square -->
  <property type="string" name="style" default="Square" /> <!-- Styles: Round, Square -->

  <!-- Whether this chip can be closed/removed/hidden. Shows a close button -->
  <property type="bool" name="closable" default="false" />

  <!-- Whether to hide this chip by default -->
  <property type="bool" name="hidden" default="false" />

  <!-- The severity color of the chip, can be: primary, secondary, success, info, warning, danger  -->
  <property type="string" name="severity" default="primary" /> <!-- primary, secondary, success, info, warning, danger -->

  <!-- Custom background color of the chip -->
  <property type="string?" name="bgColor" default="" />

  <!-- Custom text color of the chip -->
  <property type="string?" name="textColor" default="" />
  
  <template>
    <Tag 
            id="{{ id }}"
            text="{{ text }}"
            x="{{ x }}"
            y="{{ y }}"
            width="{{ width }}"
            style="{{ style }}"
            closable="{{ closable }}"
            hidden="{{ hidden }}"
            severity="{{ severity }}"
            bgColor="{{ bgColor }}"
            textColor="{{ textColor }}"
            centerText="false"
    >
      <template slot="content">
        <label
                text="{{ Icons.TimesCircle }}"
                textcolor="{{ string.IsNullOrEmpty(textColor) ? Util.TypeToColorText(severity) : textColor }}"
                textsize="0.7"
                pos="{{ width-2.7 }} -1.35"
                valign="center"
                scriptevents="1"
                class="chip-btnClose"
        />
      </template>
    </Tag>
    <!-- <Panel
            x="{{ x }}"
            y="{{ y }}"
            id="{{ id }}"
            width="{{ width }}"
            height="3"
            cornerRadius='{{ style == "Round" ? 2.5 : 0 }}'
            bgColor="{{ Theme.UI_Chip_Default_Bg }}"
            data="{{ closable }}"
            hidden="{{ hidden }}"
    >
      <label 
              class="text"
              pos="0.4 -1.2" 
              textcolor="{{ Theme.UI_Chip_Default_Text }}"
              size="{{ width }} 5"
              text="{{ text }}" 
              textsize="1"
              valign="center"
      />
      
      <label 
              text="{{ Icons.TimesCircle }}"
              textcolor="{{ Theme.UI_Chip_Default_Text }}"
              textsize="0.7"
              pos="{{ width-3 }} -1.35"
              valign="center"
              scriptevents="1"
              class="chip-btnClose"
      />
    </Panel> -->
  </template>
  
  <script resource="EvoSC.Scripts.Chip" once="true" />
</component>
