<component>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="80"/>
    <property type="double" name="radius" default="3.0"/>
    <property type="double" name="headerHeight" default="14.0"/>
    <property type="string" name="primaryColor" default="0c0f31"/>
    <property type="string" name="headerColor" default="0c0f31"/>
    <property type="string" name="gradientColor" default="c22477"/>
    <property type="double" name="gradientOpacity" default="0.5"/>
    <property type="string" name="color" default="041138"/>

    <template>
        <framemodel id="Scoreboard_RoundedCorner_Header">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ headerColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.99"/>
            </frame>
        </framemodel>
        <framemodel id="Scoreboard_RoundedCorner_Header_Primary">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
        </framemodel>
        <framemodel id="Scoreboard_RoundedCorner">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ color }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.85"/>
            </frame>
        </framemodel>

        <!-- Top bar -->
        <frameinstance modelid="Scoreboard_RoundedCorner_Header"/>
        <quad pos="{{ radius }} 0" size="{{ w - radius * 2.0 }} {{ radius }}" bgcolor="{{ headerColor }}"
              opacity="0.99"/>
        <quad pos="0 {{ -radius }}" size="{{ w - radius }} {{ headerHeight - radius }}" bgcolor="{{ headerColor }}"
              opacity="0.99"/>
        <frameinstance modelid="Scoreboard_RoundedCorner_Header_Primary" pos="{{ w }}" rot="90"/>
        <quad pos="{{ w - radius }} {{ -radius }}" size="{{ radius }} {{ headerHeight - radius }}"
              bgcolor="{{ primaryColor }}"/>

        <!-- Gradient -->
        <frame pos="{{ -radius }} {{ -headerHeight }}">
            <quad size="{{ headerHeight }} {{ w }}"
                  rot="270"
                  modulatecolor="{{ primaryColor }}"
                  image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
            />
        </frame>

        <!-- Middle part -->
        <quad pos="0 {{ -headerHeight }}" size="{{ w }} {{ h + radius }}" bgcolor="{{ color }}" opacity="0.85"/>

        <!-- Bottom bar -->
        <!--        <quad pos="0 {{ -h - headerHeight }}" size="{{ w - radius }} {{ radius + 0.05 }}" bgcolor="{{ color }}" opacity="0.85"/>-->
        <!--        <frameinstance modelid="Scoreboard_RoundedCorner" pos="{{ w }} {{ -h - headerHeight - radius - 0.05 }}" rot="180"/>-->
    </template>
</component>