<component>
    <template/>
    <script><!--
    main() {
        if(GUIPlayer == Null){
            log("[Spec Info] No target player.");
            return;
        }
        
        if(GUIPlayer.User == LocalUser){
            log("[Spec Info] Target player is local user.");
            return;
        }
    
        log("[Spec Info] Sending focused player " ^ GUIPlayer.User.Name);
        TriggerPageAction("SpectatorTargetInfoManialinkController/SetSpectatorTarget/" ^ GUIPlayer.User.Login);
    }
    --></script>
</component>
