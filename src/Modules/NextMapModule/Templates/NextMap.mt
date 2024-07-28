<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>
    <using namespace="EvoSC.Modules.Official.NextMapModule.Config"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    
    <property type="INextMapSettings" name="settings" />
    <property type="string" name="mapName" />
    <property type="string" name="author" />
    
    <template>
        <UIStyle/>
        <Widget header="upcoming map" height="10" position="{{ settings.Position }}" y="{{ settings.Y }}">
            <template slot="body">
                <frame pos='{{ settings.Position=="right" ? settings.Width-2.0 : 0 }} 0'>
                    <label text="{{ mapName }}"
                           class="text-primary"
                           pos="0 -3"
                           valign="center"
                           halign="right"
                           size="{{ settings.Width-2 }} 5"
                    />
                    <label text="$<$tby {{ author }}$>"
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