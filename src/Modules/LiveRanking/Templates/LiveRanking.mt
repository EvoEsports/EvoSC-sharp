<component>
    <using namespace="EvoSC.Modules.Official.LiveRankingModule.Models" />
    <using namespace="EvoSC.Common.Interfaces.Models" />
    <property type="List<LiveRankingWidgetPosition>" name="liverankings" />
    
    <template>
        <frame id="liveranking" pos="-158 55" z-index="-5">
            <quad id="live_ranking_bg" size="55 6" pos="0 0" bgcolor="005" opacity="0.5" z-index="0"/>
            <Label textfont="GameFontBlack" textprefix="$t$fff" x="27.5" y="-3" text="LIVE RANKING"  halign="center" valign="center2" zIndex="2"/>
        </frame>

        <Frame x="-158" y="{{50 - (__index * 5)}}" size="55 23" foreach="LiveRankingWidgetPosition pos in liverankings" zIndex="15">
            <Quad id="live_map_bg_grad" w="55" h="5" x="0" y="-3.6" modulatecolor="1856d1" opacity="0.8" zIndex="5" valign="center2" halign="left" image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"/>
            <Label x="1" y="-4" w="3" h="10" textprefix="$fff$t" text="{{pos.position}}" textfont="GameFontExtraBold" textsize="1" valign="center2" halign="left" zIndex="10"/>
            <Label x="6" y="-4" w="35" h="10" textprefix="$fff$t" text="{{pos.player.NickName}}" textfont="GameFontExtraBold" textsize="1" valign="center2" halign="left" zIndex="10"/>
            <Label x="54" y="-4" w="15" h="10" textprefix="$fff$t" text="{{pos.time}}" textfont="GameFontExtraBold" textsize="1" valign="center2" halign="right" zIndex="10" />
        </Frame>
        
    </template>
</component>