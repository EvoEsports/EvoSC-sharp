using System;
using System.Linq;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;
using Xunit;

namespace EvoSC.Common.Tests;

public class ReflectionUtilTests
{
    private class GenericCustomDummyClass
    {
    }
    
    [Controller]
    private class MyController : EvoScController<IControllerContext>
    {
    }

    private class ClassWithMethods
    {
        public string MyMethod(string arg) => $"Success: {arg}";
        
        public static string MyStaticMethod(string arg) => $"Static Success: {arg}";
    }

    public class ReflectionUtilTestsAttributeAttribute : Attribute
    {
    }
    
    public class ReflectionUtilTestsAttributeAttribute2 : Attribute
    {
    }

    [ReflectionUtilTestsAttribute]
    public class MyClassWithAttribute
    {
    }

    [Theory]
    [InlineData(typeof(GenericCustomDummyClass), false)]
    [InlineData(typeof(MyController), true)]
    public void Controller_Class_Check_Is_Correct(Type type, bool expected)
    {
        var isController = type.IsControllerClass();
        
        Assert.Equal(expected, isController);
    }

    [Theory]
    [InlineData(typeof(string), "")]
    [InlineData(typeof(int), 0)]
    [InlineData(typeof(uint), 0)]
    [InlineData(typeof(short), 0)]
    [InlineData(typeof(ushort), 0)]
    [InlineData(typeof(long), 0)]
    [InlineData(typeof(ulong), 0)]
    [InlineData(typeof(float), 0.0)]
    [InlineData(typeof(double), 0.0)]
    [InlineData(typeof(int[]), new int[]{})]
    [InlineData(typeof(string[]), new int[]{})]
    public void Correct_Default_Type_Value_Is_Returned(Type type, object expected)
    {
        var value = type.GetDefaultTypeValue();
        
        Assert.Equal(expected, value);
    }

    [Fact]
    public void Custom_Class_Default_Value()
    {
        var value = typeof(GenericCustomDummyClass).GetDefaultTypeValue()?.GetType();

        Assert.Equal(typeof(GenericCustomDummyClass), value);
    }

    [Fact]
    public void Method_Called_On_Instance()
    {
        var instance = new ClassWithMethods();

        var value = ReflectionUtils.CallMethod(typeof(ClassWithMethods), instance, "MyMethod", "Test");
        
        Assert.Equal("Success: Test", value);
    }

    [Fact]
    public void Unknown_Method_On_Instance_Throws_Exception()
    {
        var instance = new ClassWithMethods();
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            ReflectionUtils.CallMethod(typeof(ClassWithMethods), instance, "SomeUnknownMethod");
        });
    }

    [Fact]
    public void Static_Method_Called()
    {
        var value = ReflectionUtils.CallStaticMethod(typeof(ClassWithMethods), "MyStaticMethod", "Test");
        
        Assert.Equal("Static Success: Test", value);
    }
    
    [Fact]
    public void Unknown_Static_Method_Throws_Exception()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            ReflectionUtils.CallStaticMethod(typeof(ClassWithMethods), "SomeUnknownMethod");
        });
    }

    [Fact]
    public void Find_Assembly_Types_With_Attribute()
    {
        var attributes = this.GetType().Assembly.AssemblyTypesWithAttribute<ReflectionUtilTestsAttributeAttribute>();
        var count = attributes.Count();
        
        Assert.Equal(1, count);
    }
    
    [Fact]
    public void No_Types_With_Custom_Attribute_Found_In_Assembly()
    {
        var attributes = this.GetType().Assembly.AssemblyTypesWithAttribute<ReflectionUtilTestsAttributeAttribute2>();
        var count = attributes.Count();
        
        Assert.Equal(0, count);
    }

    [Fact]
    public void Instance_Method_Returned()
    {
        var instance = new ClassWithMethods();

        var method = instance.GetInstanceMethod("MyMethod");
        
        Assert.Equal("System.String MyMethod(System.String)", method.ToString());
    }
    
    [Fact]
    public void Instance_Method_Not_Found_Throws_Exception()
    {
        var instance = new ClassWithMethods();

        Assert.Throws<InvalidOperationException>(() =>
        {
            instance.GetInstanceMethod("SomeUnknownMethod");
        });
    }
}
