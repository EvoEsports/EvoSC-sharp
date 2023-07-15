<component>
    <import component="MotdModule.MotdWindow" as="MotdWindow" />
    
    <property type="string" name="test" />
    
    <template>
        <MotdWindow h="90" w="160" x="0" y="70" zIndex="1" title="Message of the Day" buttonAction="MotdManialinkController/Close">
            <label pos="-78 35" text="Hello!" halign="left" z-index="2"/>
        </MotdWindow>
    </template>
</component>


