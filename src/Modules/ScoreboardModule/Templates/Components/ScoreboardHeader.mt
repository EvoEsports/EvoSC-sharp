<component>
    <property type="double" name="w" />
    <property type="double" name="headerHeight" />
    
    <template>
        <frame pos="1 {{ headerHeight / -2.0 }}">
        </frame>

        <frame pos="{{ w - 1.0 }} {{ headerHeight / -2.0 }}">
            <frame pos="0 2.5">
                <label id="round_label"
                       pos="-4 0"
                       valign="center"
                       halign="right"
                       textsize="2.2"
                       textcolor="{{ Theme.ScoreboardModule_ScoreboardHeader_Text }}"
                       textfont="{{ Font.Regular }}"/>
            </frame>

            <!-- Sub Text (Below highlighted box) -->
            <label id="sub_text"
                   pos="-8 -2.25"
                   textcolor="{{ Theme.ScoreboardModule_ScoreboardHeader_Text }}"
                   valign="center"
                   halign="right"
                   textsize="1.1"
                   textfont="{{ Font.Thin }}"
                   opacity="0.85"
            />

            <!-- Settings Icon -->
            <label id="settings_icon"
                   pos="-5.5 -2.45"
                   size="5 5"
                   textcolor="{{ Theme.ScoreboardModule_ScoreboardHeader_Text }}"
                   valign="center"
                   halign="center"
                   textsize="1.1"
                   text="{{ Icons.Cog }}"
                   textfont="{{ Font.Thin }}"
                   ScriptEvents="1"
                   focusareacolor1="0000"
                   focusareacolor2="0000"
                   opacity="0.65"
            />
        </frame>

        <quad if='Theme.ScoreboardModule_ScoreboardHeader_Logo  != ""'
              pos="{{ w / 2.0 }} 0"
              image="{{ Theme.ScoreboardModule_ScoreboardHeader_Logo }}"
              valign="center"
              halign="center"
              size="32 32"
              keepratio="Fit"
              z-index="50"
        />
    </template>
</component>