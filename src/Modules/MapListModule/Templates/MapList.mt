<component>
  <import component="EvoSC.Theme" as="Theme" />
  <import component="EvoSC.Window" as="Window" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="EvoSC.Controls.Text" as="Text" />
  <import component="EvoSC.Controls.Select" as="Select" />
  <import component="EvoSC.Controls.SelectItem" as="SelectItem" />
  <import component="EvoSC.Controls.Container" as="Container" />
  <import component="MapListModule.Styles" as="MapListStyles" />
  
  <import component="MapListModule.MapListRow" as="MapListRow" />
  
  <template>
    <Theme />
    <MapListStyles />
    
    <Window title="Maps" icon="{{ Icons.Map }}" width="230" height="130" x="-100" y="65">
      <frame pos="0 -7">
        <TextInput name="txtSearch" y="0" width="34" placeholder="Search ..." />
        <Checkbox id="chkAllMaps" text="All Maps" y="-8" isChecked="true" />
        <Separator length="34" y="-14" />
        <Checkbox id="chkFavorites" text="Favorites" y="-17" />
        <Checkbox id="chkNoFinish" text="No Finish" y="-21" />
        <Checkbox id="chkNoLocal" text="No Local" y="-25" />
        <Separator length="34" y="-31" />
        <Text text="Tags:" y="-35" />
        <Select y="-39" width="34" id="selectTags" options="3">
          <SelectItem text="Transitional" value="transitional" n="0" />
          <SelectItem text="Speed Drift" value="speeddrift" n="1" />
          <SelectItem text="Roller Coaster" value="rollercoater" n="2" selected="true" />
        </Select>
      </frame>
      
      <Separator direction="vertical" length="106" x="36" y="-7" />
      
      <Container id="maplist"
                 x="38"
                 y="-7"
                 height="106"
                 width="188"
                 scrollable="true"
                 scrollHeight="{{ (20*12)-106-2 }}"
                 scrollGridY="12"
                 scrollGridSnap="true"
      >
        <MapListRow foreach="int j in Util.Range(20)" y="{{ -j*12 }}" key="{{ j }}" mapName="FMS - Bloom ft Aethal {{ j }}" />
        <!-- <MapListRow 
                mapName="$i$fffイ$5b0入$000»$fffŦlє$5b0ॅ$fffx$5b0N - $o$i$f00к$fffιиgѕ $f00и$fffєvєя $f00Đ$fffιє 2020"
                key="0"
                y="0"
        />

        <MapListRow
                mapName="Vert"
                key="1"
                y="-12"
        />
        
        <MapListRow
                mapName="$s$o$FF4E$DE4L$BD3E$AC3K$8B2T$6A2R$491I$491C$681 $781C$972I$A72T$C62Y $F64F$F65E$F55A$F56T$E57 $E47E$E48N$E48T$C48R$949Y$749L$44AA$24AG"
                key="2"
                y="-24"
        /> -->
      </Container>
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
