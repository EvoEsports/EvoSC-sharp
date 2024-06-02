<!--
Draws a rectangle.
-->
<component>
  <import component="EvoSC.Drawing.QuarterCircle" as="QuarterCircle" />

  <!-- Unique identifier of the rectangle -->
  <property type="string" name="id" default="evosc_rectangle" />

  <!-- Corner radius of the rectangle for rounded corners -->
  <property type="double" name="cornerRadius" default="0" />

  <!-- X location of the rectangle -->
  <property type="double" name="x" default="0.0" />

  <!-- Y location of the rectangle -->
  <property type="double" name="y" default="0.0" />

  <!-- Width of the rectangle -->
  <property type="double" name="width" />

  <!-- Height of the rectangle -->
  <property type="double" name="height" />

  <!-- Enable/disable script events of the rectangle -->
  <property type="bool" name="scriptEvents" default="false" />

  <!-- Action to trigger when clicking the rectawngle -->
  <property type="string" name="action" default="" />

  <!-- Background color of the rectangle -->
  <property type="string" name="bgColor" default="00000000" />

  <!-- Z index of the rectangle -->
  <property type="double" name="zIndex" default="0" />

  <!-- Whether to hide the rectangle by default -->
  <property type="bool" name="hidden" default="false" />

  <!-- Rotation of the rectangle in degrees -->
  <property type="double" name="rotate" default="0" />
  
  <!-- Corners to round off, can be a combined comma-separated list of the following values: TopLeft, TopRight, BottomLeft or BottomRight -->
  <property type="string" name="corners" default="TopLeft,TopRight,BottomLeft,BottomRight" />

  <!-- Styling class to pass to the rectangle -->
  <property type="string" name="className" default="" />
  
  <!-- Data Id attribute to set -->
  <property type="string" name="dataId" default="" />
  
  <template>
    <frame pos="{{ x }} {{ y }}" 
           size="{{ width }} {{ height }}"
           id="{{ id }}"
           rot="{{ rotate }}"
           z-index="{{ zIndex }}"
           class="{{ className }}"
           hidden='{{ hidden ? "1" : "0" }}'
           scriptevents='{{ scriptEvents ? "1" : "0" }}'
           data-id="{{ dataId }}"
    >
        <frame>
          <frame if="cornerRadius > 0">
            <QuarterCircle scriptevents="{{ scriptEvents ? 1 : 0 }}" if='Util.HasItem(corners, "TopLeft")' quadrant="TopLeft" radius="{{ cornerRadius }}" x="0" y="0" color="{{ bgColor }}" data-id="{{ dataId }}" />
            <QuarterCircle scriptevents="{{ scriptEvents ? 1 : 0 }}" if='Util.HasItem(corners, "TopRight")' quadrant="TopRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="0" color="{{ bgColor }}" data-id="{{ dataId }}" />
            <QuarterCircle scriptevents="{{ scriptEvents ? 1 : 0 }}" if='Util.HasItem(corners, "BottomLeft")' quadrant="BottomLeft" radius="{{ cornerRadius }}" x="0" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" data-id="{{ dataId }}" />
            <QuarterCircle scriptevents="{{ scriptEvents ? 1 : 0 }}" if='Util.HasItem(corners, "BottomRight")' quadrant="BottomRight" radius="{{ cornerRadius }}" x="{{ width-cornerRadius }}" y="-{{ height-cornerRadius }}" color="{{ bgColor }}" data-id="{{ dataId }}" />
          </frame>

          <!-- Top -->
          <quad bgcolor="{{ bgColor }}" 
                pos='{{ cornerRadius - (Util.HasItem(corners, "TopLeft") ? 0 : cornerRadius) }} 0'
                size='{{ width-cornerRadius*2 + (Util.HasItem(corners, "TopLeft") ? 0 : cornerRadius) + (Util.HasItem(corners, "TopRight") ? 0 : cornerRadius) }} {{ cornerRadius }}'
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
          />

          <!-- Left -->
          <quad bgcolor="{{ bgColor }}" 
                pos='0 -{{ cornerRadius }}'
                size='{{ cornerRadius }} {{ height-cornerRadius*2 }}'
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
          />

          <!-- Bottom -->
          <quad bgcolor="{{ bgColor }}" 
                pos='{{ cornerRadius - (Util.HasItem(corners, "BottomLeft") ? 0 : cornerRadius) }} -{{ height-cornerRadius }}'
                size='{{ width-cornerRadius*2 + (Util.HasItem(corners, "BottomLeft") ? 0 : cornerRadius) + (Util.HasItem(corners, "BottomRight") ? 0 : cornerRadius) }} {{ cornerRadius }}'
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
          />

          <!-- Right -->
          <quad bgcolor="{{ bgColor }}" 
                pos='{{ width-cornerRadius }} -{{ cornerRadius }}'
                size='{{ cornerRadius }} {{ height-cornerRadius*2 }}'
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
          />
        </frame>

        <quad
                pos="{{ cornerRadius }} {{ -cornerRadius }}"
                size="{{ width-cornerRadius*2 }} {{ height-cornerRadius*2 }}"
                bgcolor="{{ bgColor }}"
                scriptevents="{{ scriptEvents ? 1 : 0 }}"
                data-id="{{ dataId }}"
        />
    </frame>
  </template>
</component>