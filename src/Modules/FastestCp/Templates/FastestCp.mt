<component>
    <using namespace="EvoSC.Modules.Official.FastestCp"/>
    <using namespace="EvoSC.Modules.Official.FastestCp.Models"/>

    <property type="PlayerCpTime[]" name="data"/>

    <template>
        <Label foreach="var time in data" text="{{ time.Player.StrippedNickName }} {{ time.RaceTime }}"
               x="{{ -120 + 30 * __index }}" y="85"/>
    </template>
</component>