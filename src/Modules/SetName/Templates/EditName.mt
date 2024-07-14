<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <using namespace="EvoSC.Common.Interfaces.Localization" />
    <import component="EvoSC.FormEntry" as="FormEntry" />
    <!-- <import component="EvoSC.FormSubmit" as="FormSubmit" /> -->
    <import component="EvoSC.Containers.Window" as="Window" />
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    <import component="EvoSC.Controls.Button" as="Button" />

    <property type="FormValidationResult" name="Validation" />
    <property type="string" name="Nickname" />
    <property type="dynamic" name="Locale" />

    <template>
      <UIStyle />
      
      <Window
              width="55"
              height="{{ (Validation != null && !Validation.IsValid) ? 40 : 30 }}"
              x="-25"
              y="11.5"
              title="{{ Locale.PlayerLanguage.UI_EditYourNickname }}"
              icon=""
      >
        <FormEntry
                validationResults='{{ Validation?.GetResult("Nickname") }}'
                value='{{ Nickname }}'
                name="Nickname"
                label="{{ Locale.PlayerLanguage.UI_Nickname }}"
                w="49"
                x="0"
                y="0"
        />
        
        <frame pos="0 {{ (Validation != null && !Validation.IsValid) ? -10 : 0 }}">
            <Button id="btnEdit" text="{{ Locale.PlayerLanguage.UI_Submit }}" action="SetName/EditName" x="0"  y="-11" />
            <Button id="btnCancel" text="{{ Locale.PlayerLanguage.UI_Cancel }}" action="SetName/Cancel" x="32" y="-11" type="secondary" />
        </frame>
      </Window>
    </template>

  <script resource="EvoSC.Scripts.UIScripts" />
</component>
