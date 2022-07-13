# Plugin Dev Environment

## Setup
1. Create a new **Class Library** project
2. Inside the .csproj file, paste the following contents to reference the main project:
```xml
<ItemGroup>
  <ProjectReference Include="..\..\EvoSC\EvoSC.csproj">
      <ExcludeAssets>runtime</ExcludeAssets>
      <Private>false</Private>
  </ProjectReference>
</ItemGroup>
```
3. Create the plugin class
4. Build the project and put the entire output in it's own folder under MainProject/plugins/
5. (Windows Only) To set up automatic copying of the build output, you can add the following to the .csproj file:
```xml
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
  <Exec Command="mkdir $(SolutionDir)EvoSC\plugins\$(ProjectName)&#xA;xcopy $(OutDir)* $(SolutionDir)EvoSC\plugins\$(ProjectName)\ /Y /I /E" />
</Target>

<Target Name="PreBuild" BeforeTargets="PreBuildEvent">
  <Exec Command="rmdir $(SolutionDir)EvoSC\plugins\$(ProjectName)" />
</Target>
```