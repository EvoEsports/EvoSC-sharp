<component>
  <property type="string" name="id" default="evosc_container" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="0.0" />
  <property type="double" name="height" default="0.0" />
  <property type="bool" name="scrollable" default="false" />
  <property type="bool" name="scrollGridSnap" default="false" />
  <property type="double" name="scrollHeight" default="0.0" />
  <property type="double" name="scrollWidth" default="0.0" />
  <property type="double" name="scrollGridX" default="0.0" />
  <property type="double" name="scrollGridY" default="0.0" />
  <property type="double" name="zIndex" default="0" />
  <property type="double" name="rotate" default="0" />
  <property type="double" name="scale" default="1" />
  <property type="string" name="className" default="" />
  <property type="bool" name="hidden" default="false" />
  
  <template>
    <frame 
            id='{{ Util.DefaultOrRandomId("evosc_container", id) }}'
            pos="{{ x }} {{ y }}"
            size="{{ width }} {{ height }}"
            scriptevents="1"
            z-index="{{ zIndex }}"
            rot="{{ rotate }}"
            scale="{{ scale }}"
            class="{{ className }}"
            hidden='{{ hidden ? "1" : "0" }}'
    >
      <quad size="9999 9999" pos="0 0" if="scrollable" scriptevents="1" />
      <slot />
    </frame>
  </template>

  <script resource="EvoSC.Scripts.Container" />
</component>
