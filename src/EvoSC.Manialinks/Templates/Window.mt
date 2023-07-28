<!--
General purpose window that can hold any type of component. The window is designed to only be used once per Manialink.
-->
<component>
    <!-- The ID of the window. Default is 'evosc-window' -->
    <property type="string" name="id" default="evosc-window" />
    
    <!-- The X position of the window. -->
    <property type="double" name="x" default="-50"/>
    
    <!-- The Y position of the window. -->
    <property type="double" name="y" default="30"/>
    
    <!-- The overall width of the window. -->
    <property type="double" name="width" default="100" />
    
    <!-- 
    The overall height of the window. Keep in mind that the titlebar is 
    exactly 5 units high and is included in the overall height.
     -->
    <property type="double" name="height" default="60" />
    
    <!-- The text to show in the titlebar. -->
    <property type="string" name="title" default="New Window"/>
    
    <!-- The icon to show in the titlebar. -->
    <property type="string" name="icon" default="⬜"/>
    
    <!-- The style of the window, can be default or secondary. -->
    <property type="string" name="style" default="default"/>
    
    <!-- Whether to show the close button. -->
    <property type="bool" name="canClose" default="true" />
    
    <!-- Whether to show the minimize button. -->
    <property type="bool" name="canMinimize" default="false" />
    
    <!-- Whether the user can drag the window around. -->
    <property type="bool" name="canMove" default="true" />
    
    <!-- Whether to display the titlebar or not. -->
    <property type="bool" name="hasTitlebar" default="true" />

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
                    if="hasTitlebar"
            />

            <label
                    class="window-title-{{ style }} evosc-window"
                    valign="center"
                    text="{{ icon }}"
                    size="{{ width-1.5 }} 5"
                    pos="1.5 -2.2"
                    if='hasTitlebar &amp;&amp; icon != ""'
            />
            
            <label 
                    class="window-title-{{ style }} evosc-window"
                    valign="center"
                    text="{{ title }}"
                    size='{{ width-1.5 }} 5'
                    pos='{{ icon != "" ? 5.5 : 1.5 }} -2.2'
                    if="hasTitlebar"
            />
            
            <label
                    class="window-closebtn-{{ style }} evosc-window-closebtn evosc-window-ctrlbtn"
                    data-id="{{ id }}"
                    valign="center"
                    text="❌"
                    size="5 5"
                    pos="{{ width-4.5 }} -2.2"
                    scriptevents="1"
                    if="hasTitlebar &amp;&amp; canClose"
            />

            <label
                    class="window-minimizebtn-{{ style }} evosc-window-minimizebtn evosc-window-ctrlbtn"
                    data-id="{{ id }}"
                    valign="center"
                    text="—"
                    size="5 5"
                    pos="{{ width - 4.5 - (canClose ? 4 : 0) }} -2.2"
                    scriptevents="1"
                    if="hasTitlebar &amp;&amp; canMinimize"
            />

            <frame pos="1 -{{ hasTitlebar ? 6 : 1 }}" size="{{ width-2 }} {{ height-(hasTitlebar ? 7 : 2) }}">
                <slot />
            </frame>
        </frame>
    </template>

    <script resource="EvoSC.Scripts.Window" once="true" />
</component>
