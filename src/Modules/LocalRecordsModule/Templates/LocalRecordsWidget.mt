<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="EvoSC.Common.Util" />
  <using namespace="EvoSC.Modules.Official.LocalRecordsModule.Interfaces" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Widget" as="Widget" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="EvoSC.Controls.Panel" as="Panel" />
  
  <import component="LocalRecordsModule.Widget.RecordRow" as="RecordRow" />
  <import component="LocalRecordsModule.Style.LocalRecordsStyle" as="LocalRecordsStyle" />
  
  <property type="IEnumerable<ILocalRecord>" name="records" />
  <property type="IPlayer" name="currentPlayer" />
  
  <template>
    <UIStyle />
    <LocalRecordsStyle />
      <Widget header="local records" height="20" bodyStyle="unstyled" y="80">
      <template slot="body">
        <RecordRow 
                pos="{{ record.Position }}"
                name="{{ record.Record.Player.NickName }}" 
                y="{{ -__index*(4+0.3) }}" 
                foreach="ILocalRecord record in records"
                time="{{ RaceTime.FromMilliseconds(record.Record.Score) }}" 
                self="{{ record.Record.Player.Id == currentPlayer.Id }}"
        />
        
        <frame if="records.Count() == 0">
          <quad
                  bgcolor="{{ Theme.UI_AccentPrimary }}"
                  size="0.7 9"
          />
          <Panel
                  width="36"
                  height="9"
                  x="0.7"
                  className="lr-body-primary"
                  bgColor=""
          >
            <label
                    class="text-primary"
                    text="No records for this map"
                    valign="center"
                    halign="center"
                    pos="18 -4.2"
                    size="36 9"
            />
          </Panel>
        </frame>
      </template>
    </Widget>
  </template>
</component>
