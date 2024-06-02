<!--
    A toggleable switch control that is either on or off.
-->
<component>
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
    <import component="EvoSC.Controls.Panel" as="Panel" />

    <!-- The ID of the switch. -->
    <property type="string" name="id" />
    
    <!-- The X position of the switch. -->
    <property type="double" name="x" default="0.0"/>
    
    <!-- The Y position of the switch. -->
    <property type="double" name="y" default="0.0"/>
    
    <!-- The initial value of the switch. -->
    <property type="bool" name="value" default="false"/>
    
    <template>
        <frame 
                pos="{{ x }} {{ y }}" 
                size="10 5"
                scriptevents="1" 
                id="{{ id }}"
                class="evosc-toggleswitch-frame"
                data-value="{{ value }}"
        >
            <!-- <quad 
                    class='evosc-toggleswitch'
                    bgcolor="{{ Theme.UI_SurfaceBgPrimary }}"
                    size="10 5"
                    scriptevents="1"
                    data-id="{{ id }}"
            /> -->
          <Rectangle className='evosc-toggleswitch'
                     bgColor="{{ Theme.UI_SurfaceBgPrimary }}"
                     width="10"
                     height="5"
                     scriptEvents="true"
                     dataid="{{ id }}"
                     cornerRadius="0.5"
          />
          <Panel className='evosc-toggleswitch'
                 bgColor="{{ Theme.UI_SurfaceBgPrimary }}"
                 width="10"
                 height="5"
                 scriptEvents="true"
                 dataId="{{ id }}"
                 cornerRadius="0.5">
            <quad
                    bgcolor="{{ Theme.UI_ToggleSwitch_Default_BgSecondary }}"
                    pos="{{ value ? 5 : 0 }} 0"
                    size="5 5"
                    scriptevents="1"
                    class="evosc-toggleswitch"
                    data-id="{{ id }}"
                    id="{{ id }}-head"
            />
            <label
                    class='{{ value ? "toggleswitch-on-default" : "toggleswitch-off-default" }} evosc-toggleswitch'
                    text='{{ value ? Icons.Check : Icons.Close }}'
                    valign="center"
                    halign="center"
                    pos="{{ value ? 7.5 : 2.5 }} -2.1"
                    textsize="1.5"
                    scriptevents="1"
                    data-id="{{ id }}"
                    id="{{ id }}-icon"
            />
          </Panel>
            
          <HiddenEntry 
                  name="{{ id }}"
                  value="{{ value }}"
          />
        </frame>
    </template>

    <script resource="EvoSC.Scripts.Switch" once="true"/>
</component>
