<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <using namespace="EvoSC.Common.Interfaces.Localization" />
    <import component="EvoSC.FormEntry" as="FormEntry" />
    <!-- <import component="EvoSC.FormSubmit" as="FormSubmit" /> -->
    <import component="EvoSC.Window" as="Window" />
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    <import component="EvoSC.Controls.Button" as="Button" />

    <property type="FormValidationResult" name="Validation" />
    <property type="string" name="Nickname" />
    <property type="dynamic" name="Locale" />

    <template>
      <UIStyle />
      
      <Window
              width="50"
              height="23"
              x="-25"
              y="11.5"
              title="{{ Locale.PlayerLanguage.UI_EditYourNickname }}"
      >
        <FormEntry
                validationResults='{{ Validation?.GetResult("Nickname") }}'
                value='{{ Nickname }}'
                name="Nickname"
                label="{{ Locale.PlayerLanguage.UI_Nickname }}"
                w="48"
                x="0"
                y="0"
        />
        
        <Button id="btnEdit" text="{{ Locale.PlayerLanguage.UI_Submit }}" action="SetName/EditName" x="14" y="-8" />
        <Button id="btnCancel" text="{{ Locale.PlayerLanguage.UI_Cancel }}" action="SetName/Cancel" x="32" y="-8" type="secondary" />
      </Window>
    </template>

  <script resource="EvoSC.Scripts.UIScripts" />
</component>
