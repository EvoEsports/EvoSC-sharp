<component>
    <property type="double" name="width" default="36"/>
    <property type="string" name="position" default="right"/>
    
    <template>
        <frame>
            <quad pos='{{ position == "right" ? width + 0.7 : 0 }}'
                  bgcolor="{{ Theme.UI_AccentPrimary }}"
                  size="0.7 9"
                  halign='{{ position }}'
            />
            <Panel
                    width="{{ width }}"
                    height="9"
                    x='{{ position == "right" ? 0 : 0.7 }}'
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