<component>
    <using namespace="EvoSC.Modules.Official.SpectatorCamModeModule.Config"/>
    <using namespace="EvoSC.Common.Util.Manialinks"/>

    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="ISpectatorCamModeSettings" name="settings"/>
    <property type="double" name="width" default="32.0"/>
    <property type="double" name="height" default="5.5"/>
    <property type="double" name="opacityUnfocused" default="0.001"/>

    <template>
        <UIStyle/>

        <framemodel id="option">
            <quad id="cam_mode_bg"
                  size="{{ width }} {{ height }}"
                  bgcolor="{{ Theme.UI_SurfaceBgPrimary }}"
                  opacity="{{ opacityUnfocused }}"
                  scriptevents="1"
            />
            <label id="cam_mode_label"
                   class="text-primary"
                   pos="2 {{ height / -2.0 - 0.25 }}"
                   textsize="{{ Theme.UI_FontSize*1.5 }}"
                   textfont="{{ Font.Bold }}"
                   text="CAM MODE"
                   valign="center2"
                   halign="right"
            />
            <quad id="cam_mode_icon"
                  size="3.5 3.5"
                  pos="{{ width - 6.0 }} {{ height / -2.0 }}"
                  valign="center2"
                  colorize="{{ Theme.UI_TextPrimary }}"
            />
        </framemodel>

        <frame id="main_frame" pos="{{ settings.X }} {{ settings.Y }}" hidden="1">
            <frameinstance id="cam_mode_frame" modelid="option"/>
            <frame id="modes_wrapper" 
                   pos="0 {{ height * 3.0 }}"
                   size="{{ width }} {{ height * 3.0 }}"
                   hidden="1"
            >
                <frameinstance modelid="option" size="{{ width }} {{ height }}"/>
                <frameinstance modelid="option" size="{{ width }} {{ height }}"/>
                <frameinstance modelid="option" size="{{ width }} {{ height }}"/>
                <frameinstance modelid="option" size="{{ width }} {{ height }}"/>
            </frame>
        </frame>
    </template>

    <script resource="SpectatorCamModeModule.Scripts.SpectatorMode" main="true" />
</component>
