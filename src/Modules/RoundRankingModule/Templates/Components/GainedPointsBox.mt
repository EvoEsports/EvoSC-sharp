<component>
    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="int" name="points" default="0"/>

    <template>
        <frame if="points > 0"
               pos="{{ x }} {{ y }}"
        >
            <quad size="{{ height }} {{ height }}"
                  bgcolor="{{ Theme.UI_AccentPrimary }}"
                  opacity="0.9"
            />
            <label class="text-primary"
                   pos="{{ width/2.0 }} {{ height/-2.0 }}"
                   text="{{ points }}"
                   textsize="{{ Theme.UI_FontSize*0.8 }}"
                   textprefix="+"
                   valign="center"
                   halign="center"
            />
        </frame>
    </template>
</component>