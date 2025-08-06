<component>
    <using namespace="System.Linq"/>
    <using namespace="System.Collections.Generic"/>

    <import component="EvoSC.Containers.Window" as="Window"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Controls.Button" as="Button"/>
    <import component="EvoSC.Controls.Checkbox" as="Checkbox"/>
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    <import component="EvoSC.Containers.Container" as="Container" />

    <property type="List<string>" name="moduleNames"/>
    <property type="List<string>" name="hiddenModules"/>

    <template>
        <UIStyle/>

        <Window
                width="120"
                height="100.5"
                x="-60"
                y="55"
                title="UI Control"
                icon=""
        >
            <Container 
                    id="checkboxList"
                    width="120"
                    height="78"
                    scrollable="true"
                    scrollHeight="{{ (moduleNames.Count - 13) * 6 }}"
            >
                <Checkbox
                        id="checkbox_{{ __index }}"
                        foreach="string moduleName in moduleNames"
                        y="{{ __index * -6 }}"
                        isChecked='{{ hiddenModules.Contains(moduleName) }}'
                        text='{{ moduleName.Replace("Module", "").Replace(".", " / ") }}'
                />
            </Container>
            
            <HiddenEntry
                    id="hiddenManialinksEntry"
                    name="HiddenManialinks"
            />
            
            <frame pos="0 -81">
                <Button
                        id="btnSave"
                        text="Submit"
                        action="UiControlModule/SaveConfiguration"
                />
                <Button
                        id="btnHideAll"
                        text="Hide all"
                        action="UiControlModule/HideAll"
                        x="22"
                />
            </frame>
        </Window>
    </template>

    <script resource="UiControlModule.Scripts.Menu"/>
    <script resource="EvoSC.Scripts.UIScripts"/>
</component>
