<!--
Show a row with a single command's usage, description and required permission.
-->
<component>
  <using namespace="EvoSC.Modules.Official.HelpModule.Models" />

  <property type="HelpCommandInfo" name="command" />

  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="94" />
  <property type="double" name="height" default="10" />
  <property type="int" name="index" default="0" />

  <template>
    <frame pos="{{ x }} {{ y }}">
      <quad pos="0 0"
            size="{{ width }} {{ height }}"
            bgcolor="{{ Theme.UI_BgHighlight }}"
            opacity="{{ index % 2 == 0 ? 0.4 : 0.15 }}"
      />

      <label class="text-primary"
             text="{{ command.Usage }}"
             textsize="{{ Theme.UI_FontSize*1.1 }}"
             pos="1 {{ -1.5 }}"
      />

      <label class="text-primary"
             autonewline="1"
             text="{{ command.Description }}"
             pos="1 {{ -5 }}"
             size="{{ width - 2 }} {{ height - 5 }}"
      />

      <label class="text-secondary"
             text='{{ "Permission: " + command.Permission }}'
             halign="right"
             pos="{{ width - 1 }} {{ -1.5 }}"
             if="command.Permission != null"
      />
    </frame>
  </template>
</component>
