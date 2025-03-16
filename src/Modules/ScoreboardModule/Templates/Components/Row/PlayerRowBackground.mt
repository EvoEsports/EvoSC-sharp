<component>
    <property type="string" name="id"/>
    <property type="double" name="rowHeight"/>
    <property type="double" name="padding"/>
    <property type="double" name="w"/>
    <property type="double" name="h"/>
    <property type="double" name="x"/>

    <template>
        <frame id="{{ id }}"
               pos="{{ x }}"
        >
            <quad id="background"
                  size="{{ w }} {{ h }}"
                  bgcolor="{{ Theme.ScoreboardModule_Background_Row_Color }}"
                  opacity="{{ Theme.ScoreboardModule_Background_Row_Opacity }}"
            />
            <frame pos="{{ w / 2f }} {{ h / -2f }}">
                <quad id="custom_gradient"
                      size="{{ h }} {{ w }}"
                      pos="{{ w / -2f }} {{ h / -2f }}"
                      rot="-90"
                      image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                      modulatecolor="{{ Theme.UI_AccentPrimary }}"
                      opacity="0.25"
                />
            </frame>
        </frame>
    </template>

    <script resource="ScoreboardModule.Scripts.PlayerRowBg" once="true" />
</component>