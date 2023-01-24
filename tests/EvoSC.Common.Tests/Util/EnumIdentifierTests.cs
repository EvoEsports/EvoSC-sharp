using System;
using EvoSC.Common.Util.EnumIdentifier;
using Xunit;

namespace EvoSC.Common.Tests.Util;

public class EnumIdentifierTests
{
    public enum MyEnum
    {
        [Identifier(Name = "MyCustomIdentifierName")]
        MyIdentifier,
        
        MyIdentifier2
    }
    
    [Identifier(Name = "myCustomEnumName")]
    public enum MyEnum2
    {
        SomeEnumValue
    }

    [Fact]
    public void Non_Enum_Throws_Exception()
    {
        object myVar = "test";

        Assert.Throws<InvalidOperationException>(() =>
        {
            myVar.AsEnum();
        });
    }

    [Fact]
    public void Object_Converted_To_Enum()
    {
        object myEnum = MyEnum.MyIdentifier;

        var converted = myEnum.AsEnum();
        var hasFlag = converted?.HasFlag(MyEnum.MyIdentifier) ?? false;
        
        Assert.True(hasFlag);
    }

    [Theory]
    [InlineData(MyEnum.MyIdentifier, "MyEnum.MyCustomIdentifierName")]
    [InlineData(MyEnum.MyIdentifier2, "MyEnum.MyIdentifier2")]
    public void Identifier_Name_Correctly_Converted(MyEnum enumValue, string expected)
    {
        var idString = enumValue.GetIdentifier();
        
        Assert.Equal(expected, idString);
    }

    [Fact]
    public void Custom_Enum_Identifier_Overrides_Default()
    {
        var idString = MyEnum2.SomeEnumValue.GetIdentifier();

        Assert.Equal("myCustomEnumName.SomeEnumValue", idString);
    }
}
