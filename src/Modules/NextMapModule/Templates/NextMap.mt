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
        
<!--        <frame pos="{{ 160.0 - w * scale }} {{ y }}" scale="{{ scale }}" z-index="100">-->
<!--            <frame>-->
<!--                <frame size="{{ w }} {{ headerHeight - 0.1 }}">-->
<!--                    &lt;!&ndash; HEADER &ndash;&gt;-->
<!--                    <quad pos="-0.15 0"-->
<!--                          size="{{ w + 20.1 }} {{ headerHeight + 0.3 }}"-->
<!--                          style="UICommon64_1"-->
<!--                          substyle="BgFrame1"-->
<!--                          colorize="{{ Theme.NextMapModule_NextMap_Default_BgHeaderGrad1 }}"-->
<!--                    />-->
<!--    -->
<!--                    &lt;!&ndash; GRADIENT &ndash;&gt;-->
<!--                    <quad pos="{{ w }} {{ -headerHeight }}"-->
<!--                          size="{{ w }} {{ headerHeight - 0.1 }}"-->
<!--                          image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"-->
<!--                          modulatecolor="{{ Theme.NextMapModule_NextMap_Default_BgHeaderGrad2 }}"-->
<!--                          rot="180"-->
<!--                    />-->
<!--    -->
<!--                    &lt;!&ndash; LABEL &ndash;&gt;-->
<!--                    <label pos="2 {{ headerHeight / -2.0 - 0.4 }}"-->
<!--                           text="Next Map"-->
<!--                           valign="center2"-->
<!--                           textfont="{{ Font.Bold }}"-->
<!--                           textprefix="$i$t"-->
<!--                           textsize="2"-->
<!--                           textcolor="{{ Theme.NextMapModule_NextMap_Default_Text }}"-->
<!--                    />-->
<!--    -->
<!--                    &lt;!&ndash; LOGO &ndash;&gt;-->
<!--                    <quad if='logoUrl != ""'-->
<!--                          pos="{{ w - 3.0 }} {{ headerHeight / -2.0 }}"-->
<!--                          size="20 3.2"-->
<!--                          valign="center"-->
<!--                          halign="right"-->
<!--                          keepratio="Fit"-->
<!--                          image="{{ Theme.NextMapModule_NextMap_Default_Logo }}"-->
<!--                          opacity="0.75"-->
<!--                    />-->
<!--                </frame>-->
<!--    -->
<!--                &lt;!&ndash; BACKGROUND &ndash;&gt;-->
<!--                <quad pos="0 {{ -headerHeight + 0.1 }}"-->
<!--                      size="{{ w }} {{ bodyHeight }}"-->
<!--                      bgcolor="{{ Theme.NextMapModule_NextMap_Default_BgContent }}"-->
<!--                      opacity="0.9"-->
<!--                />-->
<!--            </frame>-->
<!--    -->
<!--            <framemodel id="gradient_box">-->
<!--                <frame size="0.95 8">-->
<!--                    <quad pos="0 0.2"-->
<!--                          size="2 8.4"-->
<!--                          style="UICommon64_1"-->
<!--                          substyle="BgFrame2"-->
<!--                          colorize="{{ Theme.NextMapModule_NextMap_Default_BgRow }}"-->
<!--                          opacity="0.75"-->
<!--                    />-->
<!--                </frame>-->
<!--    -->
<!--                &lt;!&ndash; GRADIENT &ndash;&gt;-->
<!--                <quad pos="1 0"-->
<!--                      size="{{ w - 10.0 }} 8"-->
<!--                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"-->
<!--                      modulatecolor="{{ Theme.NextMapModule_NextMap_Default_BgRow }}"-->
<!--                      opacity="0.75"-->
<!--                />-->
<!--            </framemodel>-->
<!--    -->
<!--            &lt;!&ndash; CONTENT &ndash;&gt;-->
<!--            <frame pos="2 {{ -headerHeight - 2.0 }}" size="{{ w }} 999" z-index="10">-->
<!--                <frameinstance modelid="gradient_box" />-->
<!--                <frame pos="-0.2 -1">-->
<!--                    <label pos="2 0"-->
<!--                           text="{{ mapName }}"-->
<!--                           textsize="1.4"-->
<!--                           textfont="{{ Font.Regular }}"-->
<!--                           textcolor="{{ Theme.NextMapModule_NextMap_Default_Text }}"/>-->
<!--                    <label pos="2 -3.2"-->
<!--                           text="by {{ author }}"-->
<!--                           textsize="1.1"-->
<!--                           textfont="{{ Font.Thin }}"-->
<!--                           textcolor="{{ Theme.NextMapModule_NextMap_Default_Text }}"/>-->
<!--                </frame>-->
<!--            </frame>-->
<!--        </frame>-->
    </template>
</component>