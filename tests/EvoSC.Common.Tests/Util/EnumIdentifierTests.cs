using System;
using System.Linq;
using EvoSC.Common.Util.EnumIdentifier;
using MySql.Data.MySqlClient;
using Xunit;

namespace EvoSC.Common.Tests.Util;

public class EnumIdentifierTests
{
    public enum MyEnum
    {
        [Identifier(Name = "MyCustomIdentifierName")]
        [Alias(Name = "MyAlias")]
        MyIdentifier,
        
        MyIdentifier2,
        
        [Identifier(NoPrefix = true)]
        MyIdentifier3
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

    [Fact]
    public void Get_Enum_Alias()
    {
        var aliases = MyEnum.MyIdentifier.GetAliases();

        var alias = aliases.FirstOrDefault();
        
        Assert.NotNull(alias);
        Assert.Equal("MyAlias", alias);
    }

    [Fact]
    public void Throw_If_Enum_Is_Null()
    {
        Enum myEnum = null;

        Assert.Throws<InvalidOperationException>(() =>
        {
            myEnum.GetIdentifier();
        });
    }

    [Fact]
    public void Get_Name_Without_Prefix()
    {
        var idString = MyEnum.MyIdentifier3.GetIdentifier();
        
        Assert.Equal("MyIdentifier3", idString);
    }
}
