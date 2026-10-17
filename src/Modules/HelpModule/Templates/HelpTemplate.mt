<!--
Shows a window with a list of all available chat commands, including the permission
required to run them, if any.
-->
<component>
  <using namespace="EvoSC.Modules.Official.HelpModule.Models" />
  <using namespace="System.Linq" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="HelpModule.Parts.CommandRow" as="CommandRow" />

  <property type="IEnumerable<HelpCommandInfo>" name="commands" />

  <template>
    <UIStyle />

    <Window title="Commands"
            icon="{{ Icons.QuestionCircle }}"
            width="120"
            height="80"
    >
      <Container scrollable="true"
                 scrollHeight="{{ commands.Count()*10 }}"
                 width="116"
                 height="80"
                 id="help-command-list"
      >
        <CommandRow
                foreach="HelpCommandInfo command in commands"
                command="{{ command }}"
                y="{{ -__index*10 }}"
                width="114"
                index="{{ __index }}"
        />
      </Container>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
