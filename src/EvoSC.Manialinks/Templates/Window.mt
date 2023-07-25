<component>
    <property type="string" name="id" default="evosc-window" />
    <property type="double" name="x" default="-50"/>
    <property type="double" name="y" default="30"/>
    <property type="double" name="width" default="100" />
    <property type="double" name="height" default="60" />
    <property type="string" name="title" default="New Window"/>
    <property type="string" name="icon" default="⬜"/>
    <property type="string" name="style" default="default"/>
    <property type="bool" name="canClose" default="true" />
    <property type="bool" name="canMinimize" default="false" />

    <template>
        <frame 
                class="evosc-window"
                pos="{{ x }} {{ y }}" 
                size="{{ width }} {{ height }}" 
                id="{{ id }}"
        >
            <quad
                    class="window-bg-{{ style }}"
                    size="{{ width }} {{ height }}"
            />
            
            <quad 
                    class="window-header-{{ style }} evosc-window-header evosc-window"
                    size="{{ width }} {{ 5 }}"
            />

            <label
                    class="window-title-{{ style }} evosc-window"
                    valign="center"
                    text="{{ icon }}"
                    size="{{ width-1.5 }} 5"
                    pos="1.5 -2.2"
                    if='icon != ""'
            />
            
            <label 
                    class="window-title-{{ style }} evosc-window"
                    valign="center"
                    text="{{ title }}"
                    size='{{ width-1.5 }} 5'
                    pos='{{ icon != "" ? 5.5 : 1.5 }} -2.2'
            />
            
            <label
                    class="window-closebtn-{{ style }} evosc-window-closebtn evosc-window-ctrlbtn"
                    data-id="{{ id }}"
                    valign="center"
                    text="❌"
                    size="5 5"
                    pos="{{ width-4.5 }} -2.2"
                    scriptevents="1"
                    if="canClose"
            />

            <label
                    class="window-minimizebtn-{{ style }} evosc-window-minimizebtn evosc-window-ctrlbtn"
                    data-id="{{ id }}"
                    valign="center"
                    text="—"
                    size="5 5"
                    pos="{{ width - 4.5 - (canClose ? 4 : 0) }} -2.2"
                    scriptevents="1"
                    if="canMinimize"
            />

            <frame pos="1 -6" size="{{ width-2 }} {{ height-7 }}">
                <slot />
            </frame>
        </frame>
    </template>

    <script resource="EvoSC.Scripts.Window" once="true" />
</component>
