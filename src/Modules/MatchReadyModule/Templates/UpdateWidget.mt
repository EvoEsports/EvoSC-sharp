<component>
    <property type="int" name="PlayerCount" />
    <property type="bool" name="IsReady" />
    
    <template>
    </template>
    
    <script><!--
        declare Integer EvoSC_ReadyWidget_PlayerCount for This = 0;
        declare Boolean EvoSC_ReadyWidget_IsReady for This = False;
        declare Boolean EvoSC_ReadyWidget_HasUpdate for This = True;
        
        EvoSC_ReadyWidget_PlayerCount = {{ PlayerCount }};
        EvoSC_ReadyWidget_HasUpdate = True;
        EvoSC_ReadyWidget_IsReady = {{ IsReady }};
    --></script>
</component>
