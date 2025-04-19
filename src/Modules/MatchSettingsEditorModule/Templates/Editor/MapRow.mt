<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  
  <import component="EvoSC.Controls.Panel" as="Panel" />
  
  <property type="double" name="width" />
  <property type="double" name="y" />
  <property type="IMap" name="map" />
  
  <template>
    <frame pos="0 {{ y }}" size="{{ width }} 10">
      <Rectangle width="{{ width }}"
                 height="10"
                 bgColor="{{ Theme.UI_SurfaceBgPrimary }}"
                 cornerRadius="0.5"
      />

      <frame pos="7 0">
        <label text="{{ map.Name}}"
               class="text-lg text-primary"
               textcolor="{{ Theme.UI_TextPrimary }}"
               pos="2 -2"
        />

        <!-- <label text='{{ map.Author?.StrippedNickName ?? "<unknown author>" }}'
               class="text-xs"
               textcolor="{{ Theme.UI_TextMuted }}"
               pos="2 -6"
        /> -->
      </frame>
      
    </frame>
  </template>
</component>
