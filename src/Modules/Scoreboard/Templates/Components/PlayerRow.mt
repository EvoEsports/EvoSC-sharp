<component>
    <property type="int" name="id" />
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    
    <template>
        <frame pos="{{ x }} {{ y }}">
            <label text="name {{ id }}" textsize="1" />
            <label text="score" pos="100" textsize="1" />
        </frame>
    </template>
</component>
