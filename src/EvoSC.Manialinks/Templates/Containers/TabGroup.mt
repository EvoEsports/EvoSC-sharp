<component>
  
  <property type="string" name="id" default="evosc_tabgroup" />
  <property type="int" name="selectedIndex" default="0" />
  <property type="double" name="width" />
  <property type="double" name="height" />
  
  <template>
    <frame id="{{ id }}-frame" size="{{ width }} {{ height }}">
      <quad bgcolor="{{ Theme.UI_SurfaceBgSecondary }}" 
            pos="0 -5"
            size="{{ width }} 0.5"
      />
      <slot />
    </frame>
  </template>

  <script resource="EvoSC.Scripts.TabGroup" once="true" />
</component>
