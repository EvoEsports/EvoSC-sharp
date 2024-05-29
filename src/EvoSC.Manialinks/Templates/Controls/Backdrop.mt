<component>
  <property type="string" name="color" default="00000099" />
    
    <template>
        <frame z-index="1000">
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