<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="EvoSC.Common.Util" />
  
  <import component="EvoSC.Containers.Widget" as="Widget" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <import component="LocalRecordsModule.Widget.RecordRow" as="RecordRow" />
  <import component="LocalRecordsModule.Style.LocalRecordsStyle" as="LocalRecordsStyle" />
  
  <template>
    <UIStyle />
    <LocalRecordsStyle />
    
    <Widget header="local records" height="20" bodyStyle="unstyled" y="80">
      <template slot="body">
        <RecordRow 
                pos="{{ __index + 140 }}"
                name="snixtho" 
                y="{{ -__index*(4+0.3) }}" 
                foreach="int i in Util.Range(10)"
                time="{{ RaceTime.FromMilliseconds(45236) }}" 
                self="{{ __index == 6 }}"
        />
      </template>
    </Widget>
  </template>
</component>
