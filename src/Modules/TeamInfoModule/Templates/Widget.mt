<component>
    <using namespace="GbxRemoteNet.Structs" />
    
    <property type="TmTeamInfo" name="team1" />
    <property type="TmTeamInfo" name="team2" />
    
    <template>
        <UIStyle/>
        
        <frame>
            <label textsize="1" text="{{ team1.Name }}" />
            <label textsize="1" text="{{ team2.Name }}" pos="0 -5"/>
        </frame>
    </template>
</component>
