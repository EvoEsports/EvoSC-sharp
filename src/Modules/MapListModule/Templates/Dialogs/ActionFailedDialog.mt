<component>
  <import component="EvoSC.Controls.ConfirmDialog" as="ConfirmDialog" />

  <property type="string" name="title" />
  <property type="string" name="text" />

  <template>
    <ConfirmDialog
            title="{{ title }}"
            text="{{ text }}"
            cancelBtn="false"
            severity="danger"
    />
  </template>
</component>