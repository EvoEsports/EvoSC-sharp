<component>
    <using namespace="EvoSC.Modules.Official.ConfigManialinkModule.Config"/>

    <property type="IConfigManialinkSettings" name="settings"/>

    <template>
        <label text="{{ settings.Greeting }} {{ settings.Answer }}"/>
    </template>
</component>
