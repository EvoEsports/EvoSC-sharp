<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  
  <import component="EvoSC.Controls.Panel" as="Panel" />
  
  <property type="double" name="width" default="36" />
  <property type="double" name="height" default="4" />
  <property type="double" name="y" default="0" />
  
  <property type="int" name="pos" />
  <property type="string" name="name" />
  <property type="IRaceTime" name="time" />
  <property type="bool" name="self" />
  
  <template>
    <frame pos="0 {{ y }}">
      <quad
              bgcolor="{{ Theme.UI_LocalRecordsModule_Widget_AccentColor(pos) }}"
              size="0.7 {{ height }}"
              pos="0 0"
      />
      <quad
              bgcolor="{{ Theme.UI_AccentSecondary }}"
              size="{{ height }} {{ height }}"
              opacity="0.9"
              pos="0.7 0"
      />
      <label 
              text="{{ pos }}" 
              class="text-primary"
              pos="{{ 0.6 + height/2 }} {{ -height/2.0 + 0.2 }}"
              size="{{ height }} {{ height }}"
              valign="center"
              halign="center"
              textcolor="{{ Theme.Black }}"
      />
      
      <Panel 
              width="{{ width-4 }}" 
              height="4" 
              className='{{ self ? "lr-body-highlight" : "lr-body-primary" }}'
              x="4.7"
              bgColor=""
      >
        <label 
                text="{{ name }}"
                class="text-primary" 
                textsize="0.5"
                pos="1 {{ -height/2.0 + 0.2 }}"
                valign="center"
                size="{{ width-18 }} {{ height }}"
        />

        <label
                text="{{ Util.StyledTime(time) }}"
                class="text-primary"
                textsize="0.5"
                pos="{{ width-16.5 }} {{ -height/2.0 + 0.2 }}"
                valign="center"
                size="16 {{ height }}"
        />
      </Panel>
    </frame>
  </template>
</component>
