<component>
    <property type="double" name="width" />
    <property type="double" name="height" />

    <template>
        <quad size="{{ width }} {{ height }}"
              bgcolor="{{ Theme.ScoreboardModule_Background_Header_Color }}"
              opacity="{{ Theme.ScoreboardModule_Background_Header_Opacity }}"
        />
    </template>
</component>
