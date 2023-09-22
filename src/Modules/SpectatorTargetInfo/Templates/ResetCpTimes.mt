<component>
    <template>
    </template>
    
    <script><!--
    #Struct EvoSC_CheckpointTime {
        Text AccountId;
        Integer CpIndex;
        Integer Time;
    }
    
    main() {
        declare EvoSC_CheckpointTime[Text] EvoCheckpointTimes for UI;
        declare Integer EvoCheckpointTimesUpdate for UI;
        declare Boolean EvoCheckpointTimesReset for UI = False;
        
        EvoCheckpointTimes = EvoSC_CheckpointTime[Text];
        EvoCheckpointTimesUpdate = Now;
        EvoCheckpointTimesReset = True;
    }
    --></script>
</component>