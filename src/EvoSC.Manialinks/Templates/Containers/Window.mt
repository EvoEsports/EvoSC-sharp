<!--
General purpose window that can hold any type of component.
The window is designed to only be used once per Manialink.
-->
<component>
    <import component="EvoSC.Containers.Container" as="Container" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  
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
          <Rectangle 
                  id="bg-header"
                  width="{{ width }}"
                  height="8"
                  bgColor="{{ Theme.UI_Window_Header_Bg }}"
                  corners="TopLeft,TopRight"
                  cornerRadius="0.75"
                  opacity="{{ Theme.UI_Window_Header_Bg_Opacity }}"
          />

          <quad pos="0 -8"
                size="{{ width }} 0.5"
                bgcolor="{{ Theme.UI_Window_Header_Separator }}"
                opacity="{{ Theme.UI_Window_Header_Separator_Opacity }}"
          />
          
          <label
                  class="window-title-icon"
                  valign="center"
                  halign="center"
                  text="{{ icon }}"
                  textcolor="{{ Theme.UI_Window_Header_Icon }}"
                  pos="4.2 -3.5"
                  textsize="2"
          />

          <label
                  class="text-primary"
                  textfont="{{ Font.Regular }}"
                  textsize="{{ Theme.UI_FontSize*1.75 }}"
                  valign="center"
                  textcolor="{{ Theme.UI_Window_Header_Title }}"
                  text="{{ title.ToUpper() }}"
                  pos="8 -3.5"
          />

          <IconButton className="evosc-window-closebtn"
                      id="btnClose-{{ id }}"
                      icon="{{ Icons.Close }}"
                      style="round"
                      type="secondary"
                      x="{{ width-6 }}"
                      y="-2"
                      data="{{ id }}"
                      size="small"
                      if="canClose"
          />
          
          <Rectangle
                  width="{{ width }}"
                  height="{{ height - 8.5 }}"
                  x="0"
                  y="-8.5"
                  bgColor="{{ Theme.UI_Window_Body_Bg }}"
                  corners="BottomLeft,BottomRight"
                  cornerRadius="0.75"
                  opacity="{{ Theme.UI_Window_Body_Bg_Opacity }}"
          />

          <Container
                  x="{{ padding }}"
                  y="-{{ 8.5+padding }}"
                  width="{{ width-padding*2 }}"
                  height="{{ height - (8.5+padding*2) }}"
                  scrollable="{{ scrollable }}"
          >
            <slot />
          </Container>
        </frame>
    </template>

    <script resource="EvoSC.Scripts.Window" once="true" />
</component>
