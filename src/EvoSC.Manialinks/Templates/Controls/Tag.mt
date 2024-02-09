<component>
  <import component="EvoSC.Controls.Panel" as="Panel" />
  
  <property type="string" name="id" default="evosc_tag" />
  <property type="string" name="text" default="" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="15" />
  <property type="string" name="style" default="Square" /> <!-- Styles: Round, Square -->
  <property type="string" name="closable" default="false" />
  <property type="bool" name="hidden" default="false" />
  <property type="string" name="severity" default="primary" /> <!-- primary, secondary, success, info, warning, danger -->
  <property type="string?" name="bgColor" default="null" />
  <property type="string?" name="textColor" default="null" />
  
  <template>
    <Panel
            x="{{ x }}"
            y="{{ y }}"
            id="{{ id }}"
            width="{{ width }}"
            height="3"
            cornerRadius='{{ style == "Round" ? 2.5 : 0 }}'
            bgColor="{{ bgColor == null ? Util.TypeToColorBg(severity) : bgColor }}"
            data="{{ closable }}"
            hidden="{{ hidden }}"
    >
      <label
              class="text"
              pos="0.4 -1.2"
              textcolor="{{ textColor == null ? Util.TypeToColorText(severity) : textColor }}"
              size="{{ width }} 5"
              text="{{ text }}"
              textsize="1"
              valign="center"
      />

      <slot name="content" />
    </Panel>
  </template>
</component>
