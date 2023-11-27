<!--
    General theme component for all UI elements. Must be imported for the UI to work.
-->
<component>
    <template>
        <stylesheet>
            <!-- General -->
            <style 
                    class="text"
                    textfont="{{ Theme.UI_TextPrimary }}" 
                    textcolor=""
                    textsize=""
            />
            
            <style 
                    class="bg-primary"
                    bgcolor="{{ Theme.UI_BackgroundPrimary }}"
            />

            <style
                    class="bg-secondary"
                    bgcolor="{{ Theme.UI_BackgroundSecondary }}"
            />
            
            <!-- Buttons -->
            <style 
                    class="btn-default" 
                    textfont="{{ Theme.UI_Font }}" 
                    textcolor="{{ Theme.UI_Button_Default_Text }}"
                    textsize="{{ Theme.UI_FontSize }}"
                    bgcolor="{{ Theme.UI_Button_Default_Bg }}"
                    bgcolorfocus="{{ Theme.UI_Button_Default_BgFocus }}"
                    focusareacolor1="00000000"
                    focusareacolor2="ffffff22" />
            <style 
                    class="btn-secondary" 
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Button_Secondary_Text }}"
                    textsize="{{ Theme.UI_FontSize }}"
                    bgcolor="{{ Theme.UI_BackgroundSecondary }}"
                    bgcolorfocus="{{ Theme.UI_Button_Secondary_BgFocus }}"
                    focusareacolor1="00000000"
                    focusareacolor2="ffffff22"/>
            <style
                    class="btn-disabled"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Button_Default_DisabledText }}"
                    textsize="{{ Theme.UI_FontSize }}"
                    bgcolor="{{ Theme.UI_Button_Default_DisabledBg }}" />
            <style
                    class="btn-secondary-disabled"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Button_Secondary_DisabledText }}"
                    textsize="{{ Theme.UI_FontSize }}"
                    bgcolor="{{ Theme.UI_Button_Secondary_DisabledBg }}" />
            
            <!-- Text Fields -->
            <style 
                    class="textinput-default"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_TextField_Default_Text }}"
                    bgcolor="{{ Theme.UI_TextField_Default_Bg }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />

            <style
                    class="textinput-outline-default"
                    bgcolor="{{ Theme.UI_TextField_Default_Border }}"
            />
            
            <!-- Toggle Switch -->
            <style 
                    class="toggleswitch-on-default"
                    bgcolor="{{ Theme.UI_ToggleSwitch_Default_OnText }}"
                    textcolor="{{ Theme.UI_ToggleSwitch_Default_OnBg }}"
            />
            
            <style
                    class="toggleswitch-off-default"
                    bgcolor="{{ Theme.UI_ToggleSwitch_Default_OffText }}"
                    textcolor="{{ Theme.UI_ToggleSwitch_Default_OffBg }}"
            />
            
            <!-- Checkbox -->
            <style
                    class="checkbox-default"
                    bgcolor="{{ Theme.UI_Checkbox_Default_Bg }}"
                    textcolor="{{ Theme.UI_Checkbox_Default_Text }}"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    bgcolorfocus="{{ Theme.UI_Checkbox_Default_BgFocus }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />
            <style
                    class="checkbox-outline-default"
                    bgcolor="{{ Theme.UI_Checkbox_Default_Border }}"
                    bgcolorfocus="{{ Theme.UI_Checkbox_Default_Border }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />
            
            <!-- Radio Button -->
            <style 
                    class="radiobutton-default"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_RadioButton_Default_Text }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />
            
            <!-- Window -->
            <style 
                    class="window-bg-default"
                    bgcolor="{{ Theme.UI_Window_Default_Bg }}"
            />

            <style
                    class="window-bg-secondary"
                    bgcolor="{{ Theme.UI_Window_Secondary_Bg }}"
            />
            
            <style 
                    class="window-header-default"
                    bgcolor="{{ Theme.UI_Window_Default_Header_Bg }}"
                    bgcolorfocus="{{ Theme.UI_Window_Default_Header_BgFocus }}"
            />

            <style
                    class="window-header-secondary"
                    bgcolor="{{ Theme.UI_Window_Secondary_Header_Bg }}"
                    bgcolorfocus="{{ Theme.UI_Window_Secondary_Header_BgFocus }}"
            />
            
            <style
                    class="window-title-default"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Window_Default_Title_Text }}"
            />

            <style
                    class="window-title-secondary"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Window_Secondary_Title_Text }}"
            />
            
            <style 
                    class="window-closebtn-default"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Window_Default_CloseBtn_Text }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />

            <style
                    class="window-closebtn-secondary"
                    textsize="{{ Theme.UI_FontSize }}"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Window_Secondary_CloseBtn_Text }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />

            <style
                    class="window-minimizebtn-default"
                    textsize="2"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Window_Default_MinimizeBtn_Text }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />

            <style
                    class="window-minimizebtn-secdonary"
                    textsize="2"
                    textfont="{{ Theme.UI_Font }}"
                    textcolor="{{ Theme.UI_Window_Secondary_MinimizeBtn_Text }}"
                    focusareacolor1="00000000"
                    focusareacolor2="00000000"
            />
        </stylesheet>
    </template>
</component>
