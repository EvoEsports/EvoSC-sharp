<component>
  <import component="EvoSC.Controls.Chip" as="Chip" />
  
  <property type="string" name="id" />
  <property type="int" name="options" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="25.0" />
  <property type="double" name="height" default="5.0" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" id="{{ id }}">
      <quad
              pos="0 0"
              size="{{ width }} {{ height }}"
              bgcolor="{{ Theme.UI_Select_Default_Border }}"
      />
      <quad
              class="evosc-select-trigger"
              pos="{{ 0.1 }} {{ -0.1 }}"
              size="{{ width-0.2 }} {{ height-0.2 }}"
              bgcolor="{{ Theme.UI_Select_Default_Bg }}"
              data-id="{{ id }}"
              scriptevents="1"
      />
      
      <!-- throws error -->
      <!-- <Chip foreach="int i in Util.Range(options)" /> -->
      
      <frame id="evosc-select-dpanel-{{ id }}"
             data-id="{{ id }}"
             class="evosc-select-dpanel"
             pos="0 {{ -height }}"
      >
        <slot />
      </frame>
    </frame>
  </template>

  <script resource="EvoSC.Scripts.Select" once="true" />
</component>
