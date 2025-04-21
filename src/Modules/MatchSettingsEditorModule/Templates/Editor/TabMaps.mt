<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.ScrollBar" as="ScrollBar" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />

  <import component="MatchSettingsEditorModule.Editor.MapRow" as="MapRow" />

  <property type="IMatchSettings" name="matchSettings" />
  <property type="double" name="width" />
  <property type="double" name="height" />
  
  <template>
    <Container scrollable="true"
               scrollHeight="{{ matchSettings.Maps.Count()*12 - height + 6 }}"
               width="{{ width - 3 }}"
               height="{{ height - 2 }}"
               y="-2"
               id="mapsListContainer"
    >
      <MapRow width="{{ width - 3 }}" y="{{ -__index*12 }}" map="{{ map }}" foreach="IMap map in matchSettings.Maps" />
    </Container>

    <ScrollBar id="scrollMapsList"
               forFrame="mapsListContainer"
               y="-2"
               x="{{ width - 2 }}"
               max="{{ matchSettings.Maps.Count()*12 - height + 6 }}"
               length="{{ height - 8 }}"
    />
  </template>
</component>