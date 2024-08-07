﻿<!--
    Generic input element for Manialink Forms that can also handle validation errors.
-->
<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <using namespace="System.Linq" />

    <import component="EvoSC.Controls.TextInput" as="TextInput" />

    <property type="IEnumerable<EntryValidationResult>?" name="validationResults" />
    <property type="string?" name="value"/>
    <property type="string" name="name"/>
    <property type="int" name="zIndex" default="0"/>
    <property type="string" name="label" default=""/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="20.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="bool" name="isPassword" default="false"/>
    <property type="string" name="valueType" default="Ml_String"/>
    
    <template>
        <frame pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}">
            <Label
                    text="{{ label }}"
                    class="text-primary"
                    x="0"
                    y="0"
                    textsize="1"
                    halign="left" 
                    valign="top"
            />
            <TextInput 
                    x="0"
                    y="-3"
                    id="{{ name }}"
                    value='{{ value ?? "" }}'
                    width="{{ w }}"
                    isPassword="{{ isPassword }}"
                    valueType="{{ valueType }}"
            />
            <Label 
                    if='validationResults?.Any(v => v.IsInvalid) ?? false'
                    text='$s$e11 {{ validationResults?.FirstOrDefault(v => v.IsInvalid)?.Message ?? "Invalid input." }}'
                    class="text-primary"
                    x="0"
                    y="-10"
                    w="{{ w }}"
                    h="10"
                    autonewline="1"
                    textsize="0.75"
                    halign="left" 
                    valign="top"
            />
        </frame>
    </template>
</component>
