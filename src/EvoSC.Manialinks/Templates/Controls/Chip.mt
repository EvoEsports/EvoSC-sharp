<component>
  <import component="EvoSC.Controls.Panel" as="Panel" />
  
  <property type="string" name="id" default="evosc_chip" />
  <property type="string" name="text" default="" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="15" />
  <property type="string" name="style" default="Round" /> <!-- Styles: Round, Square -->
  <property type="string" name="bgColor" default="ffffff" />
  <property type="string" name="textColor" default="000000" />
  <property type="string" name="removable" default="false" />
  
  <template>
    <Panel
            id="{{ id }}"
            width="{{ width }}"
            height="5"
            cornerRadius='{{ style == "Round" ? 2.5 : 0 }}'
            bgColor="{{ bgColor }}"
    >
      <label 
              pos="0 0" 
              color="{{ textColor }}"
              size="{{ width }} 5"
              text="{{ text }}" 
              textsize="1" 
      />
    </Panel>
  </template>
</component>
