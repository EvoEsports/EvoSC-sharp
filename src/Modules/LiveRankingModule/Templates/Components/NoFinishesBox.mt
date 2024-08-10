<component>
    <using namespace="EvoSC.Common.Util.Manialinks"/>
    
    <property type="double" name="width" default="36"/>
    <property type="WidgetPosition" name="position" default="WidgetPosition.Right"/>
    
    <template>
        <frame>
            <quad pos='{{ position == WidgetPosition.Right ? width + 0.7 : 0 }}'
                  bgcolor="{{ Theme.UI_AccentPrimary }}"
                  size="0.7 9"
                  halign='{{ position == WidgetPosition.Right ? "right" : "left" }}'
            />
            <Panel
                    width="{{ width }}"
                    height="9"
                    x='{{ position == WidgetPosition.Right ? 0 : 0.7 }}'
                    className="lr-body-primary"
                    bgColor=""
            >
                <label
                        class="text-primary"
                        text="No finishes"
                        valign="center"
                        halign="center"
                        pos="18 -4.2"
                        size="{{ width }} 9"
                />
            </Panel>
        </frame>
    </template>
</component>