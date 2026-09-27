using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Castle.DynamicProxy;
using EvoSC.Modules.Exceptions;

namespace EvoSC.Modules.Util;

/// <summary>
/// Emits an implementation of a settings interface into a collectible assembly, so a module's
/// settings work from a collectible load context.
/// </summary>
/// <remarks>
/// The emitted type implements the interface by handing every call to a delegate supplied at
/// construction, which forwards it to Config.Net's interceptor. Emitting with
/// <see cref="AssemblyBuilderAccess.RunAndCollect"/> is the CLR's supported way to build a type that
/// references a collectible one, and the emitted assembly is attached to the interface's own load
/// context so it unloads together with the module.
/// <para>
/// Castle DynamicProxy, which Config.Net uses by default, cannot do this: it emits into a
/// non-collectible assembly, and the CLR does not allow a non-collectible assembly to reference a
/// collectible one.
/// </para>
/// </remarks>
internal static class CollectibleConfigProxy
{
    /// <summary>
    /// Emitted shapes, keyed weakly by the settings interface so a module's load context can still
    /// be unloaded.
    /// </summary>
    private static readonly ConditionalWeakTable<Type, ProxyShape> Shapes = new();

    /// <summary>
    /// Creates an instance of <paramref name="settingsInterface"/> whose members are handled by
    /// <paramref name="interceptor"/>.
    /// </summary>
    internal static object Create(Type settingsInterface, IInterceptor interceptor)
    {
        var shape = Shapes.GetValue(settingsInterface, static type => ProxyShape.Create(type));
        var dispatch = new Func<MethodInfo, object?[], object?>((method, arguments) =>
            Intercept(interceptor, method, arguments));

        return Activator.CreateInstance(shape.ProxyType, dispatch, shape.Accessors)
               ?? throw new EvoScModuleException($"Failed to create settings for '{settingsInterface.FullName}'.");
    }

    private static object? Intercept(IInterceptor interceptor, MethodInfo method, object?[] arguments)
    {
        var invocation = new InterfaceInvocation(method, arguments);
        interceptor.Intercept(invocation);
        return invocation.ReturnValue;
    }

    /// <summary>
    /// A single call to the interceptor. Mirrors what Castle passes to an interceptor for a proxy
    /// without a target.
    /// </summary>
    private sealed class InterfaceInvocation : IInvocation
    {
        public InterfaceInvocation(MethodInfo method, object?[] arguments)
        {
            Method = method;
            Arguments = arguments;
            ReturnValue = DefaultValue(method.ReturnType);
        }

        public MethodInfo Method { get; }
        public object?[] Arguments { get; }
        public object? ReturnValue { get; set; }
        public object? ProxyObject => null;
        public object? InvocationTarget => null;
        public object? Proxy => null;
        public Type? TargetType => null;
        public MethodInfo MethodInvocationTarget => Method;
        public Type[] GenericArguments => Method.IsGenericMethod ? Method.GetGenericArguments() : [];

        public object? GetArgumentValue(int index) => Arguments[index];

        public void SetArgumentValue(int index, object? value) => Arguments[index] = value;

        public void GetObjectAndMethod(out object? target, out MethodInfo invokedMethod)
        {
            target = null;
            invokedMethod = Method;
        }

        public void Proceed()
        {
            // Nothing to proceed to: the interceptor is the whole implementation.
        }

        public MethodInfo GetConcreteMethod() => Method;

        public MethodInfo GetConcreteMethodInvocationTarget() => Method;

        public IInvocationProceedInfo CaptureProceedInfo() => throw new NotSupportedException();

