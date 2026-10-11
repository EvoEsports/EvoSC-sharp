<!--
    Lists every loaded module with controls to enable, disable, reload, unload or uninstall it.
-->
<component>
    <using namespace="System.Linq"/>
    <using namespace="System.Collections.Generic"/>
    <using namespace="EvoSC.Modules.Interfaces"/>

    <import component="EvoSC.Containers.Window" as="Window"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Containers.Container" as="Container"/>
    <import component="ModuleManagerModule.Components.ModuleRow" as="ModuleRow"/>

    <property type="IEnumerable<IModuleLoadContext>" name="modules"/>
    <property type="dynamic" name="Locale"/>

    <template>
        <UIStyle/>

        <Window
                width="120"
                height="{{ Math.Min(modules.Count(), 13) * 6 + 10 }}"
                x="-60"
                y="55"
                title="{{ Locale.ModuleManagerWindowTitle }}"
                icon="{{ Icons.PuzzlePiece }}"
        >
            <Container
                    id="moduleList"
                    width="118"
                    height="{{ Math.Min(modules.Count(), 13) * 6 }}"
                    scrollable="true"
                    scrollHeight="{{ (modules.Count() - 13) * 6 }}"
            >
                <ModuleRow
                        foreach="IModuleLoadContext module in modules"
                        module="{{ module }}"
                        y="{{ __index * -6 }}"
                        width="116"
                        Locale="{{ Locale }}"
                />
            </Container>
        </Window>
    </template>

    <script resource="EvoSC.Scripts.UIScripts" main="true"/>
</component>
