<component>
  <import component="EvoSC.Theme" as="Theme" />
  <import component="EvoSC.Window" as="Window" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="EvoSC.Controls.Text" as="Text" />
  <import component="EvoSC.Controls.Select" as="Select" />
  <import component="EvoSC.Controls.SelectItem" as="SelectItem" />
  
  <template>
    <Theme />
    
    <Window title="Maps" icon="{{ Icons.Map }}" width="200" height="130" x="-100" y="65">
      <frame pos="0 -10">
        <TextInput name="txtSearch" y="0" width="50" placeholder="Search ..." />
        <Checkbox id="chkAllMaps" text="All Maps" y="-8" isChecked="true" />
        <Separator width="50" y="-14" />
        <Checkbox id="chkFavorites" text="Favorites" y="-17" />
        <Checkbox id="chkNoFinish" text="No Finish" y="-21" />
        <Checkbox id="chkNoLocal" text="No Local" y="-25" />
        <Separator width="50" y="-31" />
        <Text text="Tags:" y="-35" />
        <Select y="-39" width="50" id="selectTags" options="3">
          <SelectItem text="Transitional" value="transitional" n="0" />
          <SelectItem text="Speed Drift" value="speeddrift" n="1" />
          <SelectItem text="Roller Coaster" value="rollercoater" n="2" selected="true" />
        </Select>
      </frame>
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
