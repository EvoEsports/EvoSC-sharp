<!--
Asks for confirmation before deleting a group.
-->
<component>
  <import component="EvoSC.Controls.ConfirmDialog" as="ConfirmDialog" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />

  <property type="int" name="groupId" />
  <property type="string" name="groupTitle" />

  <template>
    <UIStyle />

    <ConfirmDialog
            title="You are about to delete a group"
            text="Delete the group '{{ groupTitle }}'? Players in it lose the group and its permissions."
            severity="danger"
            action="Groups/ConfirmDeleteGroup/{{ groupId }}"
    />
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
