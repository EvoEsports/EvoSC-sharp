<component>
  <property type="string" name="id" />
  <property type="string" name="forFrame" default="" />
  
  <property type="double" name="x" default="0" />
  <property type="double" name="y" default="0" />
  <property type="double" name="min" default="0" />
  <property type="double" name="max" default="20" />
  <property type="double" name="value" default="0" />
  <property type="double" name="length" default="20" />
  
  <property type="string" name="direction" default="vertical" />
  <property type="int" name="zIndex" default="0" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" id="{{ id }}">
      <quad
              if='direction == "vertical"'
              size="2 5"
              bgcolor="{{ Theme.UI_BgPrimary }}"
              scriptevents="1"
      />
    </frame>
  </template>
  
  <script><!--
  *** OnInitialization ***
  ***
  declare CMlFrame scrollFrame_{{ forFrame }} <=> (Page.MainFrame.GetFirstChild("{{ forFrame }}") as CMlFrame);
  ***
  
  *** OnLoop ***
  ***
  {
    declare scrollBarPos = scrollFrame_{{ forFrame }}.ScrollOffset.Y/{{ max }}*{{ length-5 }};
    declare scrollBarQuad <=> (Page.MainFrame.GetFirstChild("{{ id }}") as CMlFrame).Controls[0];
    scrollBarQuad.RelativePosition_V3 = <scrollBarQuad.RelativePosition_V3.X, -scrollBarPos>;
  }
  ***
  --></script>
</component>
