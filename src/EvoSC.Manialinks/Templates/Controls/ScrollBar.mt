<component>
  <property type="string" name="id" />
  <property type="string" name="forFrame" default="" />
  
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="double" name="min" default="0" />
  <property type="double" name="max" default="20" />
  <property type="double" name="value" default="0" />
  <property type="double" name="length" default="20" />
  
  <property type="int" name="zIndex" default="0" />
  
  <!-- Color of the slider handle. Uses the primary background color when empty. -->
  <property type="string" name="color" default="{{ Theme.UI_BgPrimary }}" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" id="{{ id }}">
      <quad
              size="2 5"
              bgcolor='{{ string.IsNullOrEmpty(color) ? Theme.UI_BgPrimary : color }}'
              scriptevents="1"
              id="scrollbar_quad_{{ id }}"
      />
    </frame>
  </template>
  
  <script resource="EvoSC.Scripts.ScrollBar.Instance" />
</component>
