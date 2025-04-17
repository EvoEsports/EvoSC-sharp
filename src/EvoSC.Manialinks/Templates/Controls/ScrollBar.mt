<component>
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  
  <property type="string" name="id" />
  <property type="string" name="forFrame" default="" />
  
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="double" name="min" default="0" />
  <property type="double" name="max" default="20" />
  <property type="double" name="value" default="0" />
  <property type="double" name="length" default="20" />
  
  <property type="int" name="zIndex" default="0" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" id="{{ id }}">
      <Rectangle id="scrollbar_quad_{{ id }}"
                 width="2"
                 height="5"
                 bgColor="{{ Theme.UI_SurfaceBgPrimary }}"
                 cornerRadius="1"
                 scriptEvents="true"
      />
    </frame>
  </template>
  
  <script resource="EvoSC.Scripts.ScrollBar.Instance" />
</component>
