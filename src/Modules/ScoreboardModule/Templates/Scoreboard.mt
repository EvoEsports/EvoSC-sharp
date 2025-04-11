<component>
    <using namespace="EvoSC.Modules.Official.ScoreboardModule.Config"/>
    <using namespace="System.Linq"/>

    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="ScoreboardModule.Components.ScoreboardHeader" as="ScoreboardHeader"/>
    <import component="ScoreboardModule.Components.ScoreboardBody" as="Body"/>
    <import component="ScoreboardModule.Components.ScoreboardBg" as="ScoreboardBg"/>
    <import component="ScoreboardModule.Components.Row.PlayerRowFramemodel" as="PlayerRowFramemodel"/>

    <property type="IScoreboardSettings" name="settings"/>
    <property type="int" name="MaxPlayers" default="0"/>

    <property type="double" name="backgroundBorderRadius" default="3f"/>
    <property type="double" name="headerHeight" default="14f"/>
    <property type="double" name="rowHeight" default="8f"/>
    <property type="double" name="rowInnerHeight" default="5f"/>
    <property type="double" name="rowSpacing" default="0f"/>
    <property type="double" name="columnSpacing" default="4f"/>
    <property type="double" name="pointsWidth" default="16f"/>
    <property type="double" name="padding" default="2f"/>
    <property type="double" name="innerSpacing" default="1.6"/>
    <property type="double" name="legendHeight" default="3.8"/>
    <property type="int" name="actionButtonCount" default="2"/>

    <template layer="ScoresTable">
        <!-- UI Styles -->
        <UIStyle/>

        <!-- Frame Models -->
        <PlayerRowFramemodel w="{{ settings.Width }}"
                             padding="{{ padding }}"
                             rowHeight="{{ rowHeight }}"
                             rowSpacing="{{ rowSpacing }}"
                             columnSpacing="{{ columnSpacing }}"
                             innerSpacing="{{ innerSpacing }}"
                             rowInnerHeight="{{ rowInnerHeight }}"
                             pointsWidth="{{ pointsWidth }}"
                             actionButtonCount="{{ actionButtonCount }}"
                             settings="{{ settings }}"
        />

        <!-- Scoreboard Content -->
        <frame pos="{{ settings.Width / -2f }} {{ settings.Height / 2f }}">
            <!-- Background -->
            <ScoreboardBg
                    width="{{ settings.Width }}"
                    height="{{ settings.Height }}"
            />

            <!-- Header -->
            <ScoreboardHeader
                    width="{{ settings.Width }}"
                    height="{{ headerHeight }}"
            />

            <!-- Body -->
            <Body y="{{ -headerHeight }}"
                  width="{{ settings.Width }}"
                  height="{{ settings.Height - headerHeight }}"
                  legendHeight="{{ legendHeight }}"
                  rowSpacing="{{ rowSpacing }}"
                  columnSpacing="{{ columnSpacing }}"
                  flagWidth="{{ rowInnerHeight * 1.5 }}"
                  clubTagWidth="{{ rowInnerHeight * 1.5 }}"
            />

            <!-- Player Rows -->
            <frame id="rows_wrapper"
                   pos="0 {{ -headerHeight-legendHeight }}"
                   size="{{ settings.Width }} {{ settings.Height-headerHeight }}"
            >
                <frame id="rows_inner">
                    <frame id="frame_scroll"
                           size="{{ settings.Width }} {{ settings.Height-headerHeight-legendHeight - 0.1 }}">
                        <frameinstance modelid="player_row"
                                       foreach="int rowId in Enumerable.Range(0, MaxPlayers * 2).ToList()"
                                       pos="0 {{ rowId * -rowHeight + (rowId+1) * -rowSpacing }}"
                        />
                    </frame>
                </frame>
            </frame>
        </frame>
    </template>

    <script resource="ScoreboardModule.Scripts.Scoreboard" once="true"/>
    <script resource="EvoSC.Scripts.UIScripts"/>
</component>
