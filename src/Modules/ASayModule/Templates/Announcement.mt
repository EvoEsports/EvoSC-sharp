<component>
    <property type="string" name="text"/>
  
    <template>
        <frame id="window" pos="-80 47" halign="center" valign="center">
            <frame id="textFrame" size="160 10" valign="center2" halign="left" pos="0 0" z-index="0">
                <label 
                      id="text" 
                      size="160 10" 
                      valign="center2" 
                      halign="left" 
                      textsize="{{ Theme.ASayModule_Announcement_Text_Size }}" 
                      text="{{ text }}"
                      textfont="{{ Theme.UI_Font }}" 
                      pos="0 0" 
                      textcolor="{{ Theme.ASayModule_Announcement_Text }}" 
                      z-index="1"
                      textprefix="$t$s" />
            </frame>
          
            <quad 
                    id="bgQuad"
                    size="170 10"
                    style="UICommon64_1" 
                    substyle="BgFrame1" 
                    halign="left" 
                    colorize="{{ Theme.ASayModule_Announcement_Bg }}"
                    opacity="{{ Theme.ASayModule_Announcement_Bg_Opacity }}"
                    pos="0 0"
                    valign="center" 
                    z-index="-1" />
          
            <label 
                    size="10 10"
                    valign="center2"
                    halign="center" 
                    textsize="{{ Theme.ASayModule_Announcement_Icon_Size }}" 
                    text="{{ Icons.Bullhorn }}"
                    textcolor="{{ Theme.ASayModule_Announcement_Icon }}"
                    z-index="3" 
                    pos="5 0" />
          
            <quad 
                    size="10 10"
                    style="UICommon64_1"
                    substyle="BgFrame1"
                    halign="left" 
                    colorize="{{ Theme.ASayModule_Announcement_Bg_Secondary }}" 
                    opacity="1"
                    pos="0 0"
                    valign="center"
                    z-index="2" />
        </frame>
    </template>

    <script main="true">
      <!--
      #Include "MathLib" as ML
      #Const C_MaxWidth 160.
      #Const C_IconWidth 12.
      
      main() {
          declare CMlFrame Frame <=> Page.GetFirstChild("window") as CMlFrame;
                          declare CMlFrame InnerFrame <=> Page.GetFirstChild("textFrame") as CMlFrame;
          declare CMlLabel Label = Page.GetFirstChild("text") as CMlLabel;
          declare CMlQuad BgQuad <=> Page.GetFirstChild("bgQuad") as CMlQuad;
          declare Real Width = Label.ComputeWidth(Label.Value);
          declare CMlLabel DbgLabel <=> Page.GetFirstChild("debugStuff") as CMlLabel;
          if (Width >= C_MaxWidth) {
              Width = C_MaxWidth;
          }
                          Width += C_IconWidth;
          Frame.RelativePosition_V3.X = -(Width) /2.;
          BgQuad.Size.X = Width;
          InnerFrame.RelativePosition_V3.X = 11.;
          if (Width == (C_MaxWidth + C_IconWidth)) {
              declare Real Pos = Width/2. +5;
              Label.RelativePosition_V3.X = Pos;
              while(True) {
                      yield;
                      Label.RelativePosition_V3 = <(ML::Sin(Now*0.0005)) * (Width/8), 0.>;	
              }
          }
      }
      
      -->
    </script>
</component>