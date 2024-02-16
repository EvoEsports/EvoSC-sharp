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
                    text="{{ Util.RatingParts(min, max, value, parts) &gt;= i ? partFull : partEmpty }}"
                    textcolor="{{ Util.RatingParts(min, max, value, parts) &gt;= i ? Theme.UI_BgPrimary : Theme.UI_BgSecondary }}"
                    textfont="{{ Font.Regular }}"
                    textsize="2"
                    pos="{{ i*4 }} 0"
            />
        </frame>
    </template>
</component>
