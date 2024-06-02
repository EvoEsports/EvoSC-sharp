<component>
  <property type="string" name="color" default="00000099" />
  <property type="string" name="id" default="evosc-backdrop" />
    
    <template>
        <frame z-index="1000" id="{{ id }}">
          <!-- We set the size to 1000x1000 to account for weird resolutions -->
          <quad
                  bgcolor="{{ color }}"
                  pos="0 0"
                  size="1000 1000"
                  valign="center"
                  halign="center"
          />
          <slot />
        </frame>
    </template>
</component>