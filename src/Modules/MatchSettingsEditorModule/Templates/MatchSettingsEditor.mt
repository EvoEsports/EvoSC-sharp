<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <property type="IEnumerable<IMatchSettings>" name="matchSettings" />
  
  <template>
    <UIStyle />
    
    <Window title="Match Settings"
            icon="{{ Icons.Gears }}"
            width="100"
            height="60"
    >
      
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
