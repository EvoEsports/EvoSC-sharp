using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using Castle.DynamicProxy;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Util;

namespace EvoSC.Modules.Tests;

/// <summary>
/// Tests for <see cref="CollectibleConfigProxy"/>, the emitter that makes Config.Net settings work
/// for modules whose settings interface lives in a collectible load context.
/// </summary>
[Collection("ModuleSystem")]
public class CollectibleConfigProxyTests
{
    /// <summary>
    /// A module's settings interface is defined in its own collectible load context, so the proxy
    /// has to be emitted there as well. The runtime refuses a proxy from a non-collectible
    /// assembly, which is what Config.Net's default emitter produces.
    /// </summary>
    [Fact]
    public void Create_MakesTheSettingsInterfaceUsable()
    {
        var context = new AssemblyLoadContext("settings-module", isCollectible: true);
        var settingsInterface = DefineSettingsInterface(context);

        var proxy = CollectibleConfigProxy.Create(settingsInterface, new StubInterceptor());

        Assert.IsAssignableFrom(settingsInterface, proxy);
    }

    [Fact]
    public void Create_ReadsAndWritesThroughTheInterceptor()
    {
        var context = new AssemblyLoadContext("settings-module", isCollectible: true);
        var settingsInterface = DefineSettingsInterface(context);
        var interceptor = new StubInterceptor();

        var proxy = CollectibleConfigProxy.Create(settingsInterface, interceptor);

        var round = settingsInterface.GetMethod("GetRound")!;
        var setRound = settingsInterface.GetMethod("SetRound")!;

        Assert.Equal(7, setRound.Invoke(proxy, [7]));
        Assert.Equal(7, round.Invoke(proxy, []));
        Assert.Equal([7], interceptor.Written);
    }

    [Fact]
    public void Create_RejectsAnInterfaceWithByRefMembers()
    {
        var context = new AssemblyLoadContext("settings-module", isCollectible: true);
        var settingsInterface = DefineSettingsInterface(context, byRefMember: true);

        var exception = Assert.Throws<EvoScModuleException>(() =>
            CollectibleConfigProxy.Create(settingsInterface, new StubInterceptor()));

        Assert.Contains("by-ref", exception.Message);
    }

    /// <summary>
    /// Defines a settings interface inside a collectible load context, the way a module assembly
    /// provides one.
    /// </summary>
    private static Type DefineSettingsInterface(AssemblyLoadContext context, bool byRefMember = false)
    {
        context.EnterContextualReflection();
        AssemblyBuilder assembly;
        try
        {
            assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("TestModule.Settings"), AssemblyBuilderAccess.RunAndCollect);
        }
        finally
        {
            AssemblyLoadContext.Default.EnterContextualReflection();
        }

        var module = assembly.DefineDynamicModule("TestModule");
        var typeBuilder = module.DefineType("ISettings",
            TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);

        var round = typeBuilder.DefineProperty("Round", PropertyAttributes.None, typeof(int), Type.EmptyTypes);
        round.SetGetMethod(typeBuilder.DefineMethod("get_Round",
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
            MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Abstract,
            typeof(int), Type.EmptyTypes));
        round.SetSetMethod(typeBuilder.DefineMethod("set_Round",
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
            MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Abstract,
            null, [typeof(int)]));

        if (byRefMember)
        {
            typeBuilder.DefineMethod("Adjust",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                MethodAttributes.HideBySig | MethodAttributes.Abstract,
                typeof(void), [typeof(int).MakeByRefType()]);
        }
        else
        {
            typeBuilder.DefineMethod("GetRound",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                MethodAttributes.HideBySig | MethodAttributes.Abstract,
                typeof(int), Type.EmptyTypes);
            typeBuilder.DefineMethod("SetRound",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                MethodAttributes.HideBySig | MethodAttributes.Abstract,
                typeof(int), [typeof(int)]);
        }

        return typeBuilder.CreateTypeInfo()!.AsType();
    }

    /// <summary>Stands in for Config.Net's interceptor to observe what the proxy forwards.</summary>
    private sealed class StubInterceptor : IInterceptor
    {
        public List<object?> Written { get; } = [];

        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "SetRound":
                    Written.Add(invocation.Arguments[0]);
                    invocation.ReturnValue = invocation.Arguments[0];
                    break;
                case "get_Round":
                case "GetRound":
                    invocation.ReturnValue = Written.Count > 0 ? Written[^1] : 0;
                    break;
            }
        }
    }
}
