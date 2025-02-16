<component>
    <property type="int" name="roundNumber" />
    <property type="int" name="warmUpCount" />
    <property type="int" name="roundsPerMap" />
    <property type="int" name="pointsLimit" />
    <property type="bool" name="isWarmUp" />
    
    <template/>
    <script><!--
    main() {
        declare Integer EvoSC_RoundsPerMap for UI = -1;
        declare Integer EvoSC_RoundNumber for UI = -1;
        declare Integer EvoSC_WarmUpCount for UI = -1;
        declare Integer EvoSC_PointsLimit for UI = -1;
        declare Boolean EvoSC_WarmUpActive for UI = False;
        
        EvoSC_RoundsPerMap = {{ roundsPerMap }};
        EvoSC_RoundNumber = {{ roundNumber }};
        EvoSC_WarmUpCount = {{ warmUpCount }};
        EvoSC_PointsLimit = {{ pointsLimit }};
        EvoSC_WarmUpActive = {{ isWarmUp }};
    }
    --></script>
</component>