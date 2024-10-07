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
    <property type="string" name="playerLogin" default="unknown"/>
    <property type="string" name="teamColorCode" default="000"/>

    <property type="double" name="centerBoxWidth" default="50.0"/>
    <property type="double" name="h" default="6.0"/>

    <template>
        <UIStyle/>
        <frame id="main_frame" pos="{{ (h*4.4+centerBoxWidth) / -2.0 }} {{ settings.Y }}">
            <Rectangle width="{{ h*1.6 }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_AccentSecondary }}cc"
                       cornerRadius="0.5"
                       corners='TopLeft'
            />
            <quad pos="{{ h * 1.6 }}"
                  size="{{ h * 3.0 }} {{ h }}"
                  bgcolor="{{ teamColorCode }}"
                  opacity="0.8"
            />
            <Rectangle x="{{ h*4.6  }}"
                       width="{{ centerBoxWidth }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_BgPrimary }}cc"
                       cornerRadius="0.5"
                       corners='TopRight'
            />

            <label id="position_label"
                   class="text-primary"
                   pos="{{ (h*1.6) / 2.0 }} {{ h / -2.0 }}"
                   size="{{ h * 0.8 }} {{ h * 0.8 }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textcolor="{{ Theme.Black }}"
                   text='{{ playerRank }}'
                   halign="center"
                   valign="center2"
            />
            <label id="diff_label"
                   pos="{{ h*1.6+h*1.5 }} {{ h / -2.0 }}"
                   size="{{ (h*3.4)*0.8 }} {{ h }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textfont="{{ Font.Regular }}"
                   text='{{ RaceTime.FromMilliseconds(timeDifference) }}'
                   textprefix="+"
                   halign="center"
                   valign="center2"
            />

            <frame id="name_box" pos="{{ h*4.6 }} {{ h / -2.0 }}">
                <label id="left_button"
                       class="text-primary"
                       pos="4 -0.25"
                       size="{{ h }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ Icons.ArrowCircleLeft }}"
                       valign="center2"
                       halign="center"
                       opacity="0.5"
                       scriptevents="1"
                       focusareacolor1="0000"
                       focusareacolor2="0000"
                />
                <label id="name_label"
                       class="text-primary"
                       pos="{{ centerBoxWidth/2.0 }} 0"
                       size="{{ centerBoxWidth - 14.0 }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ playerName }}"
                       valign="center2"
                       halign="center"
                />
                <label id="right_button"
                       class="text-primary"
                       pos="{{ centerBoxWidth - 4 }} -0.25"
                       size="{{ h }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ Icons.ArrowCircleRight }}"
                       valign="center2"
                       halign="center"
                       opacity="0.5"
                       scriptevents="1"
                       focusareacolor1="0000"
                       focusareacolor2="0000"
                />
            </frame>

            <Rectangle y="{{ -h }}"
                       width="{{ h*4.6+centerBoxWidth }}"
                       height="0.75"
                       bgColor="{{ teamColorCode }}"
                       cornerRadius="0.5"
                       corners='BottomRight,BottomLeft'
            />
        </frame>
    </template>

    <script resource="SpectatorTargetInfoModule.Scripts.SpectatorTargetInfo" main="true" />
</component>