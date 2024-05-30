<component>
  <using namespace="EvoSC.Modules.Official.MapListModule.Interfaces.Models" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="MapListModule.Parts.MapRow" as="MapRow" />
  
  <property type="IEnumerable<IMapListMap>" name="maps" />
  <property type="bool" name="canRemoveMaps" />
  
  <template>
    <UIStyle />
    
    <Window title="Maps" 
            icon="{{ Icons.Map }}"
            width="100"
            height="60"
    >
      <Container scrollable="true"
                 scrollHeight="{{ maps.Count()*8 }}"
                 width="96"
                 height="60"
      >
        <MapRow 
                foreach="IMapListMap map in maps" 
                map="{{ map }}" 
                y="{{ -__index*8 }}" 
                width="94" 
                index="{{ __index }}" 
                canRemoveMaps="{{ canRemoveMaps }}"
        />
      </Container>
    </Window>
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
