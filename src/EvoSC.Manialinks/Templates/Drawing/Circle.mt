<component>
  <property type="string" name="id" default="evosc_rectangle" />
  <property type="double" name="radius" />
  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="bool" name="scriptEvents" default="false" />
  <property type="string" name="bgColor" default="00000000" />
  <property type="double" name="zIndex" default="0" />
  <property type="bool" name="hidden" default="false" />
  
  <property type="string" name="className" default="" />
  <property type="string" name="dataId" default="" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" 
           size="{{ radius*2 }} {{ radius*2 }}"
           id="{{ id }}"
           z-index="{{ zIndex }}"
           class="{{ className }}"
           hidden='{{ hidden ? "1" : "0" }}'
           scriptevents='{{ scriptEvents ? "1" : "0" }}'
           data-id="{{ dataId }}"
    >
        <quad 
                size="{{ radius*2 }} {{ radius*2 }}"
                pos="0 0"
                image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                modulatecolor="{{ bgColor }}"
                class="{{ className }}"
                
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
        />
    </frame>
  </template>
</component>
