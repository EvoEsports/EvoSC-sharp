<component>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="80"/>
    <property type="double" name="radius" default="3.0"/>
    <property type="double" name="headerHeight" default="14.0"/>

    <template>
        <framemodel id="Scoreboard_RoundedCorner_Header">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeader }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"
                      opacity="0.99"/>
            </frame>
        </framemodel>
        <framemodel id="Scoreboard_RoundedCorner_Header_Primary">
            <frame size="{{ radius }} {{ radius }}">
                <quad size="{{ radius * 2.0 }} {{ radius * 2.0 }}"
                      modulatecolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeaderGrad }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
        </framemodel>

        <!-- Top bar -->
        <frameinstance modelid="Scoreboard_RoundedCorner_Header"/>
        <quad pos="{{ radius }} 0" size="{{ w - radius * 2.0 }} {{ radius }}" bgcolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeader }}"
              opacity="0.99"/>
        <quad pos="0 {{ -radius }}" size="{{ w - radius }} {{ headerHeight - radius }}" bgcolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeader }}"
              opacity="0.99"/>
        <frameinstance modelid="Scoreboard_RoundedCorner_Header_Primary" pos="{{ w }}" rot="90"/>
        <quad pos="{{ w - radius }} {{ -radius }}" size="{{ radius }} {{ headerHeight - radius }}"
              bgcolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeaderGrad }}"/>

        <!-- Gradient -->
        <frame pos="{{ -radius }} {{ -headerHeight }}">
            <quad size="{{ headerHeight }} {{ w }}"
                  rot="270"
                  modulatecolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeaderGrad }}"
                  image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
            />
        </frame>

        <!-- Middle part -->
        <quad pos="0 {{ -headerHeight }}" size="{{ w }} {{ h + radius }}" bgcolor="{{ Theme.ScoreboardModule_BackgroundBox_BgList }}" opacity="0.9"/>
    </template>
</component>