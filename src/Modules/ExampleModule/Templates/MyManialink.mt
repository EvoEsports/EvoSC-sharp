<component>
    <using namespace="EvoSC.Modules.Official.ExampleModule" />

    <property type="MyObjectThings" name="MyObject"/>
    
    <template>
        <Label text="Message: {{ MyObject.Message }}!" />
    </template>
</component>