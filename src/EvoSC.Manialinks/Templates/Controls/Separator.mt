<component>
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="double" name="width" default="10" />
  <property type="double" name="thickness" default="0.2" />
  <property type="string" name="direction" default="horizontal" /> <!-- horizontal or vertical -->
  
  <template>
    <quad 
            pos="{{ x }} {{ y }}" 
            size="{{ width }} {{ thickness }}" 
            bgcolor="{{ Theme.UI_Separator_Default_Bg }}"
            if='direction == "horizontal"'
    />

    <quad
            pos="{{ x }} {{ y }}"
            size="{{ thickness }} {{ width }}"
            bgcolor="{{ Theme.UI_Separator_Default_Bg }}"
            if='direction == "vertical"'
    />
  </template>
</component>
