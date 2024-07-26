<component>
  <import component="EvoSC.Style.Controls.ButtonStyle" as="ButtonStyle" />
  <import component="EvoSC.Style.Controls.DropdownStyle" as="DropdownStyle" />
  <import component="EvoSC.Style.Controls.TextInputStyle" as="TextInputStyle" />
  <import component="EvoSC.Style.Controls.CheckboxStyle" as="CheckboxStyle" />
  <import component="EvoSC.Style.Containers.WindowStyle" as="WindowStyle" />
  
  <template>
    <stylesheet>
      <!-- Text -->
      <style
              class="text-primary"
              textfont="{{ Font.Regular }}"
              textcolor="{{ Theme.UI_TextPrimary }}"
              textsize="{{ Theme.UI_FontSize }}"
      />

      <style
              class="text-secondary"
              textfont="{{ Font.Regular }}"
              textcolor="{{ Theme.UI_TextSecondary }}"
              textsize="{{ Theme.UI_FontSize }}"
      />

      <style
              class="text-muted"
              textfont="{{ Font.Regular }}"
              textcolor="{{ Theme.UI_TextMuted }}"
              textsize="{{ Theme.UI_FontSize }}"
      />

      <style
              class="text-header"
              textfont="{{ Font.Bold }}"
              textcolor="{{ Theme.UI_TextPrimary }}"
              textsize="3"
      />

      <style
              class="text-xs"
              textsize="{{ Theme.UI_FontSize*0.8 }}"
      />

      <style
              class="text-normal"
              textsize="{{ Theme.UI_FontSize }}"
      />

      <style
              class="text-md"
              textsize="{{ Theme.UI_FontSize*1.2 }}"
      />

      <style
              class="text-lg"
              textsize="{{ Theme.UI_FontSize*1.5 }}"
      />

      <style
              class="text-xl"
              textsize="{{ Theme.UI_FontSize*2 }}"
      />

      <style
              class="text-2xl"
              textsize="{{ Theme.UI_FontSize*4 }}"
      />

      <style
              class="text-3xl"
              textsize="{{ Theme.UI_FontSize*6 }}"
      />

      <style
              class="text-4xl"
              textsize="{{ Theme.UI_FontSize*8 }}"
      />

      <!-- Background -->
      <style
              class="bg-header"
              bgcolor="{{ Theme.UI_HeaderBg }}"
              opacity="0.95"
      />

      <style
              class="bg-primary"
              bgcolor="{{ Theme.UI_BgPrimary }}"
              opacity="0.9"
      />

      <style
              class="bg-highlight"
              bgcolor="{{ Theme.UI_BgHighlight }}"
              opacity="0.9"
      />

      <!-- Surface -->
      <style
              class="surface-primary"
              bgcolor="{{ Theme.UI_SurfaceBgPrimary }}"
      />

      <style
              class="surface-secondary"
              bgcolor="{{ Theme.UI_SurfaceBgSecondary }}"
      />

      <!-- Accent -->
      <style
              class="accent-primary"
              bgcolor="{{ Theme.UI_AccentPrimary }}"
      />

      <style
              class="accent-secondary"
              bgcolor="{{ Theme.UI_AccentSecondary }}"
      />
    </stylesheet>
    
    <ButtonStyle />
    <DropdownStyle />
    <TextInputStyle />
    <CheckboxStyle />
    <WindowStyle />
  </template>
</component>
