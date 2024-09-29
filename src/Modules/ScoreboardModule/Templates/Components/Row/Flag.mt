<component>
    <property type="double" name="height" />
    
    <template>
        <quad id="flag"
              size="{{ height * 1.5 }} {{ height * 0.75 }}"
              valign="center"
              alphamask="{{ Theme.ScoreboardModule_Background_Row_Flag_AlphaMaskUrl }}"
        />
    </template>
</component>
