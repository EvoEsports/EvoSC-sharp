<component>
  <property type="string" name="text" />
  <property type="string" name="value" />
  <property type="int" name="n" />
  <property type="double" name="width" default="25" />
  <property type="bool" name="selected" default="false" />
  
  <template>
    <frame pos="0 {{ -n*5.0 }}">
      <quad 
              size="{{ width }} 5"
              bgcolor="{{ Theme.UI_SelectItem_Default_Bg }}"
              focusareacolor1="00000000"
              focusareacolor2="ffffff22"
              scriptevents="1"
      />
      <label text="{{ Icons.Check }}" class="text" pos="0 -2.5" valign="center" if="selected" />
      <label text="{{ text }}" class="text" valign="center" pos="5 -2.5"/>
    </frame>
  </template>
</component>
