<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>
    <using namespace="EvoSC.Modules.Official.CurrentMapModule.Config"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>

    <property type="ICurrentMapSettings" name="settings"/>
    <property type="IMap?" name="map" default="null"/>
    <property type="string" name="mapAuthor" default="null"/>

    <template>
        <UIStyle/>
        <Widget header="current map" height="10" position="{{ settings.Position }}" y="{{ settings.Y }}">
            <template slot="body">
                <frame pos='{{ settings.Position=="right" ? settings.Width-2.0 : 0 }} 0'>
                    <label text="{{ map?.Name }}"
                           class="text-primary"
                           pos="0 -3"
                           valign="center"
                           halign="right"
                           size="{{ settings.Width-2 }} 5"
                    />
                    <label text="$<$tby {{ mapAuthor }}$>"
                           class="text-primary"
                           textsize="0.75"
                           pos="0 -6.5"
                           valign="center"
                           halign="right"
                           size="{{ settings.Width-2 }} 5"
                    />
                </frame>
            </template>
        </Widget>
    </template>
</component>