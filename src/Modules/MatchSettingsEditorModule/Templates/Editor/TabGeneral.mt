<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  
  <import component="EvoSC.Controls.Text" as="Text" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  
  <property type="IMatchSettings" name="matchSettings" />
  <property type="double" name="width" />
  
  <template>
    <frame pos="0 -1">
      <Text text="NAME" />
      <TextInput id="txtName" y="-3" width="{{ width }}" value="{{ matchSettings.Name }}" />

      <Text y="-10" text="SCRIPT NAME" />
      <TextInput id="txtScriptName" y="-13" width="{{ width }}" value="{{ matchSettings.GameInfos.ScriptName }}" />

      <Text y="-20" text="MAP SORTING INDEX" />
      <TextInput id="txtSortIndex" y="-23" width="{{ width }}" value="{{ matchSettings.Filter.SortIndex }}" />
      
      <Checkbox id="chkRandomMapOrder" y="-30" text="Random map order" isChecked="{{ matchSettings.Filter.RandomMapOrder }}" />
      
    </frame>
  </template>
</component>