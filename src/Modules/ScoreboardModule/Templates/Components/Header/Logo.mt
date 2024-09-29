<component>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>

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
