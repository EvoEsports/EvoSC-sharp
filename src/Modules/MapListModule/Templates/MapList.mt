<component>
  <import component="EvoSC.Theme" as="Theme" />
  <import component="EvoSC.Window" as="Window" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  
  <template>
    <Theme />
    
    <Window title="Maps" icon="{{ Icons.Map }}" width="200" height="130" x="-100" y="65">
      <TextInput name="txtSearch" y="-10" />
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
