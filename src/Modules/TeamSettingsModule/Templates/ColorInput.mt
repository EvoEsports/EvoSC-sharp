<component>
    <using namespace="EvoSC.Manialinks.Validation"/>
    <using namespace="System.Linq"/>

    <import component="EvoSC.Controls.TextInput" as="TextInput"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="IEnumerable<EntryValidationResult>?" name="validationResults"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="width" default="25.0"/>
    <property type="double" name="height" default="5.0"/>
    <property type="string" name="name" default="EvoSC_ColorInput"/>
    <property type="string" name="label" default=""/>
    <property type="string?" name="value" default="000"/>

    <template>
        <frame pos="{{ x }} {{ y }}">
            <Label
                    text="{{ label }}"
                    class="text-primary"
                    textsize="1"
                    halign="left"
                    valign="top"
            />

            <!-- Input BG -->
            <Rectangle
                    y="-3.0"
                    width="{{ width }}"
                    height="{{ height }}"
                    bgColor='{{ Theme.UI_SurfaceBgPrimary }}'
                    cornerRadius="0.5"
            />

            <!-- Input FG -->
            <quad id="rectangleColorPreview"
                  size="{{ height - 2.0 }} {{ height - 2.0 }}"
                  pos="4 -4.26"
                  rot="90"
                  modulatecolor='{{ value ?? "fff" }}'
                  image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                  scale="0.8"
            />
            <TextInput
                    x="{{ height - 1.0 }}"
                    y="-3.0"
                    id="{{ name }}"
                    value='{{ value ?? "" }}'
                    width="{{ width - height }}"
                    classes="colorInput"
                    prefix="#"
                    autoSelect="{{ true }}"
            />
            <label id="labelPipette"
                   pos="{{ width - 3.0 }} {{ height / -2.0 - 3.0 }}"
                   text="{{ Icons.Eyedropper }}"
                   textsize="0.75"
                   valign="center"
                   halign="center"
                   class="text-primary"
                   ScriptEvents="1"
            />

            <!-- Validation -->
            <Label
                    if='validationResults?.Any(v => v.IsInvalid) ?? false'
                    text='$s$e11 {{ validationResults?.FirstOrDefault(v => v.IsInvalid)?.Message ?? "Invalid input." }}'
                    class="text-primary"
                    x="0"
                    y="-10"
                    w="{{ width }}"
                    h="10"
                    autonewline="1"
                    textsize="0.75"
                    halign="left"
                    valign="top"
            />

            <!-- Color Picker -->
            <frame id="frameColorPicker"
                   pos="{{ width - 32 }} {{ -height - 4.0 }}"
                   size="32 22"
                   z-index="5"
                   hidden="1"
            >
                <Rectangle
                        width="32.0"
                        height="22.0"
                        bgColor='{{ Theme.UI_SurfaceBgPrimary }}'
                        cornerRadius="0.5"
                />
                <colorpicker
                        id="picker"
                        pos="2 -2"
                        size="{{ 28.0 }} {{ 17.0 }}"
                        Color="{{ value }}"
                        scriptevents="1"
                        halign="left"
                        valign="top"
                />
            </frame>
        </frame>
    </template>

    <script resource="TeamSettings.Scripts.ColorInput" once="true"/>
</component>