<!--
Form for creating a group or editing the details of one.
-->
<component>
  <using namespace="EvoSC.Manialinks.Validation" />
  <using namespace="EvoSC.Modules.Official.GroupsModule.Models" />

  <import component="EvoSC.FormEntry" as="FormEntry" />
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />

  <property type="FormValidationResult" name="Validation" />
  <property type="int" name="groupId" />
  <property type="GroupEditModel" name="input" />

  <template>
    <UIStyle />

    <Window title='{{ groupId == 0 ? "New group" : "Edit group" }}'
            icon="{{ Icons.Users }}"
            width="70"
            height="90"
            x="-35"
            y="45"
    >
      <FormEntry
              validationResults='{{ Validation?.GetResult("Title") }}'
              name="Title"
              value="{{ input.Title }}"
              label="Title"
              w="64"
              x="0"
              y="0"
      />
      <FormEntry
              validationResults='{{ Validation?.GetResult("Description") }}'
              name="Description"
              value="{{ input.Description }}"
              label="Description"
              w="64"
              x="0"
              y="-13"
      />
      <FormEntry
              validationResults='{{ Validation?.GetResult("Icon") }}'
              name="Icon"
              value="{{ input.Icon }}"
              label="Icon (single character)"
              w="30"
              x="0"
              y="-26"
      />
      <FormEntry
              validationResults='{{ Validation?.GetResult("Color") }}'
              name="Color"
              value="{{ input.Color }}"
              label="Color (RGB, e.g. F00)"
              w="30"
              x="34"
              y="-26"
      />

      <Checkbox id="Unrestricted"
                text="Unrestricted (has all permissions)"
                isChecked="{{ input.Unrestricted }}"
                y="-39"
      />

      <frame pos="0 -48">
        <Button id="btnSave" text="Save" action="Groups/SaveGroup/{{ groupId }}" />
        <Button id="btnCancel" text="Cancel" type="secondary" x="20" action="Groups/ShowGroupList" />
      </frame>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
