<component>
    <property type="string" name="accountId" />
    <property type="int" name="cpIndex" />
    <property type="int" name="time" />
    
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
        
        EvoCheckpointTimes["{{ accountId }}"] = EvoSC_CheckpointTime{ AccountId = "{{ accountId }}", Time = {{ time }}, CpIndex = {{ cpIndex }} };
        EvoCheckpointTimesUpdate = Now;
        
        log("[SpecInfo] New cp time received: {{ accountId }} -> {{ time }}.");
    }
    --></script>
</component>