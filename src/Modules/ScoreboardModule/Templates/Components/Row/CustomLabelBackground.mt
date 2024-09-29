<component>
    <property type="string" name="id" />
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="width" default="0.0"/>
    <property type="double" name="height" default="0.0"/>
    
    <template>
        <frame id="{{ id }}" pos="{{ x }} {{ y }}">
            <quad size="{{ height }} {{ width }}"
                  pos="{{ width }} 0"
                  rot="-90"
                  class="modulate"
                  modulatecolor="ffffff"
                  opacity="0.25"
                  image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
            />
        </frame>
    </template>
</component>