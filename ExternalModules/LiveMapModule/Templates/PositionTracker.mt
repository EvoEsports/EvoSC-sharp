<component>
  <script>
    main() {
      while (True) {
        yield;
    
        if (GUIPlayer == Null) {
          continue;
        }
    
        // log("X: " ^ GUIPlayer.Position.X ^ ", Y: " ^ GUIPlayer.Position.Y ^ ", Z: " ^ GUIPlayer.Position.Z);
        TriggerPageAction("DataUpdateManialinkController/UpdatePosition/" ^ GUIPlayer.Position.X ^ "/" ^ GUIPlayer.Position.Y ^ "/" ^ GUIPlayer.Position.Z);
      }
    }
  </script>
</component>