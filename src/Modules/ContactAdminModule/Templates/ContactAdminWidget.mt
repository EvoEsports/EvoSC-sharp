<component>
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    <import component="EvoSC.Controls.Button" as="Button" />

    <template>
        <UIStyle />
        <Button text="Contact Admins" id="ContactAdminButton" width="69" />
    </template>
    
    <script>
          <!--
              *** OnMouseClick ***
              ***
                  if (Event.Control.ControlId == "ContactAdminButton") {
                      TriggerPageAction("ContactAdminManialinkController/ContactAdminButton/");
                  }
              ***      
          -->
        </script>
        
        <script resource="EvoSC.Scripts.UIScripts" />
</component>