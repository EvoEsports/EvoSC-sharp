<component>
  <property type="string" name="text" />
  <property type="string" name="className" default="text" />
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="bool" name="bold" default="false" />
  
  <template>
    <label
            pos="{{ x }} {{ y }}"
            text='{{ bold ? "$o" : "" }} {{ text }}'
            class="{{ className }}"
    />
  </template>
</component>