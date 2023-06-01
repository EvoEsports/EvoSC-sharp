<component>
    <!--
        Manialink Form submit button.
    -->
    
    <using namespace="System.Linq" />
    
    <property type="string?" name="text" />
    <property type="string" name="action" />
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    
    <template>
        <label
                scriptevents="1"
                style="TextButtonMedium"
                text="{{ text }}"
                action="{{ action }}"
                pos="{{ x }} {{ y }}"
                textsize="1"
        />
    </template>
</component>
