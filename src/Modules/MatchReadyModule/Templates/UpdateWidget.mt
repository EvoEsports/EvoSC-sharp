<component>
    |    <property type="int" name="playerCount" />
    <property type="bool" name="isReady" />

    <template>
    </template>

    <script><!--
        declare Integer EvoSC_ReadyWidget_PlayerCount for This = 0;
        declare Boolean EvoSC_ReadyWidget_IsReady for This = False;
        declare Boolean EvoSC_ReadyWidget_HasUpdate for This = True;
        
        EvoSC_ReadyWidget_PlayerCount = {{ playerCount }};
        EvoSC_ReadyWidget_HasUpdate = True;
        EvoSC_ReadyWidget_IsReady = {{ isReady }};
    --></script>
</component>
