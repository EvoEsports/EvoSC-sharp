<component>
    <using namespace="System.Linq"/>
    
    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="columnSpacing" default="2f"/>
    <property type="double" name="positionBoxWidth" default="9.6"/>
    <property type="double" name="flagWidth" default="9.6"/>
    <property type="double" name="clubTagWidth" default="9.6"/>

    <template>
        <quad size="{{ width }} {{ height }}"
              bgcolor="{{ Theme.ScoreboardModule_Background_Legend_Color }}"
              opacity="{{ Theme.ScoreboardModule_Background_Legend_Opacity }}"
        />
        <frame foreach="int legendId in Enumerable.Range(1, 2).ToList()" 
               id="legend{{ legendId }}"
               pos="{{ (width / 2f) * (legendId - 1) }}"
        >
            <label text="POS"
                   class="text-primary"
                   pos="{{ positionBoxWidth/2f }} {{ height / -2 + 0.25 }}"
                   halign="center"
                   valign="center"
                   textsize="{{ Theme.UI_FontSize*0.2 }}"
                   textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
                   opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
            />
            <label text="NAT."
                   class="text-primary"
                   pos="{{ positionBoxWidth+columnSpacing+(flagWidth/2f) }} {{ height / -2 + 0.25 }}"
                   halign="center"
                   valign="center"
                   textsize="{{ Theme.UI_FontSize*0.2 }}"
                   textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
                   opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
            />
            <label text="CLUB"
                   class="text-primary"
                   pos="{{ positionBoxWidth+columnSpacing+flagWidth+columnSpacing+(clubTagWidth/2f) }} {{ height / -2 + 0.25 }}"
                   halign="center"
                   valign="center"
                   textsize="{{ Theme.UI_FontSize*0.2 }}"
                   textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
                   opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
            />
            <label text="NAME"
                   class="text-primary"
                   pos="{{ positionBoxWidth+columnSpacing+flagWidth+columnSpacing+clubTagWidth+columnSpacing }} {{ height / -2 + 0.25 }}"
                   valign="center"
                   textsize="{{ Theme.UI_FontSize*0.2 }}"
                   textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
                   opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
            />
            <label id="legend_best_time"
                   text="BEST TIME"
                   class="text-primary"
                   pos="{{ width-columnSpacing - 55f }} {{ height / -2 + 0.25 }}"
                   valign="center"
                   halign="right"
                   textsize="{{ Theme.UI_FontSize*0.2 }}"
                   textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
                   opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
            />
            <label id="legend_score"
                   text="SCORE"
                   class="text-primary"
                   pos="{{ width-columnSpacing }} {{ height / -2 + 0.25 }}"
                   valign="center"
                   halign="right"
                   textsize="{{ Theme.UI_FontSize*0.2 }}"
                   textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
                   opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
            />
        </frame>
    </template>

    <script><!--
    Void UpdateLegend(Boolean isPointsBased) {
        declare legend1Frame <=> (Page.MainFrame.GetFirstChild("legend1") as CMlFrame);
        declare legend2Frame <=> (Page.MainFrame.GetFirstChild("legend2") as CMlFrame);
        declare CMlFrame[Integer] legends = [0 => legend1Frame, 1 => legend2Frame];
        declare maxIndex = 0;
        
        if(UseClans){
            maxIndex = 1;
            legend2Frame.Show();
        }else{
            legend2Frame.Hide();
        }
    
        for(legendId, 0, maxIndex){
            declare targetLegend <=> legends[legendId];
            declare bestTimeLabel <=> (targetLegend.GetFirstChild("legend_best_time") as CMlLabel);
            declare scoreLabel <=> (targetLegend.GetFirstChild("legend_score") as CMlLabel);
            
            if(UseClans){
                bestTimeLabel.Hide();
                scoreLabel.Value = "POINTS";
                scoreLabel.RelativePosition_V3.X = {{ (width / 2f)-columnSpacing }} * 1.0;
            }else if(isPointsBased){
                bestTimeLabel.Show();
                scoreLabel.Value = "POINTS";
                scoreLabel.RelativePosition_V3.X = {{ width-columnSpacing }} * 1.0;
            }else{
                bestTimeLabel.Hide();
                scoreLabel.Value = "BEST TIME";
                scoreLabel.RelativePosition_V3.X = {{ width-columnSpacing }} * 1.0;
            }
        }
    }
    --></script>
</component>
