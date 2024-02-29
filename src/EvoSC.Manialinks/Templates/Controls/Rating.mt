<component>
    <property type="string" name="id" default="evosc-rating" />
    
    <property type="double" name="min" default="0" />
    <property type="double" name="max" default="100" />
    <property type="double" name="value" default="50" />
    
    <property type="double" name="x" default="0" />
    <property type="double" name="y" default="0" />
    
    <property type="int" name="parts" default="5" />
    <property type="string" name="partEmpty" default="" />
    <property type="string" name="partHalf" default="" />
    <property type="string" name="partFull" default="" />
    
    <property type="string?" name="bgFilled" default="null" />
    <property type="string?" name="bgEmpty" default="null" />
    
    <template>
        <frame
                id="{{ id }}"
                class="evosc-rating"
                pos="{{ x }} {{ y }}"
        >
          <label
                  foreach="int i in Util.Range(parts)"
                  pos="{{ i*4}} 0"
                  textfont="{{ Font.Regular }}"
                  textcolor="{{ Theme.UI_BgSecondary }}"
                  text="{{ partEmpty }}"
                  textsize="2"
          />
          <frame size="{{ ((value-min)/(max-min))*(4*parts-0.6) }} 5">
            <label
                    foreach="int i in Util.Range(parts)"
                    pos="{{ i*4 }} 0"
                    textfont="{{ Font.Regular }}"
                    textcolor="{{ Theme.UI_BgPrimary }}"
                    text="{{ partEmpty }}"
                    autoscale="0"
                    textsize="2"
            />
          </frame>
        </frame>
    </template>
</component>
