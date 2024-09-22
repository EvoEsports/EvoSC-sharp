<component>
    <property type="double" name="x"/>
    <property type="double" name="y"/>
    <property type="double" name="width"/>
    <property type="double" name="height"/>

    <template>
        <quad pos="{{ x }} {{ y }}"
              size="{{ width }} {{ height }}"
              valign="center"
              halign="center"
              image="https://maptesting.evotm.com/images/logo.png"
              keepratio="Fit"
        />
    </template>
</component>
