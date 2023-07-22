<component>
    <import component="EvoSC.Theme" as="Theme" />

    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="bool" name="value" default="false"/>
    <property type="string" name="id" />
    
    <template>
        <Theme />
        <frame pos="{{ x }} {{ y }}" size="10 5" scriptevents="1" id="{{ id }}">
            <quad 
                    class='{{ value ? "toggleswitch-on-default" : "toggleswitch-off-default" }}'
                    size="10 5"
                    id="{{ id }}-bg"
                    scriptevents="1"
            />
            <quad 
                    bgcolor="FAE1EA"
                    pos="{{ value ? 5 : 0 }} 0"
                    size="5 5"
                    id="{{ id }}-head"
                    scriptevents="1"
            />
            <label
                    class='{{ value ? "toggleswitch-on-default" : "toggleswitch-off-default" }}'
                    text='{{ value ? "" : "" }}'
                    valign="center"
                    halign="center"
                    pos="{{ value ? 7.5 : 2.5 }} -2.1"
                    textsize="1.5"
                    id="{{ id }}-icon"
                    scriptevents="1"
            />
            <entry 
                    default="{{ value }}"
                    pos="100 100"
                    name="{{ id }}"
                    id="{{ id }}-entry"
            />
        </frame>
    </template>

    <script resource="EvoSC.Scripts.SwitchMethods" once="true" />
    <script resource="EvoSC.Scripts.Switch" />
</component>
