<component>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="80"/>
    <property type="double" name="radius" default="3.0"/>
    <property type="double" name="headerHeight" default="14.0"/>
    <property type="double" name="headerGap" default="0.2"/>
    <property type="string" name="headerColor" default="0c0f31"/>
    <property type="string" name="color" default="041138"/>

    <template>
        <framemodel id="Scoreboard_RoundedCorner_Header">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ headerColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.93"/>
            </frame>
        </framemodel>
        <framemodel id="Scoreboard_RoundedCorner">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ color }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.7"/>
            </frame>
        </framemodel>

        <!-- Top bar -->
        <frameinstance modelid="Scoreboard_RoundedCorner_Header"/>
        <quad pos="{{ radius }} 0" size="{{ w - radius }} {{ radius }}" bgcolor="{{ headerColor }}" opacity="0.93"/>
        <quad pos="0 {{ -radius }}" size="{{ w }} {{ headerHeight - radius }}" bgcolor="{{ headerColor }}" opacity="0.93"/>

        <!-- Middle part -->
        <quad pos="0 {{ -headerHeight - headerGap }}" size="{{ w }} {{ h }}" bgcolor="{{ color }}" opacity="0.7"/>

        <!-- Bottom bar -->
        <quad pos="0 {{ -h - headerHeight - headerGap - 0.02 }}" size="{{ w - radius }} {{ radius }}" bgcolor="{{ color }}" opacity="0.7"/>
        <frameinstance modelid="Scoreboard_RoundedCorner" pos="{{ w }} {{ -h - headerHeight - headerGap - radius - 0.02 }}" rot="180"/>

        <!-- Gradient -->
        <frame pos="0 {{ -headerHeight - headerGap }}" size="{{ w }} {{ h + radius }}">
            <frame pos="{{ w * 1.5 }} 0">
                <quad size="{{ w * 3.0 }} {{ w * 1.5 }}"
                      rot="115"
                      modulatecolor="c22477"
                      image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                      opacity="0.3"/>
            </frame>
        </frame>
    </template>
</component>