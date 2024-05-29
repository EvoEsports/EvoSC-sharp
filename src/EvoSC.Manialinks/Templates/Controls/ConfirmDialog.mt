<component>
  <import component="EvoSC.Controls.Backdrop" as="Backdrop" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Drawing.Circle" as="Circle" />

  <!-- The width of the window's outer bounds. -->
  <property type="double" name="width" default="80" />

  <!-- The height of the window's outer bounds. -->
  <property type="double" name="height" default="45" />
  
  <property type="bool" name="okButton" default="true" />
  <property type="bool" name="cancelButton" default="true" />
  
  <property type="string" name="okButtonText" default="OK" />
  <property type="string" name="cancelButtonText" default="CANCEL" />
  
  <property type="string" name="title" />
  <property type="string" name="text" default="" />
  
  <property type="string" name="action" />
  <property type="bool" name="customBody" default="false" />
  
  <!-- info, success, warning, danger, primary, secondary -->
  <property type="string" name="severity" default="info" />
  
  <template>
    <Backdrop>
      <frame class="evosc-dialog"
             pos="{{ -width/2 }} {{ height/2 }}"
             id="{{ action }}"
      >
        <quad class="bg-primary" 
              pos="1 0"
              size="{{ width-1 }} {{ height-11 }}"
        />

        <quad class="bg-header"
              pos="1 {{ -height+11 }}"
              size="{{ width-1 }} 11"
        />
        
        <quad class="accent-primary"
              pos="0 0"
              size="1 {{ height }}"
        />
        
        <Circle x="{{ 1 + (width-11)/2 }}" y="-2" radius="5" bgColor="{{ Theme.UI_SurfaceBgPrimary}}" opacity="0.3"/>
        <label text="{{ Util.TypeToIcon(severity) }}"
               valign="center"
               halign="center"
               pos="{{ (width-1)/2 + 1 }} -6.6"
               class="text-primary"
               textsize="4"
               textcolor="{{ Util.TypeToColorBg(severity) }}"
        />
        
        <label class="text-primary"
               textfont="{{ Font.ExtraBold }}"
               text="{{ title.ToUpper() }}"
               halign="center"
               pos="{{ 1 + (width-1)/2 }} -15"
        />

        <!-- <quad bgcolor="000" pos="{{ 1 + (width-1)/2 }} -21" size="{{ width-1 }} {{ height-8-22 }}" halign="center" opacity="0.5" /> -->
        <label class="text-primary"
               textfont="{{ Font.Regular }}"
               text="{{ text }}"
               halign="center"
               pos="{{ 1 + (width-1)/2 }} -21"
               size="{{ width-1 }} {{ height-8-22 }}"
               autonewline="1"
               if="!customBody"
        />
        
        <slot name="body" />
        
        <IconButton id="btnCancel" 
                    text="{{ cancelButtonText }}"
                    width="{{ width/2-5 }}" 
                    x="3.5" 
                    y="{{ -height+8 }}" 
                    type="secondary" 
                    icon="{{ Icons.Times }}"
                    hasText="true" />
        
        <IconButton id="btnOk" 
                    text="{{ okButtonText }}" 
                    width="{{ width/2-5 }}" 
                    x="{{ width - (width/2-5) - 3.5 }}" 
                    y="{{ -height+8 }}" 
                    icon="{{ Icons.Check }}" 
                    hasText="true" />
      </frame>
    </Backdrop>
  </template>
</component>