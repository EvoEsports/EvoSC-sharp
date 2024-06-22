<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <using namespace="System.Linq" />
    
    <import component="EvoSC.Controls.TextInput" as="TextInput"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="IEnumerable<EntryValidationResult>?" name="validationResults" />
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
            <Rectangle
                    y="-4.0"
                    x="1.0"
                    width="{{ height - 2.0 }}"
                    height="{{ height - 2.0 }}"
                    bgColor='{{ value ?? "fff" }}'
                    cornerRadius="0.5"
            />
            <TextInput
                    x="{{ height - 1.0 }}"
                    y="-3.0"
                    id="{{ name }}"
                    value='{{ value ?? "" }}'
                    width="{{ width - height }}"
                    prefix="#"
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
        </frame>
    </template>
</component>