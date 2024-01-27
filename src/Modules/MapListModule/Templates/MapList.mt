<component>
  <import component="EvoSC.Theme" as="Theme" />
  <import component="EvoSC.Window" as="Window" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  
  <template>
    <Theme />
    
    <Window title="Maps" icon="{{ Icons.Map }}" width="200" height="130" x="-100" y="65">
      <TextInput name="txtSearch" y="-10" width="50" placeholder="Search ..." />
      <Checkbox id="chkAllMaps" text="All Maps" y="-18" isChecked="true" />
      <Separator width="50" y="-24" />
      <Checkbox id="chkFavorites" text="Favorites" y="-27" isChecked="true" />
      <Checkbox id="chkNoFinish" text="No Finish" y="-31" isChecked="true" />
      <Checkbox id="chkNoLocal" text="No Local" y="-35" isChecked="true" />
      <Separator width="50" y="-41" />
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
