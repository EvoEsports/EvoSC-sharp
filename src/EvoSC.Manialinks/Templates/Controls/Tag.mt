<component>
  <import component="EvoSC.Controls.Panel" as="Panel" />
  
  <property type="string" name="id" default="evosc_tag" />
  <property type="string" name="text" default="" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="15" />
  <property type="double" name="height" default="3" />
  <property type="string" name="style" default="Square" /> <!-- Styles: Round, Square -->
  <property type="string" name="closable" default="false" />
  <property type="bool" name="hidden" default="false" />
  <property type="string" name="severity" default="primary" /> <!-- primary, secondary, success, info, warning, danger -->
  <property type="string?" name="bgColor" default="" />
  <property type="string?" name="textColor" default="" />
  <property type="bool" name="centerText" default="true" />
  
  <template>
    <Panel
            x="{{ x }}"
            y="{{ y }}"
            id="{{ id }}"
            width="{{ width }}"
            height="{{ height }}"
            cornerRadius='{{ style == "Round" ? 1 : 0 }}'
            bgColor='{{ string.IsNullOrEmpty(bgColor) ? Util.TypeToColorBg(severity) : bgColor }}'
            data="{{ closable }}"
            hidden="{{ hidden }}"
    >
      <label
              class="text-primary"
              pos="{{ centerText ? width/2.0 : 0.4 }} {{ -height/2+0.2 }}"
              textcolor="{{ string.IsNullOrEmpty(textColor) ? Util.TypeToColorText(severity) : textColor }}"
              size="{{ width }} 5"
              text="{{ text }}"
              textsize="0.4"
              valign="center"
              halign="center"
      />

      <slot name="content" />
    </Panel>
  </template>
</component>
