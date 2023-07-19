<component>
    <property type="int" name="zIndex" default="0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w"/>
    <property type="double" name="h"/>
    <property type="string" name="title" default="Test"/>
    <property type="double" name="titleBarHeight" default="6.0"/>
    
    <property type="string" name="halign" default="center"/>
    <property type="string" name="valign" default="center"/>
    <property type="double" name="padding" default="2"/>

    <property type="string" name="primaryColor" default="ff0058"/>
    <property type="string" name="backgroundColor" default="47495A"/>

    <template>
        <frame id="window" pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}" valign="{{ valign }}" halign="{{ halign }}">
            <!-- Title Bar -->
            <quad pos="0 {{ h/2-titleBarHeight/2 }}" size="{{ w }} {{ titleBarHeight }}" valign="center" halign="center" bgcolor="{{ primaryColor }}" opacity="1" z-index="{{ zIndex }}"/>
            <!-- Title Bar Close Button -->
            <label id="window_close_button" scriptevents="1" pos="{{ w/2-2 }} {{ h/2-titleBarHeight/2 }}" textsize=".6" halign="right" valign="center2" text="❌" z-index="{{ zIndex + 1 }}"/>
            <!-- Title Bar Title -->
            <label pos="{{ (w*-1)/2+padding }} {{ h/2-padding }}" text="{{ title }}" textsize=".8" z-index="{{ zIndex + 1 }}" />
            <!-- Content Background -->
            <quad pos="0 0" size="{{ w }} {{ h }}" valign="center" halign="center" bgcolor="{{ backgroundColor }}" opacity="1" />
            <!-- Content -->
            <frame pos="{{ (w*-1)/2+padding }} {{ (h/2-titleBarHeight)-padding }}" size="{{ w - 4.0 }} {{ h - titleBarHeight - 4.0 }}" z-index="{{ zIndex + 1 }}">
                <slot/>
            </frame>
        </frame>
        <script>
            <!--
            *** OnMouseClick ***
            ***
            if(Event.Control.ControlId == "window_close_button"){
                Page.GetFirstChild("window").Hide();
                return;
            }
            ***
            -->
        </script>
    </template>
</component>
