<component>
    <template>
        <frame pos="0 0">
            <quad pos="0 0" size="30 30" bgcolor="000000" opacity="0.5" />
            <label text="Username" pos="0 0" />
            <quad pos="0 -5" size="20 5" bgcolor="000000" opacity="0.5" />
            <entry pos="0 -5" name="Username" size="20 5" />
            <label text="Password" pos="0 -10"/>
            <quad pos="0 -15" size="20 5" bgcolor="000000" opacity="0.5" />
            <entry pos="0 -15" name="Password" size="20 5" />
            <label pos="0 -20" text="Login" action="test/login" />
        </frame>
    </template>
</component>