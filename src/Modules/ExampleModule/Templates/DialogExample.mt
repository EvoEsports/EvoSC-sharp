<component>
    <import component="EvoSC.Controls.ConfirmDialog" as="ConfirmDialog" />
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    
    <template>
      <UIStyle />
      <ConfirmDialog action="ExampleManialinkController/DialogAction" 
                     title="you are about to delete a map" 
                     text="Are you sure you want to delete this map? This action cannot be undone."
                     severity="warning"
                     okButtonText="i understand, go for it"
                     width="100"
      />
    </template>
</component>
