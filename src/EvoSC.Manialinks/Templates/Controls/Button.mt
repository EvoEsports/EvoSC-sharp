<component>
    <import component="EvoSC.Theme" as="Theme" />
    
    <property type="string" name="text" />
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    <property type="double" name="width" default="17.0" />
    <property type="double" name="height" default="5.0" />
    <property type="string" name="id" />
    <property type="string" name="type" default="default" />
    <property type="string" name="action" default="" />
    <property type="bool" name="disabled" default="false" />
    
    <template>
        <Theme />
        <frame pos="{{ x }} {{ y }}">
            <frame if="!disabled">
                <quad
                        class='{{ type == "secondary" ? "btn-secondary" : "btn-default" }}'
                        size="{{ width }} {{ height }}"
                        scriptevents="1"
                />
                <label
                        id="{{ id }}"
                        size="{{ width }} {{ height }}"
                        class='{{ type == "secondary" ? "btn-secondary" : "btn-default" }}'
                        text="{{ text }}"
                        scriptevents="1"
                        halign="center"
                        valign="center"
                        pos="{{ width/2 }} {{ -height/2 }}"
                        if='action.Equals("", StringComparison.Ordinal)'
                />
                <label
                        id="{{ id }}"
                        class='{{ type == "secondary" ? "btn-secondary" : "btn-default" }}'
                        size="{{ width }} {{ height }}"
                        text="{{ text }}"
                        scriptevents="1"
                        action="{{ action }}"
                        halign="center"
                        valign="center"
                        pos="{{ width/2 }} {{ -height/2 }}"
                        if='!action.Equals("", StringComparison.Ordinal)'
                />
            </frame>
            <frame if="disabled">
                <quad
                        size="{{ width }} {{ height }}"
                        class='{{ type == "secondary" ? "btn-secondary-disabled" : "btn-disabled" }}'
                />
                <label
                        id="{{ id }}"
                        class='{{ type == "secondary" ? "btn-secondary-disabled" : "btn-disabled" }}'
                        size="{{ width }} {{ height }}"
                        text="{{ text }}"
                        halign="center"
                        valign="center"
                        pos="{{ width/2 }} {{ -height/2 }}"
                />
            </frame>
        </frame>
    </template>
</component>
