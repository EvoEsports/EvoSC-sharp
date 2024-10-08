<component>
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    <import component="EvoSC.Controls.Button" as="Button" />

    <template>
        <UIStyle />
        <Button text="Contact Admins" id="ContactAdminButton" width="36.7" x="{{160-36.7-1.6}}" y="{{90-5-1.6 - 14}}" />
    </template>
    
    <script>
          <!--
              *** OnMouseClick ***
              ***
                  if (Event.Control.ControlId == "ContactAdminButton") {
                      TriggerPageAction("ContactAdminManialinkController/ContactAdminButton");
                  }
              ***      
          -->
        </script>
        
        <script resource="EvoSC.Scripts.UIScripts" />
</component>