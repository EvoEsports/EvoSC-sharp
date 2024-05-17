<component>
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Controls.LinkButton" as="LinkButton" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.Switch" as="Switch" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Controls.RadioButton" as="RadioButton" />
  <import component="EvoSC.Controls.Alert" as="Alert" />
  <import component="EvoSC.Controls.Tag" as="Tag" />
  <import component="EvoSC.Controls.Chip" as="Chip" />
  <import component="EvoSC.Controls.Rating" as="Rating" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="EvoSC.Window" as="Window" />
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  <import component="EvoSC.Drawing.Circle" as="Circle" />
  
  <template>
    <UIStyle />
    <Window title="UI Demo" icon="" width="160" height="95" x="-80" y="47.5">
        <label text="BUTTONS:" class="text-primary" />
        <frame pos="0 -4">
            <Button id="btnDefault" text="Default" />
            <Button id="btnRound" text="Round" y="-6" style="round" />
            <Button id="btnDisabled" text="Disabled" disabled="true" y="-12" />
            <Button id="btnSecondary" text="Secondary" type="secondary" y="-18" width="20" />
            <IconButton id="btnIconLeft" text="Icon Button" width="25" y="-24" icon="" hasText="true" />
            <IconButton id="btnIconRight" text="Icon Button" width="25" y="-30" icon="" iconPos="right" hasText="true" />
            <IconButton id="btnIconSingle1" y="-36" icon="" />
            <IconButton id="btnIconSingle2" y="-36" x="6.5" icon="" type="secondary" />
            <IconButton id="btnIconSingle3" y="-36" x="13.5" icon="" style="round" />+
            <IconButton id="btnIconSingle4" y="-36" x="20" icon="" type="secondary" style="round" />
            <LinkButton id="btnLink" url="https://github.com/EvoEsports/EvoSC-sharp" text="Link Button" width="25" y="-42" />
        </frame>
        
        <label text="DROPDOWN:" class="text-primary" pos="30 -1" />
        <frame pos="57 0" z-index="10">
            <Dropdown text="Chose Something" id="dropdownDemo" width="30">
                <Button text="Action 1" id="dropdownBtn1" />
                <Button text="Action 2" id="dropdownBtn2" y="-5" />
                <Button text="Action 3" id="dropdownBtn3" y="-10" />
            </Dropdown>
        </frame>
        
        <frame pos="30 -6">
            <label text="TEXT INPUT:" class="text-primary" pos="0 -1.5" />
            <TextInput id="txtInputDemo" x="27" width="30" />
            <label text="PASSWORD INPUT:" class="text-primary" pos="0 -7" />
            <TextInput id="txtInputDemoPassword" isPassword="true" y="-6" x="27" width="30" />
        </frame>

        <label text="SWITCH:" class="text-primary" pos="0 -55" />
        <frame pos="15 -54">
            <Switch id="switchDemo" value="true" />
        </frame>

        <label text="CHECKBOX:" class="text-primary" pos="30 -20" />
        <frame pos="30 -24">
            <Checkbox id="checkbox1" text="one" isChecked="true" />
            <Checkbox id="checkbox2" text="two" isChecked="true" y="-6" />
            <Checkbox id="checkbox3" text="three" y="-12" />
        </frame>

        <label text="RADIO BUTTONS:" class="text-primary" pos="57 -20" />
        <frame pos="57 -24">
            <RadioButton id="radioButton1" text="one" isChecked="true" />
            <RadioButton id="radioButton2" text="two" y="-6" />
            <RadioButton id="radioButton3" text="three" y="-12" />
        </frame>
        
        <Button id="btnShowAlert" text="Show Alert" x="90" width="19" />
        <Button id="btnHideAlert" text="Hide Alert" x="90" y="-6" width="19" />
      
      <frame pos="90 -14">
        <Tag text="Tag" width="7"/>
        <Tag text="Tag" width="7" y="-4" severity="secondary" />
        <Tag text="Tag" width="7" y="-8" severity="success" />
        <Tag text="Tag" width="7" y="-12" severity="info" />
        <Tag text="Tag" width="7" y="-16" severity="warning" />
        <Tag text="Tag" width="7" y="-20" severity="danger" />
        <Tag text="Tag" style="Round" x="12" width="7"/>
        <Tag text="Tag" style="Round" x="12" width="7" y="-4" severity="secondary" />
        <Tag text="Tag" style="Round" x="12" width="7" y="-8" severity="success" />
        <Tag text="Tag" style="Round" x="12" width="7" y="-12" severity="info" />
        <Tag text="Tag" style="Round" x="12" width="7" y="-16" severity="warning" />
        <Tag text="Tag" style="Round" x="12" width="7" y="-20" severity="danger" />
      </frame>

      <frame pos="90 -39">
        <Chip id="chip1" text="Chip" closable="true" width="9"/>
        <Chip id="chip2" text="Chip" closable="true" style="Round" x="10" width="9" severity="secondary" />
      </frame>
      
      <frame pos="111 0">
        <Rating value="0" />
        <Rating y="-4" value="100" />
        <Rating y="-8" value="67" />
      </frame>
      
      <frame pos="30 -47">
        <label text="SEPARATOR:" class="text-primary" />
        <Separator x="19" length="5" y="-1.5" />
        <Separator x="25" length="5" y="0.5" direction="vertical" />
      </frame>
      
      <frame pos="132">
        <Circle radius="2" bgColor="{{ Theme.Red }}" /><label pos="5 -0.5" class="text-primary" text="RED" />
        <Circle radius="2" bgColor="{{ Theme.Green }}" y="-5" /><label pos="5 -5.5" class="text-primary" text="GREEN" />
        <Circle radius="2" bgColor="{{ Theme.Blue }}" y="-10" /><label pos="5 -10.5" class="text-primary" text="BLUE" />
        <Circle radius="2" bgColor="{{ Theme.Pink }}" y="-15" /><label pos="5 -15.5" class="text-primary" text="PINK" />
        <Circle radius="2" bgColor="{{ Theme.Gray }}" y="-20" /><label pos="5 -20.5" class="text-primary" text="GRAY" />
        <Circle radius="2" bgColor="{{ Theme.Orange }}" y="-25" /><label pos="5 -25.5" class="text-primary" text="ORANGE" />
        <Circle radius="2" bgColor="{{ Theme.Yellow }}" y="-30" /><label pos="5 -30.5" class="text-primary" text="YELLOW" />
        <Circle radius="2" bgColor="{{ Theme.Teal }}" y="-35" /><label pos="5 -35.5" class="text-primary" text="TEAL" />
        <Circle radius="2" bgColor="{{ Theme.Purple }}" y="-40" /><label pos="5 -40.5" class="text-primary" text="PURPLE" />
        <Circle radius="2" bgColor="{{ Theme.Gold }}" y="-45" /><label pos="5 -45.5" class="text-primary" text="GOLD" />
        <Circle radius="2" bgColor="{{ Theme.Silver }}" y="-50" /><label pos="5 -50.5" class="text-primary" text="SILVER" />
        <Circle radius="2" bgColor="{{ Theme.Bronze }}" y="-55" /><label pos="5 -55.5" class="text-primary" text="BRONZE" />
        <Circle radius="2" bgColor="{{ Theme.Grass }}" y="-60" /><label pos="5 -60.5" class="text-primary" text="GRASS" />
        <Circle radius="2" bgColor="{{ Theme.Dirt }}" y="-65" /><label pos="5 -65.5" class="text-primary" text="DIRT" />
        <Circle radius="2" bgColor="{{ Theme.Tarmac }}" y="-70" /><label pos="5 -70.5" class="text-primary" text="TARMAC" />
        <Circle radius="2" bgColor="{{ Theme.Ice }}" y="-75" /><label pos="5 -75.5" class="text-primary" text="ICE" />
      </frame>
    </Window>
      
    <Alert text="Hello there!" id="myAlert" y="70" type="primary" />
  </template>
  
  <script>
      *** OnMouseClick ***
      ***
          if (Event.Control.ControlId == "btnShowAlert") {
              ShowAlert("myAlert");
          } else if (Event.Control.ControlId == "btnHideAlert") {
              HideAlert("myAlert");
          }
      ***
  </script>
  
  <script resource="EvoSC.Scripts.UIScripts" />
</component>
