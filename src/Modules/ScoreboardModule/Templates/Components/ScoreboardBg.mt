<component>
    <property type="double" name="width"/>
    <property type="double" name="height"/>

    <template>
        <quad if='Theme.ScoreboardModule_Background_Image != ""'
              size="{{ width }} {{ height }}"
              image="{{ Theme.ScoreboardModule_Background_Image }}"
              opacity="{{ Theme.ScoreboardModule_Background_Opacity }}"
              keepratio="Fill"
        />
    </template>
</component>
