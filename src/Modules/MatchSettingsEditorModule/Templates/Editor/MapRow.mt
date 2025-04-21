<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  
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

      <label text="{{ Icons.Bars }}"
             class="text-xl"
             valign="center"
             pos="2 -5"
             textcolor="{{ Theme.UI_TextMuted }}"
      />
      
      <frame pos="5 0">
        <label text="{{ map.Name}}"
               class="text-lg text-primary"
               textcolor="{{ Theme.UI_TextPrimary }}"
               pos="2 -2"
        />

        <label text='{{ Icons.User }} {{ map.Author?.StrippedNickName ?? "<unknown author>" }}'
               class="text-xs"
               textcolor="{{ Theme.UI_TextMuted }}"
               pos="2 -6"
        />
      </frame>
      
      <frame pos="{{ width - 7 }} -2.5">
        <IconButton id="{{ map.Uid }}-btnRemove" icon="{{ Icons.Remove }}" />
      </frame>
      
    </frame>
  </template>
</component>
