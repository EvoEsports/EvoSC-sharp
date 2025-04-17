<component>
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  
  <property type="string" name="id" />
  <property type="string" name="title" />
  <property type="double" name="headerWidth" default="17" />
  
  <template>
    <frame class="evosc-tabpage-frame" id="{{ id }}">
      <frame class="evosc-tabpage-header-frame" id="{{ id }}-header-frame">
        <Rectangle width="{{ headerWidth }}"
                   height="5"
                   bgColor="{{ Theme.UI_SurfaceBgPrimary }}"
                   corners="TopLeft,TopRight"
                   cornerRadius="0.5"
                   id="{{ id }}-header-bg"
                   scriptEvents="true"
                   dataId="{{ id }}"
        />
        
        <label text="{{ title }}"
               textcolor="{{ Theme.UI_TextPrimary }}"
               class="text-lg text-primary"
               pos="{{ headerWidth/2f }} -2.5"
               valign="center"
               halign="center"
               id="{{ id }}-header-text"
        />
      </frame>

      <frame class="evosc-tabpage-body-frame" pos="0 -6" id="{{ id }}-body-frame" hidden="1">
        <slot />
      </frame>
    </frame>
  </template>
</component>