﻿<component>
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
    <import component="EvoSC.Controls.Button" as="Button" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
    <import component="EvoSC.Controls.TextInput" as="TextInput" />
    <import component="EvoSC.Controls.Switch" as="Switch" />
    <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
    <import component="EvoSC.Controls.RadioButton" as="RadioButton" />
    <import component="EvoSC.Controls.Alert" as="Alert" />
    <import component="EvoSC.Containers.Window" as="Window" />
    <import component="EvoSC.Containers.Container" as="Container" />
    <import component="EvoSC.Controls.Panel" as="Panel" />
    <import component="EvoSC.Drawing.QuarterCircle" as="QuarterCircle" />
    <import component="EvoSC.Controls.Chip" as="Chip" />
    <import component="EvoSC.Controls.Separator" as="Separator" />
    <import component="EvoSC.Controls.Select" as="Select" />
    <import component="EvoSC.Controls.SelectItem" as="SelectItem" />
    <import component="EvoSC.Containers.Widget" as="Widget" />
    
    <template>
        <!-- <Theme /> -->
        <!-- <Dropdown text="Dropdown" id="myDropdown" x="10" y="20">
            <Button text="Normal" id="myAction1" y="0" />
            <Button text="Secondary" id="myAction2" y="-5" type="secondary" />
            <Button text="Disabled" id="myAction3" y="-10" disabled="true" />
            <IconButton icon="" text="Icon" id="myAction4" y="-15" />
        </Dropdown> -->
        
        <!-- <TextInput id="myinput" value="something" /> -->
        
        <!-- <Switch value="false" id="switch1" />
        <Switch value="false" y="-6" id="switch2" /> -->
        
        <!-- <Checkbox id="mycheck" text="Check this!" />
        <Checkbox id="mycheck2" text="Check this!" isChecked="true" y="-4" /> -->
        
        
        <!-- <RadioButton id="radio1" text="Group 1 Btn 1" group="group1" />
        <RadioButton id="radio2" text="Group 1 Btn 2" group="group1" y="-4" />
        <RadioButton id="radio3" text="Group 2 Btn 1" group="group2" y="-18" />
        <RadioButton id="radio4" text="Group 2 Btn 2" group="group2" y="-22" />
        <RadioButton id="radio5" text="Group 2 Btn 3" group="group2" y="-26" /> -->
        
        <!-- <Window style="secondary" hasTitlebar="false">
            <label text="test" />
        </Window> -->
        
        <!-- <Window>
          <Container width="50" height="20" id="test" scrollable="true" scrollHeight="5">
            <label text="hello 1" pos="0 0" />
            <label text="hello 2" pos="0 -5" />
            <label text="hello 3" pos="0 -10" />
            <label text="hello 4" pos="0 -15" />
            <label text="hello 5" pos="0 -20" />
          </Container>
        </Window> -->
      
        <!-- <Panel width="15" height="5" bgColor="ffffff44" cornerRadius="2" borderColor="0000ff">
          <label text="hello" />
        </Panel> -->

      
      <!-- <Window> -->
        <!-- <Chip text="TEST" closable="true" /> -->
        <!-- <Select>
          <SelectItem text="Fullspeed" value="fs" />
          <SelectItem text="Tech" value="tech" />
          <SelectItem text="Dirt" value="dirt" />
          <SelectItem text="Ice" value="ice" />
        </Select> -->
      <!-- </Window> -->
      
      <!-- <Window>
        <Chip text="TEST" closable="true" severity="primary" y="0" />
        <Chip text="TEST" closable="true" severity="secondary" y="-5" />
        <Chip text="TEST" closable="true" severity="success" y="-10" />
        <Chip text="TEST" closable="true" severity="info" y="-15" />
        <Chip text="TEST" closable="true" severity="warning" y="-20" />
        <Chip text="TEST" closable="true" severity="danger" y="-25" />
      </Window> -->


      <!-- <quad bgcolor="ff0058" size="100 5" />
      <quad 
              size="100 50"
              pos="0 -5"
              image="file:///Media/Painter/Stencils/48-ZGrafik4/Brush.tga"
              modulatecolor="1d1c21"
      />
      <quad pos="0 -5" bgcolor="161519" size="100 50" /> -->
      
      <Widget header="Live Ranking" height="20" x="0" y="9" />
      <Widget header="Live Ranking" height="20" x="-1" y="8" />
      <Widget header="Live Ranking" height="20" x="-2" y="7" />
      <Widget header="Live Ranking" height="20" x="-3" y="6" />
      <Widget header="Live Ranking" height="20" x="-4" y="5" />
      <Widget header="Live Ranking" height="20" x="-5" y="4" />
      <Widget header="Live Ranking" height="20" x="-6" y="3" />
      <Widget header="Live Ranking" height="20" x="-7" y="2" />
      <Widget header="Live Ranking" height="20" x="-8" y="1" />
      <Widget header="Live Ranking" height="20" x="-9" y="0" />
      <Widget header="Live Ranking" height="20" x="-10" y="-1" />
      <Widget header="Live Ranking" height="20" x="-11" y="-2" />
      <Widget header="Live Ranking" height="20" x="-12" y="-3" />
      
    </template>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
