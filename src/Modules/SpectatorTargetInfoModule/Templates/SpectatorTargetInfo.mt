<component>
    <using namespace="EvoSC.Modules.Official.SpectatorTargetInfoModule.Config"/>

    <import component="EvoSC.Advanced.ClubTag" as="ClubTag"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="ISpectatorTargetInfoSettings" name="settings"/>
    <property type="int" name="timeDifference" default="0"/>
    <property type="int" name="playerRank" default="0"/>
    <property type="int" name="playerTeam" default="-1"/>
    <property type="string" name="playerName" default="unknown"/>
    <property type="string" name="teamColorCode" default="000"/>
    
    <property type="double" name="centerBoxWidth" default="48.0"/>
    <property type="double" name="h" default="7.0"/>

    <template>
        <UIStyle/>
        <frame id="main_frame" pos="{{ (h*4.4+centerBoxWidth) / -2.0 }} {{ settings.Y }}">
            <Rectangle width="{{ h*1.4 }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_AccentSecondary }}"
                       cornerRadius="0.5"
                       corners='TopLeft'
            />
            <quad pos="{{ h * 1.4 }}"
                  size="{{ h * 3.0 }} {{ h }}"
                  class="bg-primary"
                  opacity="1.0"
            />
            <Rectangle x="{{ h*4.4  }}"
                       width="{{ centerBoxWidth }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_BgPrimary }}ee"
                       cornerRadius="0.5"
                       corners='TopRight'
            />

            <label id="position_label"
                   class="text-primary"
                   pos="{{ (h*1.4) / 2.0 }} {{ h / -2.0 }}"
                   size="{{ h * 0.8 }} {{ h * 0.8 }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textcolor="{{ Theme.Black }}"
                   text="{{ playerRank }}"
                   halign="center"
                   valign="center2"
            />
            <label id="diff_label"
                   pos="{{ h*2.9 }} {{ h / -2.0 }}"
                   size="{{ (h*3.4)*0.8 }} {{ h }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textfont="{{ Font.Regular }}"
                   text='{{ timeDifference > 0 ? RaceTime.FromMilliseconds(timeDifference) : "000" }}'
                   textprefix="+"
                   halign="center"
                   valign="center2"
            />

            <frame id="name_box" pos="{{ h*4.4 + 2.0 }} {{ h / -2.0 }}">
<!--                <ClubTag h="{{ h / 2.0 }}"-->
<!--                         hidden="{{ true }}"-->
<!--                />-->
                <label id="name_label"
                       class="text-primary"
                       pos="{{ centerBoxWidth/2.0 }} 0"
                       size="{{ centerBoxWidth - 4.0 }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ playerName }}"
                       valign="center2"
                       halign="center"
                />
            </frame>

            <Rectangle y="{{ -h }}"
                       width="{{ h*4.4+centerBoxWidth }}"
                       height="0.75"
                       bgColor="{{ teamColorCode }}"
                       cornerRadius="0.5"
                       corners='BottomRight,BottomLeft'
            />
        </frame>
    </template>
</component>