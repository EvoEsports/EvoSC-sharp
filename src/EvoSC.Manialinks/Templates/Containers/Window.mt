<!--
General purpose window that can hold any type of component.
The window is designed to only be used once per Manialink.
-->
<component>
    <import component="EvoSC.Containers.Container" as="Container" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
  
    <!-- The ID of the window. Default is 'evosc-window' -->
    <property type="string" name="id" default="evosc-window" />
    
    <!-- The X position of the window. -->
    <property type="double" name="x" default="-50"/>
    
    <!-- The Y position of the window. -->
    <property type="double" name="y" default="30"/>
    
    <!-- The width of the window's outer bounds. -->
    <property type="double" name="width" default="100" />
    
    <!-- The height of the window's outer bounds. -->
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
  
    <!-- Padding from the window border to the window contents -->
    <property type="double" name="padding" default="3" />
  
    <!-- Whether the contents of the window can be scrolled -->
    <property type="bool" name="scrollable" default="false" />

    <template>
        <frame class="evosc-window"
               pos="{{ x }} {{ y }}" 
               size="{{ width+2 }} {{ height }}" 
               id="{{ id }}"
        >
          <!-- Header BG Icon -->
          <quad size="7 7" 
                pos="1 0"
                bgcolor="{{ Theme.UI_AccentSecondary }}" />

          <!-- Header BG Line -->
          <quad pos="1 -7" 
                size="{{ width }} 1" 
                bgcolor="{{ Theme.UI_AccentSecondary }}" />

          <!-- Header BG -->
          <quad class="bg-header" 
                size="{{ width-7 }} 7" 
                pos="8 0" />

          <!-- Header BG Accent Line Left -->
          <quad class="accent-primary"
                size="1 {{ height }}"
          />
          
          <!-- Header BG Accent Line Right -->
          <quad class="accent-primary"
                pos="{{ width+1 }} 0"
                size="1 {{ height }}"
          />

          <!-- Body BG -->
          <quad class="bg-primary"
                pos="1 -8"
                size="{{ width }} {{ height-8 }}"
          />

          <!-- Window Icon -->
          <label
                  class="window-title-icon"
                  valign="center"
                  halign="center"
                  text="{{ icon }}"
                  pos="4.5 -3"
          />

          <!-- Window Title Text -->
          <label
                  class="text-header"
                  valign="center"
                  text="{{ title.ToUpper() }}"
                  pos="10 -3.2"
          />
          
          <IconButton className="evosc-window-closebtn"
                      id="btnClose-{{ id }}"
                      dataId="{{ id }}"
                      icon="{{ Icons.Close }}" 
                      style="round"
                      type="accent"
                      x="{{ width-4.5 }}"
                      y="-1.6"
                      data="{{ id }}"
                      size="small"
          />

          <!-- Body Contents -->
          <Container
                  x="{{ padding+1 }}"
                  y="-{{ 8+padding }}"
                  width="{{ width-padding*2 }}"
                  height="{{ height - (8+padding*2) }}"
                  scrollable="{{ scrollable }}"
          >
            <slot />
          </Container>
          
          <!-- Window Header -->
            <!-- <quad
                    class="window-bg"
                    size="{{ width }} {{ height }}"
                    scriptevents="1"
            />
            
            <quad 
                    class="window-header evosc-window-header evosc-window"
                    size="{{ width }} {{ 5 }}"
                    if="hasTitlebar"
            />

            <label
                    class="window-title evosc-window"
                    valign="center"
                    text="{{ icon }}"
                    size="{{ width-1.5 }} 5"
                    pos="1.5 -2.2"
                    if='hasTitlebar &amp;&amp; icon != ""'
            />
            
            <label 
                    class="window-title-{{ style }} evosc-window"
                    valign="center"
                    text="{{ title.ToUpper() }}"
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

            <Container
                    x="{{ padding }}"
                    y="-{{ hasTitlebar ? 5+padding : padding }}"
                    width="{{ width-padding*2 }}"
                    height="{{ height-(hasTitlebar ? 5+padding*2 : padding*2) }}"
                    scrollable="false"
            >
                <slot />
            </Container> -->
        </frame>
    </template>

    <script resource="EvoSC.Scripts.Window" once="true" />
</component>
