<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>
    <using namespace="EvoSC.Modules.Official.CurrentMapModule.Config"/>

    <import component="EvoSC.Containers.Widget" as="Widget"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>

    <property type="ICurrentMapSettings" name="settings"/>
    <property type="IMap" name="map"/>
    <property type="string" name="mapAuthor"/>
    <property type="int" name="authorTime"/>

    <template>
        <UIStyle/>
        <Widget height="9" position="{{ settings.Position }}" y="{{ settings.Y + 4.5 }}">
            <template slot="body">
                <label text="{{ map.Name }}"
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
                       pos="2.0 -5.5"
                       size="{{ settings.Width/2.0 }} 5"
                />
                <label text="{{ RaceTime.FromMilliseconds(authorTime) }} {{ Icons.ClockO }}"
                       textprefix="$t"
                       textfont="{{ Font.Thin }}"
                       class="text-primary" 
                       textsize="0.35"
                       pos="{{ settings.Width - 1.5 }} -5.5"
                       size="{{ settings.Width/2.0 }} 5"
                       halign="right"
                />
            </template>
        </Widget>
    </template>
</component>