        private static object? DefaultValue(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    /// <summary>The emitted type together with the member order it was emitted for.</summary>
    private sealed class ProxyShape
    {
        private ProxyShape(Type proxyType, MethodInfo[] accessors)
        {
            ProxyType = proxyType;
            Accessors = accessors;
        }

        internal Type ProxyType { get; }

        internal MethodInfo[] Accessors { get; }

        internal static ProxyShape Create(Type settingsInterface)
        {
            if (!settingsInterface.IsInterface)
            {
                throw new EvoScModuleException(
                    $"Settings type '{settingsInterface.FullName}' must be an interface.");
            }

            var members = CollectMembers(settingsInterface);
            var typeBuilder = DefineType(settingsInterface, members);

            return new ProxyShape(typeBuilder.CreateTypeInfo().AsType(),
                members.Select(member => member.Method).ToArray());
        }

        /// <summary>
        /// The interface's members in a fixed order, so an emitted accessor and the interceptor
        /// agree on which method it is handling.
        /// </summary>
        private static List<ProxyMember> CollectMembers(Type settingsInterface)
        {
            var members = new List<ProxyMember>();

            var properties = settingsInterface.GetProperties()
                .OrderBy(property => property.MetadataToken)
                .ThenBy(property => property.Name, StringComparer.Ordinal);

            foreach (var property in properties)
            {
                var getter = property.GetMethod;
                var setter = property.SetMethod;

                if (getter is not null)
                {
                    members.Add(ProxyMember.ForAccessor(settingsInterface, getter, property.PropertyType, isGetter: true));
                }

                if (setter is not null)
                {
                    members.Add(ProxyMember.ForAccessor(settingsInterface, setter, property.PropertyType, isGetter: false));
                }
            }

            var methods = settingsInterface.GetMethods()
                .Where(method => !method.IsSpecialName)
                .OrderBy(method => method.MetadataToken)
                .ThenBy(method => method.Name, StringComparer.Ordinal);

            foreach (var method in methods)
            {
                members.Add(ProxyMember.ForMethod(settingsInterface, method));
            }

            return members;
        }

        private static TypeBuilder DefineType(Type settingsInterface, IReadOnlyList<ProxyMember> members)
        {
            var loadContext = AssemblyLoadContext.GetLoadContext(settingsInterface.Assembly)
                               ?? AssemblyLoadContext.Default;
            AssemblyBuilder assemblyBuilder;

            // Attaching the emitted assembly to the interface's own context keeps it unloadable with
            // the module instead of pinning the module's types in a context that outlives it.
            loadContext.EnterContextualReflection();
            try
            {
                assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName($"EvoSC.Settings.{settingsInterface.Name}"),
                    AssemblyBuilderAccess.RunAndCollect);
            }
            finally
            {
                AssemblyLoadContext.Default.EnterContextualReflection();
            }

            var module = assemblyBuilder.DefineDynamicModule("EvoSC.Settings");
            var typeBuilder = module.DefineType(
                $"EvoSC.Settings.{settingsInterface.Name}Proxy",
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed);

            var dispatchField = typeBuilder.DefineField("_dispatch", typeof(Func<MethodInfo, object?[], object?>),
                FieldAttributes.Private | FieldAttributes.InitOnly);

            var accessorsField = typeBuilder.DefineField("_accessors", typeof(MethodInfo[]),
                FieldAttributes.Private | FieldAttributes.InitOnly);

            EmitConstructor(typeBuilder, dispatchField, accessorsField);
            EmitProperties(typeBuilder, members, dispatchField, accessorsField);
            EmitMethods(typeBuilder, members, dispatchField, accessorsField);

            typeBuilder.AddInterfaceImplementation(settingsInterface);

            return typeBuilder;
        }

        private static void EmitConstructor(TypeBuilder typeBuilder, FieldBuilder dispatchField, FieldBuilder accessorsField)
        {
            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard,
                [typeof(Func<MethodInfo, object?[], object?>), typeof(MethodInfo[])]);

            var il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, dispatchField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, accessorsField);
            il.Emit(OpCodes.Ret);
        }

