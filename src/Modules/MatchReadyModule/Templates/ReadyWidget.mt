<component>
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.Switch" as="Switch" />
    
    <property type="bool" name="isReady" default="false" />
    <property type="int" name="requiredPlayers" default="0" />
    <property type="int" name="playersReady" default="0" />
    <property type="bool" name="showButton" default="true" />
    
    <template>
        <Theme />

        
        <frame pos="-20 60">
            <frame if="showButton">
                <quad
                        bgcolor="000000cc"
                        size="40 18"
                />
                <frame size="40 18">
                    <quad
                            bgcolor="000000cc"
                            size="18 60"
                            pos="-18 -18"
                            image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                            modulatecolor="F43A3A"
                            rot="-90"
                            id="readywidget-bg"
                    />
                </frame>

                <label
                        class="text"
                        textfont="GameFontSemiBold"
                        textsize="2"
                        textcolor="ffffff"
                        text="$iNOT READY"
                        halign="center"
                        pos="20 -2"
                        id="readywidget-statustext"
                />

                <frame pos="5 -8">
                    <quad bgcolor="CE3535" size="30 8" id="readybtn-background" />

                    <frame size="30 8">
                        <quad
                                rot="167"
                                modulatecolor="F43A3A"
                                size="32 20"
                                pos="33 -12"
                                image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                                id="readybtn-background-gradient"
                        />
                    </frame>

                    <label
                            textfont="GameFontRegular"
                            textsize="3"
                            textcolor="000000"
                            text='{{ isReady ? "UN-READY" : "I AM READY" }}'
                            id="readybtn-text"
                            halign="center"
                            valign="center"
                            pos="15 -3.8"
                    />
                </frame>

                <quad bgcolor="ff0000" size="20 1" pos="0 0.5" id="readywidget-line-top" />
                <quad bgcolor="ff0000" size="1 9.5" pos="-0.5 0.5" id="readywidget-line-left" />

                <quad bgcolor="ff0000" size="20 1" pos="20 -17.5" id="readywidget-line-right" />
                <quad bgcolor="ff0000" size="1 9.5" pos="39.5 -9" id="readywidget-line-bottom" />
            </frame>
            
            <frame pos="0 10">
                <quad
                        image="file:///Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                        size="30 7"
                        pos="20 2"
                        modulatecolor="3759f4"
                />
                <quad
                        rot="180"
                        image="file:///Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                        size="30 7"
                        pos="20 -5"
                        modulatecolor="3759f4"
                />

                <label
                        text="$s{{ playersReady }}/{{ requiredPlayers }} PLAYERS READY"
                        class="text"
                        textsize="3"
                        textcolor="ffffff"
                        pos="20 0.5"
                        halign="center"
                        if="requiredPlayers > 0"
                        id="readywidget-readyplayers-text"
                />
            </frame>
        </frame>
    </template>

    <script><!--
    declare Vec3 c1NotReady;
    declare Vec3 c2NotReady;
    declare Vec3 c1Ready;
    declare Vec3 c2Ready;
    declare Integer playerCount;
    declare Boolean isReady;
    
    Boolean IsMouseOver(CMlQuad bg) {
        declare rPos = bg.AbsolutePosition_V3;
        declare rSize =  bg.Size;
        
        return MouseX >= rPos.X && MouseX <= rPos.X + rSize.X
        &&  MouseY <= rPos.Y && MouseY >= rPos.Y - rSize.Y;
    }
    
    Void UpdateWidget(Boolean isReady, Boolean isHover) {
        declare readyWidgetBg <=> Page.MainFrame.GetFirstChild("readywidget-bg") as CMlQuad;
        declare readyWidgetStatusText <=> Page.MainFrame.GetFirstChild("readywidget-statustext") as CMlLabel;
        declare readyBtnText <=> Page.MainFrame.GetFirstChild("readybtn-text") as CMlLabel;
        declare readyBtnBg <=> Page.MainFrame.GetFirstChild("readybtn-background") as CMlQuad;
        declare readyBtnBgG <=> Page.MainFrame.GetFirstChild("readybtn-background-gradient") as CMlQuad;
        
        declare lineTop <=> Page.MainFrame.GetFirstChild("readywidget-line-top") as CMlQuad;
        declare lineLeft <=> Page.MainFrame.GetFirstChild("readywidget-line-left") as CMlQuad;
        declare lineRight <=> Page.MainFrame.GetFirstChild("readywidget-line-right") as CMlQuad;
        declare lineBottom <=> Page.MainFrame.GetFirstChild("readywidget-line-bottom") as CMlQuad;
        
        if (isReady) {
            readyWidgetBg.ModulateColor = TextLib::ToColor("60F437");
            
            if (isHover) {
                readyBtnBg.BgColor = c1Ready;
                readyBtnBgG.ModulateColor = c2Ready;
            } else {
                readyBtnBg.BgColor = c2Ready;
                readyBtnBgG.ModulateColor = c1Ready;
            }
            
            readyWidgetStatusText.SetText("$iREADY");
            readyBtnText.SetText("$iUN-READY");
            
            lineTop.BgColor = <0., 1., 0.>;
            lineLeft.BgColor = <0., 1., 0.>;
            lineRight.BgColor = <0., 1., 0.>;
            lineBottom.BgColor = <0., 1., 0.>;
            
            AnimMgr.Add(lineTop, """<quad size='40 1' pos='0 0.5' />""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
            AnimMgr.Add(lineLeft, """<quad size='1 19' pos='-0.5 0.5' />""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
            AnimMgr.Add(lineRight, """<quad size='40 1' pos='0 -17.5' />""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
            AnimMgr.Add(lineBottom, """<quad size='1 19' pos='39.5 0.5'/>""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
        } else {
            readyWidgetBg.ModulateColor = TextLib::ToColor("F43A3A");

            if (isHover) {
                readyBtnBg.BgColor = c1NotReady;
                readyBtnBgG.ModulateColor = c2NotReady;
            } else {
                readyBtnBg.BgColor = c2NotReady;
                readyBtnBgG.ModulateColor = c1NotReady;
            }
            
            readyWidgetStatusText.SetText("$iNOT READY");
            readyBtnText.SetText("$iI AM READY");
            
            lineTop.BgColor = <1., 0., 0.>;
            lineLeft.BgColor = <1., 0., 0.>;
            lineRight.BgColor = <1., 0., 0.>;
            lineBottom.BgColor = <1., 0., 0.>;
            
            AnimMgr.Add(lineTop, """<quad size='20 1' pos='0 0.5' />""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
            AnimMgr.Add(lineLeft, """<quad size='1 9.5' pos='-0.5 0.5' />""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
            AnimMgr.Add(lineRight, """<quad size='20 1' pos='20 -17.5' />""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
            AnimMgr.Add(lineBottom, """<quad size='1 9.5' pos='39.5 -9'/>""", 400, CAnimManager::EAnimManagerEasing::ExpOut);
        }
    }
    
    Void CheckForUpdate() {
        declare Integer EvoSC_ReadyWidget_PlayerCount for This = 0;
        declare Boolean EvoSC_ReadyWidget_HasUpdate for This = False;
        declare Boolean EvoSC_ReadyWidget_IsReady for This = False;

        if (!EvoSC_ReadyWidget_HasUpdate) {
            return;
        }
        
        declare readyplayersText = Page.MainFrame.GetFirstChild("readywidget-readyplayers-text") as CMlLabel;
        readyplayersText.SetText("$i$s" ^ EvoSC_ReadyWidget_PlayerCount ^ "/{{ requiredPlayers }} PLAYERS READY");
        
        isReady = EvoSC_ReadyWidget_IsReady;
        EvoSC_ReadyWidget_HasUpdate = False;
        
        if (!{{ showButton }}) {
            return;
        }
        
        UpdateWidget(EvoSC_ReadyWidget_IsReady, False);
    }
    
    *** OnInitialization ***
    ***
    declare CMlQuad readyBtnBg;
    declare CMlQuad readyBtnBgG;
    
    if ({{ showButton }}) {
        readyBtnBg <=> Page.MainFrame.GetFirstChild("readybtn-background") as CMlQuad;
        readyBtnBgG <=> Page.MainFrame.GetFirstChild("readybtn-background-gradient") as CMlQuad;
    }
    
    isReady = {{ isReady }};

    c1NotReady = TextLib::ToColor("F43A3A");
    c2NotReady = TextLib::ToColor("CE3535");
    c1Ready = TextLib::ToColor("6FF43A");
    c2Ready = TextLib::ToColor("3ACE35");
    ***
    
    *** OnMouseMove ***
    ***
        if ({{ showButton }}) {
            declare c1 = c1NotReady;
            declare c2 = c2NotReady;
            
            if (isReady) {
                c1 = c1Ready;
                c2 = c2Ready;
            }
        
            if (IsMouseOver(readyBtnBg)) {
                readyBtnBgG.ModulateColor = c2;
                readyBtnBg.BgColor = c1;
            } else {
                readyBtnBgG.ModulateColor = c1;
                readyBtnBg.BgColor = c2;
            }
        }
    ***
    
    *** OnMouseUp ***
    ***
        if ({{ showButton }} && IsMouseOver(readyBtnBg)) {
            // pre-update widget for better UI experience
            UpdateWidget(!isReady, True);
            TriggerPageAction("ReadyManialinkController/ReadyButton/" ^ (!isReady));
        }
    ***
    
    *** OnLoop ***
    ***
    CheckForUpdate();
    ***
    --></script>
    
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
