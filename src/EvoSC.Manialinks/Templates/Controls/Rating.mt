<!--
Show a set of parts (typically stars) to visualize the rating
of something.
-->
<component>
  <property type="string" name="id" default="evosc-rating" />
  
  <!-- Minimum value -->
  <property type="double" name="min" default="0" />

  <!-- Maximum value -->
  <property type="double" name="max" default="100" />

  <!-- Value to set the rating component to -->
  <property type="double" name="value" default="50" />

  <!-- X location of the rating -->
  <property type="double" name="x" default="0" />

  <!-- Y location of the rating -->
  <property type="double" name="y" default="0" />

  <!-- Number of parts to show -->
  <property type="int" name="parts" default="5" />

  <!-- The part icon to use for the "empty" state -->
  <property type="string" name="partEmpty" default="" />

  <!-- The "filled" part state. It is recommended that the shape of this fits partEmpty -->
  <property type="string" name="partFull" default="" />
  
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
              textcolor="{{ Theme.UI_SurfaceBgPrimary }}"
              text="{{ partEmpty }}"
              textsize="2"
      />
      <frame size="{{ ((value-min)/(max-min))*(4*parts-0.6) }} 5">
        <label
                foreach="int i in Util.Range(parts)"
                pos="{{ i*4 }} 0"
                textfont="{{ Font.Regular }}"
                textcolor="{{ Theme.UI_SurfaceBgSecondary }}"
                text="{{ partEmpty }}"
                autoscale="0"
                textsize="2"
        />
      </frame>
    </frame>
  </template>
</component>
