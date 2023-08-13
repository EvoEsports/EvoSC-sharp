<component>
    <import component="EvoSC.Theme" as="Theme" />
    <property name="mapName" type="string" />
    <property name="author" type="string" />
    
    <template>
        <Theme />

        <frame pos="100 80">
            <quad pos="0 0" z-index="0" size="60 18" bgcolor="191A21" opacity="0.7"/>
            <quad pos="0 0" z-index="0" size="60 5" bgcolor="3658f1" opacity="1"/>
            <quad pos="60 0" z-index="0" size="5 60" rot="90" opacity=".3" image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga" modulatecolor="000"/>
            <label pos="2 -2.5" textsize="1" valign="center" z-index="0" size="20 5" text="Next map" textfont="GameFontRegular"  textemboss="1"/>
            <label pos="2 -8" textsize="2" valign="center" z-index="0" size="56 5" text="{{ mapName }}" textfont="GameFontSemiBold" />
            <label pos="2 -13" textsize="1" valign="center" z-index="0" size="56 5" textprefix="☺" text="by {{ author }}" textfont="GameFontSemiBold" />
        </frame>
    </template>
</component>