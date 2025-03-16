<component>
    <property type="double" name="x" default="0f"/>
    <property type="double" name="y" default="0f"/>

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
