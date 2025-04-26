<component>
    <template/>
    <script><!--
    Text ReportSpecTarget(Text targetLogin){
        TriggerPageAction("SpectatorTargetInfoManialinkController/ReportSpectatorTarget/" ^ targetLogin);
        return targetLogin;
    }
    
    main() {
        declare Text previousTarget = "";
        declare Text currentTarget = "";
    
        while(True){
            yield;
            sleep(100);
            
            if(GUIPlayer == Null || GUIPlayer.User == LocalUser){
                currentTarget = "";
            }else{
                currentTarget = GUIPlayer.User.Login;
            }
            
            if(currentTarget == previousTarget) continue;
            previousTarget = ReportSpecTarget(currentTarget);
        }
    }
    --></script>
</component>
