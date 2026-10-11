<!--
    A single module's row in the module manager window: its identity and status, and the
    controls to change its lifecycle state.
-->
<component>
    <using namespace="EvoSC.Modules"/>
    <using namespace="EvoSC.Modules.Interfaces"/>

    <import component="EvoSC.Controls.IconButton" as="IconButton"/>

    <property type="IModuleLoadContext" name="module"/>
    <property type="dynamic" name="Locale"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="width" default="116.0"/>

    <template>
        <frame pos="{{ x }} {{ y }}">
            <label
                    class="text-primary"
                    text="{{ module.ModuleInfo.Id }} ({{ module.ModuleInfo.Name }}) v{{ module.ModuleInfo.Version }}"
                    pos="1 -1"
                    size="{{ width - 50 }} 3"
            />
            <label
                    class="text-secondary"
                    text='{{ module.ModuleInfo.IsInternal ? $"[{Locale.InternalModule}] " : "" }}{{ module.Status == ModuleStatus.Enabled ? Locale.Status_Enabled : module.Status == ModuleStatus.Loaded ? Locale.Status_Loaded : module.Status == ModuleStatus.Disabled ? Locale.Status_Disabled : Locale.Status_Error }}'
                    textcolor='{{ module.Status == ModuleStatus.Enabled ? "0C0" : module.Status == ModuleStatus.Loaded ? "FC0" : module.Status == ModuleStatus.Disabled ? "C33" : "F00" }}'
                    pos="1 -4"
                    size="{{ width - 50 }} 3"
            />

            <IconButton
                    icon="{{ Icons.ToggleOff }}"
                    id="btnDisable-{{ module.LoadId }}"
                    style="round" size="small"
                    x="{{ width - 44 }}" y="-1"
                    action="ModuleManagerManialinkController/DisableModule/{{ module.ModuleInfo.Id }}"
                    if="module.IsEnabled"
            />
            <IconButton
                    icon="{{ Icons.ToggleOn }}"
                    id="btnEnable-{{ module.LoadId }}"
                    style="round" size="small"
                    x="{{ width - 44 }}" y="-1"
                    action="ModuleManagerManialinkController/EnableModule/{{ module.ModuleInfo.Id }}"
                    if="!module.IsEnabled"
            />
            <IconButton
                    icon="{{ Icons.Refresh }}"
                    id="btnReload-{{ module.LoadId }}"
                    style="round" size="small" type="secondary"
                    x="{{ width - 32 }}" y="-1"
                    action="ModuleManagerManialinkController/ReloadModule/{{ module.ModuleInfo.Id }}"
                    if="!module.ModuleInfo.IsInternal"
            />
            <IconButton
                    icon="{{ Icons.Eject }}"
                    id="btnUnload-{{ module.LoadId }}"
                    style="round" size="small" type="secondary"
                    x="{{ width - 20 }}" y="-1"
                    action="ModuleManagerManialinkController/UnloadModule/{{ module.ModuleInfo.Id }}"
                    if="!module.ModuleInfo.IsInternal"
            />
            <IconButton
                    icon="{{ Icons.Trash }}"
                    id="btnUninstall-{{ module.LoadId }}"
                    style="round" size="small" type="secondary"
                    x="{{ width - 8 }}" y="-1"
                    action="ModuleManagerManialinkController/UninstallModule/{{ module.ModuleInfo.Id }}"
                    if="!module.ModuleInfo.IsInternal"
            />
        </frame>
    </template>
</component>
