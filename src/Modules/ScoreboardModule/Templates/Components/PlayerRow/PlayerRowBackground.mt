<component>
    <property type="string" name="id"/>
    <property type="double" name="rowHeight"/>
    <property type="double" name="padding"/>
    <property type="double" name="w"/>
    <property type="double" name="h"/>
    <property type="double" name="x"/>

    <template>
        <frame id="{{ id }}" 
               size="{{ w }} {{ h }}"
               pos="{{ x }}"
        >
            <quad id="background"
                  size="{{ w }} {{ h }}"
                  bgcolor="{{ Theme.ScoreboardModule_Background_Row_Color }}"
                  opacity="{{ Theme.ScoreboardModule_Background_Row_Opacity }}"
            />
        </frame>
    </template>

    <script once="true">
        <!--
        Void SetPlayerBackgroundColor(CMlFrame backgroundFrame, Vec3 color) {
            declare backgroundQuad = (backgroundFrame.Controls[0] as CMlQuad);
            backgroundQuad.BgColor = color;
        }
        
        Void ResetPlayerBackgroundColor(CMlFrame backgroundFrame){
            SetPlayerBackgroundColor(backgroundFrame, CL::HexToRgb("{{ Theme.ScoreboardModule_Background_Row_Color }}"));
        }
        
        Void SetPlayerHighlightColor(CMlFrame backgroundFrame, Vec3 color) {
        /*
            declare targetFrame = (backgroundFrame.Controls[1] as CMlFrame);
            (targetFrame.Controls[0] as CMlQuad).BgColor = color;
            
            Page.GetClassChildren("modulate", targetFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).ModulateColor = color;
            }
        */
        }
        -->
    </script>
</component>