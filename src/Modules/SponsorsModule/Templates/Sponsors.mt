<component>
    <using namespace="System.Linq"/>
    
    <property type="List<string>" name="sponsorImageUrls"/>
    <property type="double" name="scale" default="0.9"/>
    <property type="double" name="w" default="68.0"/>
    <property type="double" name="x" default="-160.0"/>
    <property type="double" name="y" default="41.0"/>
    <property type="double" name="headerHeight" default="8.0"/>
    <property type="double" name="bodyHeight" default="23.0"/>
    <property type="double" name="padding" default="4.0"/>
    <property type="int" name="showDuration" default="7"/>

    <property type="string" name="headerColor" default="c21d62"/>
    <property type="string" name="primaryColor" default="4357ea"/>
    <property type="string" name="playerRowBackgroundColor" default="999999"/>
    <property type="string" name="logoUrl" default=""/>
    
    <template>
        <frame id="main_frame" pos="{{ x }} {{ y }}" size="{{ w }} 999" scale="{{ scale }}" z-index="100">
            <frame>
                <frame size="{{ w }} {{ headerHeight - 0.1 }}">
                    <!-- HEADER -->
                    <quad pos="{{ w * 0.5 - 10.0 }} 0.15"
                          size="{{ headerHeight + 0.3 }} {{ w + 20.1 }}"
                          valign="center"
                          style="UICommon64_1"
                          substyle="BgFrame1"
                          colorize="{{ primaryColor }}"
                          rot="90"
                    />

                    <!-- GRADIENT -->
                    <quad size="{{ w }} {{ headerHeight + 0.1 }}"
                          image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                          modulatecolor="{{ headerColor }}"
                    />

                    <!-- LABEL -->
                    <label pos="2 {{ headerHeight / -2.0 - 0.4 }}"
                           text="Sponsors"
                           valign="center2"
                           textfont="GameFontExtraBold"
                           textprefix="$i$t"
                           textsize="2"
                    />

                    <!-- LOGO -->
                    <quad if='logoUrl != ""'
                          pos="{{ w - 3.0 }} {{ headerHeight / -2.0 }}"
                          size="20 3.2"
                          valign="center"
                          halign="right"
                          keepratio="Fit"
                          image="{{ logoUrl }}"
                          opacity="0.75"
                    />
                </frame>

                <!-- BACKGROUND -->
                <quad pos="0 {{ -headerHeight + 0.1 }}"
                      size="{{ w - 0.15 }} {{ bodyHeight }}"
                      bgcolor="24262f"
                      opacity="0.9"
                />
            </frame>

            <!-- CONTENT -->
            <frame id="slides" pos="0 {{ -headerHeight }}" z-index="10">
                <quad foreach="string imgUrl in sponsorImageUrls"
                      pos="{{ w / -2.0 }} {{ bodyHeight / -2.0 }}"
                      size="{{ w - (padding * 2.0) }} {{ bodyHeight - (padding * 2.0) }}"
                      image="{{ imgUrl }}"
                      keepratio="Fit"
                      valign="center"
                      halign="center"
                />
            </frame>
        </frame>
    </template>
    
    <script><!--
    
    Void SlideIn(CMlQuad sponsorQuad) {
        declare targetX = {{ w / 2.0 }} * 1.0;
        AnimMgr.Add(sponsorQuad, "<quad pos='" ^ targetX ^ " {{ bodyHeight / -2.0 }}' />", 320, CAnimManager::EAnimManagerEasing::ExpOut);
    }
    
    Void SlideOut(CMlQuad sponsorQuad) {
        declare targetX = {{ w / -2.0 }} * 1.0;
        AnimMgr.Add(sponsorQuad, "<quad pos='" ^ targetX ^ " {{ bodyHeight / -2.0 }}' />", 320, CAnimManager::EAnimManagerEasing::ExpOut);
    }
    
    main() {
        declare mainFrame <=> (Page.MainFrame.GetFirstChild("main_frame") as CMlFrame);
        declare slidesFrame <=> (Page.MainFrame.GetFirstChild("slides") as CMlFrame);
        declare maxIndex = slidesFrame.Controls.count - 1;
        declare lastUpdate = 0;
        declare cursor = 0;
        
        while(True){
            yield;
            
            if(GUIPlayer == Null || InputPlayer != GUIPlayer){
                if(!mainFrame.Visible){
                    mainFrame.Show();
                }
            }else{
                if(mainFrame.Visible){
                    mainFrame.Hide();
                }
                sleep(100);
                continue;
            }
            
            if(Now > lastUpdate + {{ showDuration }} * 1000){
                declare otherIndex = cursor - 1;
                if(otherIndex < 0){
                    otherIndex = maxIndex;
                }
                
                declare targetQuad = (slidesFrame.Controls[cursor] as CMlQuad);
                declare otherQuad = (slidesFrame.Controls[otherIndex] as CMlQuad);
                SlideOut(otherQuad);
                SlideIn(targetQuad);
                
                lastUpdate = Now;
                cursor += 1;
                if(cursor > maxIndex){
                    cursor = 0;
                }
            }
        }
    }
    --></script>
</component>