<component>
    <template/>
    <script><!--
    main() {
        sleep(750);
        declare abortAt = Now + 3000;
    
        while(GUIPlayer == Null){
            yield;
            if(Now > abortAt) return;
        }
        
        log("[Spec Info] Reporting focused player " ^ GUIPlayer.User.Name);
        TriggerPageAction("SpectatorTargetInfoManialinkController/SetSpectatorTarget/" ^ GUIPlayer.User.Login);
    }
    --></script>
</component>
