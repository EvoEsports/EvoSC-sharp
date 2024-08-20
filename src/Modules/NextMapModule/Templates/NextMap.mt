<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>
    <using namespace="EvoSC.Modules.Official.NextMapModule.Config"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    
    <property type="INextMapSettings" name="settings" />
    <property type="string" name="mapName"/>
    <property type="string" name="mapAuthor"/>
    
    <template>
        <UIStyle/>
        <Widget header="upcoming map" height="9" position="{{ settings.Position }}" y="{{ settings.Y + 4.5 }}">
            <template slot="body">
                <label text="{{ mapName }}"
                       textfont="{{ Font.Bold }}"
                       textsize="0.75"
                       class="text-primary"
                       pos="{{ settings.Width/2.0 }} -2.5"
                       halign="center"
                       valign="center"
                       size="{{ settings.Width-4 }} 5"
                />
                <label text="{{ Icons.User }} $<{{ mapAuthor.ToUpper() }}$>"
                       textprefix="$t"
                       textfont="{{ Font.Thin }}"
                       class="text-primary"
                       textsize="0.35"
                       halign="center"
                       pos="{{ settings.Width/2.0 }} -5.5"
                       size="{{ settings.Width/2.0 }} 5"
                />
            </template>
        </Widget>
    </template>
</component>