<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />

  <property type="IOnlinePlayer" name="player" />
  <property type="double" name="x" />
  <property type="double" name="y" />
  
  <template>
    <frame pos="{{ x }} {{ y }}">
      <label class="text-primary" text="{{ player.NickName }}" />
    </frame>
  </template>
</component>
