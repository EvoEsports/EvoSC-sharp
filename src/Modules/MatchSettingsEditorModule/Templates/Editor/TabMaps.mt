<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />

  <import component="MatchSettingsEditorModule.Editor.MapRow" as="MapRow" />

  <property type="IMatchSettings" name="matchSettings" />
  <property type="double" name="width" />
  <property type="double" name="height" />
  
  <template>
    <Container>
      <label text="{{ matchSettings.Maps.Count() }}" />
    </Container>
  </template>
</component>