<component>
    <using namespace="EvoSC.Modules.Official.FastestCp"/>
    <using namespace="EvoSC.Modules.Official.FastestCp.Models"/>

    <property type="PlayerCpTime?[]" name="times"/>

    <template>
        <Frame foreach="PlayerCpTime? time in times" x="{{ -123 + 41 * __index }}" y="89" w="40" h="5">
            <Quad w="40" h="5" color="000" opacity="0.5" zIndex="-1"/>
            <Label x="2" y="-1" textsize="0.75" text="{{ __index + 1 }}"/>
            <Quad x="5" w="0.25" h="5" color="41b562"/>
            <frame pos="7 -1" if="time != null">
                <Label textsize="0.75" x="10" halign="right" opacity="0.5" if="time.Time.Minutes == 0"
                       text="{{ 0 }}.{{ time.Time.Seconds }}.{{ time.Time.Milliseconds.ToString(&quot;000&quot;) }}"
                       zIndex="1"/>
                <Label textsize="0.75" x="10" halign="right" if="time.Time.Minutes == 0"
                       text="{{ time.Time.Seconds }}.{{ time.Time.Milliseconds.ToString(&quot;000&quot;) }}"
                       zIndex="2"/>

                <Label textsize="0.75" x="10" halign="right" if="time.Time.Minutes != 0"
                       text="{{ time.Time.Minutes }}.{{ time.Time.Seconds.ToString(&quot;00&quot;) }}.{{ time.Time.Milliseconds.ToString(&quot;000&quot;) }}"
                       zIndex="2"/>
                <Label textsize="0.75" x="14" text="{{ time.Player }}"/>
            </frame>
        </Frame>
    </template>
</component>