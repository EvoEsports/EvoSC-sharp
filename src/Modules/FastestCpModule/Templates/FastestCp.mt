<component>
    <using namespace="EvoSC.Modules.Official.FastestCpModule"/>
    <using namespace="EvoSC.Modules.Official.FastestCpModule.Models"/>

    <property type="PlayerCpTime[]" name="times"/>

    <template>
        <Frame foreach="PlayerCpTime time in times" x="{{ -123 + 41 * (__index % 6) }}" y="{{ 89 - 3 * (__index / 6) }}" w="40" h="5">
            <Quad w="40" h="5" color="{{ Theme.FastestCpModule_FastestCP_Default_Bg }}" opacity="0.5" zIndex="-1"/>
            <Label x="2.5" y="-1" w="3" halign="center" textsize="0.75" text="{{ time.Cp + 1 }}" textfont="{{ Font.Regular }}" />
            <Quad x="5" w="0.25" h="5" color="{{ Theme.FastestCpModule_FastestCP_Default_Divider }}"/>
            <Frame x="7" y="-1">
                <Label textsize="0.75" x="10" halign="right" opacity="0.5" if="time.Time.Minutes == 0"
                       text='{{ 0 }}.{{ time.Time.Seconds }}.{{ time.Time.Milliseconds.ToString("000") }}'
                       zIndex="1"
                       textfont="{{ Font.Regular }}" />
              
                <Label textsize="0.75" x="10" halign="right" if="time.Time.Minutes == 0"
                       text='{{ time.Time.Seconds }}.{{ time.Time.Milliseconds.ToString("000") }}'
                       zIndex="2"
                       textfont="{{ Font.Regular }}"/>

              
                <Label textsize="0.75" x="10" halign="right" if="time.Time.Minutes != 0"
                       text='{{ time.Time.Minutes }}.{{ time.Time.Seconds.ToString("00") }}.{{ time.Time.Milliseconds.ToString("000") }}'
                       zIndex="2"
                       textfont="{{ Font.Regular }}"/>
              
                <Label textsize="0.75" x="14" text="{{ time.Player }}"
                       textfont="{{ Font.Regular }}"/>
            </Frame>
        </Frame>
    </template>
</component>