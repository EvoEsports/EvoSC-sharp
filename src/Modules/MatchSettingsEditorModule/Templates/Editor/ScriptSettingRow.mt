<component>
  <import component="EvoSC.Controls.Text" as="Text" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  
  <property type="string" name="name" />
  <property type="Type" name="type" />
  <property type="string" name="value" />
  
  <property type="double" name="y" />
  <property type="double" name="width" />
  
  <template>
    <frame pos="0 {{ y }}" size="{{ width }} 5">
      <label class="text-primary" valign="center" pos="0 -2.5" text="{{ name }}" />
      <TextInput width="60" id="{{ name }}" value="{{ value }}" x="{{ width - 60 }}" />
    </frame>
  </template>
</component>