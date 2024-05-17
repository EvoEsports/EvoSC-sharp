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
  
  <template>
    <UIStyle />
      <Window title="UI Demo" icon="" width="150" height="65" x="-60" y="30">
          <label text="BUTTONS:" class="text-primary" />
          <frame pos="0 -4">
              <Button id="btnDefault" text="Default" />
              <Button id="btnDefaultDisabled" text="Default Disabled" disabled="true" width="25" y="-6" />
              <Button id="btnSecondary" text="Secondary" type="secondary" y="-12" />
              <Button id="btnSecondaryDisabled" text="Secondary Disabled" type="secondary" disabled="true" width="25" y="-18" />
              <IconButton id="btnIconLeft" text="Icon Button" width="25" y="-24" icon="" />
              <IconButton id="btnIconRight" text="Icon Button" width="25" y="-30" icon="" iconPos="right" />
              <LinkButton id="btnLink" url="https://github.com/EvoEsports/EvoSC-sharp" text="Link Button" width="25" y="-36" />
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

          <label text="SWITCH:" class="text-primary" pos="0 -47" />
          <frame pos="15 -46">
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
        
        <frame pos="110 0">
          <Rating value="0" />
          <Rating y="-4" value="100" />
          <Rating y="-8" value="70" />
        </frame>
        
        <frame pos="30 -47">
          <label text="SEPARATOR:" class="text-primary" />
          <Separator x="19" length="5" y="-1.5" />
          <Separator x="25" length="5" y="0.5" direction="vertical" />
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
