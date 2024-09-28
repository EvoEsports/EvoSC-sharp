<component>
    <property type="double" name="x"/>
    <property type="double" name="y"/>

    <template>
        <quad pos="{{ x }} {{ y }}"
              size="{{ Theme.ScoreboardModule_Logo_Width }} {{ Theme.ScoreboardModule_Logo_Height }}"
              valign="center"
              halign="center"
              image="{{ Theme.ScoreboardModule_Logo_URL }}"
              keepratio="Fit"
        />
    </template>
</component>
