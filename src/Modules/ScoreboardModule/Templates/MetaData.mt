<component>
    <using namespace="GbxRemoteNet.Structs"/>
    
    <property type="int" name="roundNumber" />
    <property type="int" name="warmUpCount" />
    <property type="int" name="roundsPerMap" />
    <property type="int" name="pointsLimit" />
    <property type="bool" name="isWarmUp" />
    <property type="TmTeamInfo" name="team1" />
    <property type="TmTeamInfo" name="team2" />
    
    <template/>
    <script><!--
    #Include "ColorLib" as CL
        
    #Struct EvoSC_Team {
        Text Name;
        Vec3 Color;
    }
    
    main() {
        declare Integer EvoSC_RoundsPerMap for UI = -1;
        declare Integer EvoSC_RoundNumber for UI = -1;
        declare Integer EvoSC_WarmUpCount for UI = -1;
        declare Integer EvoSC_PointsLimit for UI = -1;
        declare Boolean EvoSC_WarmUpActive for UI = False;
        declare EvoSC_Team[Integer] EvoSC_Teams for UI = [];
        
        EvoSC_Teams[1] = EvoSC_Team{Name = "{{ team1.Name }}", Color = CL::HexToRgb("{{ team1.RGB }}")};
        EvoSC_Teams[2] = EvoSC_Team{Name = "{{ team2.Name }}", Color = CL::HexToRgb("{{ team2.RGB }}")};
        
        EvoSC_RoundsPerMap = {{ roundsPerMap }};
        EvoSC_RoundNumber = {{ roundNumber }};
        EvoSC_WarmUpCount = {{ warmUpCount }};
        EvoSC_PointsLimit = {{ pointsLimit }};
        EvoSC_WarmUpActive = {{ isWarmUp }};
    }
    --></script>
</component>