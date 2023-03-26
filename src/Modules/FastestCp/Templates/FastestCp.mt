<component>
    <using namespace="EvoSC.Modules.Official.FastestCp"/>
    <using namespace="EvoSC.Modules.Official.FastestCp.Models"/>

    <property type="PlayerCpTime?[]" name="data"/>

    <template>
        <Frame foreach="var time in data" x="{{ -120 + 30 * __index }}" y="85">
            <Label if="time != null" text="{{ time.Player.StrippedNickName }} {{ time.RaceTime }}"/>
        </Frame>
    </template>
</component>