        private static void EmitProperties(TypeBuilder typeBuilder,
            IReadOnlyList<ProxyMember> members, FieldBuilder dispatchField, FieldBuilder accessorsField)
        {
            for (var i = 0; i < members.Count; i++)
            {
                var member = members[i];
                if (member.Property is not { } propertyInfo)
                {
                    continue;
                }

                var property = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.None,
                    propertyInfo.PropertyType, Type.EmptyTypes);

                if (member.IsGetter)
                {
                    property.SetGetMethod(EmitAccessor(typeBuilder, member, i, dispatchField, accessorsField,
                        Type.EmptyTypes));
                }
                else
                {
                    property.SetSetMethod(EmitAccessor(typeBuilder, member, i, dispatchField, accessorsField,
                        [propertyInfo.PropertyType]));
                }
            }
        }

        private static void EmitMethods(TypeBuilder typeBuilder, IReadOnlyList<ProxyMember> members,
            FieldBuilder dispatchField, FieldBuilder accessorsField)
        {
            for (var i = 0; i < members.Count; i++)
            {
                var member = members[i];
                if (member.Property is not null)
                {
                    continue;
                }

                var parameterTypes = member.Method.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();

                var method = typeBuilder.DefineMethod(member.Method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                MethodAttributes.Final | MethodAttributes.HideBySig,
                member.Method.ReturnType, parameterTypes);

                var il = method.GetILGenerator();
                EmitDispatch(il, i, dispatchField, accessorsField, parameterTypes);
                EmitReturn(il, member.Method.ReturnType);
                il.Emit(OpCodes.Ret);
            }
        }

        private static MethodBuilder EmitAccessor(TypeBuilder typeBuilder, ProxyMember member, int index,
            FieldBuilder dispatchField, FieldBuilder accessorsField, Type[] parameterTypes)
        {
            var accessor = typeBuilder.DefineMethod((member.IsGetter ? "get_" : "set_") + member.Property!.Name,
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                MethodAttributes.Final | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                member.IsGetter ? member.Property.PropertyType : null, parameterTypes);

            var il = accessor.GetILGenerator();
            EmitDispatch(il, index, dispatchField, accessorsField, parameterTypes);
            EmitReturn(il, member.IsGetter ? member.Property.PropertyType : typeof(void));
            il.Emit(OpCodes.Ret);

            return accessor;
        }

        /// <summary>Loads the dispatch delegate and the member's interface method, then calls it.</summary>
        private static void EmitDispatch(ILGenerator il, int index,
            FieldBuilder dispatchField, FieldBuilder accessorsField, Type[] parameterTypes)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, dispatchField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, accessorsField);
            il.Emit(OpCodes.Ldc_I4, index);
            il.Emit(OpCodes.Ldelem_Ref);

            if (parameterTypes.Length > 0)
            {
                il.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
                il.Emit(OpCodes.Newarr, typeof(object));

                for (var parameter = 0; parameter < parameterTypes.Length; parameter++)
                {
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldc_I4, parameter);
                    il.Emit(OpCodes.Ldarg, parameter + 1);

                    if (parameterTypes[parameter].IsValueType)
                    {
                        il.Emit(OpCodes.Box, parameterTypes[parameter]);
                    }

                    il.Emit(OpCodes.Stelem_Ref);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Newarr, typeof(object));
            }

            il.Emit(OpCodes.Callvirt, typeof(Func<MethodInfo, object?[], object?>).GetMethod("Invoke")!);
        }

        private static void EmitReturn(ILGenerator il, Type type)
        {
            if (type == typeof(void))
            {
                il.Emit(OpCodes.Pop);
                return;
            }

            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
                return;
            }

            il.Emit(OpCodes.Castclass, type);
        }
    }

    /// <summary>One member of the settings interface, as it is emitted and dispatched.</summary>
    private sealed record ProxyMember(MethodInfo Method, PropertyInfo? Property, bool IsGetter)
    {
        internal static ProxyMember ForAccessor(Type settingsInterface, MethodInfo accessor, Type propertyType,
            bool isGetter)
        {
            var property = settingsInterface.GetProperties()
                .FirstOrDefault(candidate => candidate.GetMethod == accessor || candidate.SetMethod == accessor);

            if (property is null)
            {
                throw new EvoScModuleException(
                    $"Failed to find the property of '{settingsInterface.FullName}' for '{accessor.Name}'.");
            }

            // A getter returns the property's type, a setter returns void and takes it.
            var accessorType = isGetter
                ? accessor.ReturnType
                : accessor.GetParameters().FirstOrDefault()?.ParameterType;

            if (accessorType != propertyType)
            {
                throw new EvoScModuleException(
                    $"The property '{property.Name}' of '{settingsInterface.FullName}' does not match its accessor.");
            }

            return new ProxyMember(accessor, property, isGetter);
        }

        internal static ProxyMember ForMethod(Type settingsInterface, MethodInfo method)
        {
            if (method.ReturnType.IsByRef || method.GetParameters().Any(p => p.ParameterType.IsByRef))
            {
                throw new EvoScModuleException(
                    $"Settings type '{settingsInterface.FullName}' has the method '{method.Name}', which takes or " +
                    "returns a by-ref argument. Settings only support properties.");
            }

            return new ProxyMember(method, null, IsGetter: true);
        }
    }
}
