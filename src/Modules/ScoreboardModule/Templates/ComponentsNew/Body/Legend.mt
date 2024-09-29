<component>
    <property type="double" name="width" />
    <property type="double" name="height" />
    <property type="double" name="columnSpacing" default="2.0" />
    <property type="double" name="positionBoxWidth" default="9.6" />
    <property type="double" name="flagWidth" default="9.6" />
    <property type="double" name="clubTagWidth" default="9.6" />

    <template>
        <quad size="{{ width }} {{ height }}"
              bgcolor="{{ Theme.ScoreboardModule_Background_Legend_Color }}"
              opacity="{{ Theme.ScoreboardModule_Background_Legend_Opacity }}"
        />
        <label text="POS"
               class="text-primary"
               pos="{{ positionBoxWidth/2.0 }} {{ height / -2 + 0.25 }}"
               halign="center"
               valign="center"
               textsize="{{ Theme.UI_FontSize*0.2 }}"
               textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
               opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
        />
        <label text="NAT."
               class="text-primary"
               pos="{{ positionBoxWidth+columnSpacing+(flagWidth/2.0) }} {{ height / -2 + 0.25 }}"
               halign="center"
               valign="center"
               textsize="{{ Theme.UI_FontSize*0.2 }}"
               textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
               opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
        />
        <label text="CLUB"
               class="text-primary"
               pos="{{ positionBoxWidth+columnSpacing+flagWidth+columnSpacing+(clubTagWidth/2.0) }} {{ height / -2 + 0.25 }}"
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
        
        <label text="BEST TIME"
               class="text-primary"
               pos="{{ width-columnSpacing - 55.0 }} {{ height / -2 + 0.25 }}"
               valign="center"
               halign="right"
               textsize="{{ Theme.UI_FontSize*0.2 }}"
               textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
               opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
        />
        <label text="SCORE"
               class="text-primary"
               pos="{{ width-columnSpacing }} {{ height / -2 + 0.25 }}"
               valign="center" 
               halign="right"
               textsize="{{ Theme.UI_FontSize*0.2 }}"
               textcolor="{{ Theme.ScoreboardModule_Background_Legend_Text_Color }}"
               opacity="{{ Theme.ScoreboardModule_Background_Legend_Text_Opacity }}"
        />
    </template>
</component>
