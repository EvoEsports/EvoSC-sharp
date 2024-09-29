<component>
    <property type="string" name="id" />
    <property type="double" name="rowHeight"/>
    <property type="double" name="pointsWidth"/>
    <property type="double" name="padding"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="0.0"/>
    <property type="double" name="scale" default="1.0"/>
    <property type="int" name="zIndex" default="0"/>
    <property type="int" name="hidden" default="0"/>
    
    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}" scale="{{ scale }}" z-index="{{ zIndex }}" hidden="{{ hidden }}">
        </frame>
        
        <label id="points"
               pos="{{ x + (pointsWidth * scale) / 2.0 }} {{ rowHeight / -2.0 + 0.4 }}"
               text="x"
               valign="center"
               halign="center"
               textsize="2"
               textfont="{{ Font.Regular }}"
               z-index="11"
        />
    </template>
</component>