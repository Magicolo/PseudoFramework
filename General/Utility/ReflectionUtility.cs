using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public static class ReflectionUtility
	{
		//public delegate TValue Getter<TTarget, TValue>(TTarget target);
		//public delegate TTarget Setter<TTarget, TValue>(TTarget target, TValue value);

		public const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags PublicFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags NonPublicFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags StaticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		public const BindingFlags InstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		public static readonly object[] Empty = new object[0];

		//public static Getter<TTarget, TValue> CreateGetter<TTarget, TValue>(MemberInfo member)
		//{
		//	var field = member as FieldInfo;

		//	if (field != null)
		//		return CreateGetter<TTarget, TValue>(field);

		//	var property = member as PropertyInfo;

		//	if (property != null)
		//		return CreateGetter<TTarget, TValue>(property);

		//	return null;
		//}

		//public static Getter<TTarget, TValue> CreateGetter<TTarget, TValue>(FieldInfo field)
		//{
		//	if (!field.DeclaringType.Is<TTarget>() || !field.FieldType.Is<TValue>())
		//		throw new ArgumentException(string.Format("Target type {0} and value type {1} are incompatible with field {2}.{3}.", typeof(TTarget).FullName, typeof(TValue).FullName, field.DeclaringType.FullName, field.Name));

		//	if (ApplicationUtility.IsAOT)
		//		return (TTarget target) => (TValue)field.GetValue(target);
		//	else
		//	{
		//		var methodName = string.Format("{0}.{1}___GeneratedGetter", field.DeclaringType.FullName, field.FieldType.FullName);
		//		var method = new DynamicMethod(methodName, typeof(TValue), new Type[] { typeof(TTarget) }, field.DeclaringType, true);
		//		var generator = method.GetILGenerator();

		//		generator.EmitLoadArgument0<TTarget>(field.DeclaringType);
		//		generator.EmitCastOrUnboxToTemp<TTarget>(field.DeclaringType);
		//		generator.Emit(OpCodes.Ldfld, field);
		//		generator.EmitCastOrBox<TValue>(field.FieldType);
		//		generator.Emit(OpCodes.Ret);

		//		return (Getter<TTarget, TValue>)method.CreateDelegate(typeof(Getter<TTarget, TValue>));
		//	}
		//}

		//public static Getter<TTarget, TValue> CreateGetter<TTarget, TValue>(PropertyInfo property)
		//{
		//	if (!property.CanRead)
		//		throw new ArgumentException(string.Format("Property {0}.{1} is write only.", property.DeclaringType.FullName, property.Name));
		//	if (!property.DeclaringType.Is<TTarget>() || !property.PropertyType.Is<TValue>())
		//		throw new ArgumentException(string.Format("Target type {0} and value type {1} are incompatible with property {2}.{3}.", typeof(TTarget).FullName, typeof(TValue).FullName, property.DeclaringType.FullName, property.Name));

		//	if (typeof(TTarget) == property.DeclaringType && typeof(TValue) == property.PropertyType)
		//		return CreateDelegate<Getter<TTarget, TValue>>(property.GetGetMethod(true));
		//	else if (property.IsAutoProperty())
		//		return CreateGetter<TTarget, TValue>(property.GetBackingField());
		//	else if (ApplicationUtility.IsAOT)
		//		return target => (TValue)property.GetValue(target, null);
		//	else
		//	{
		//		var methodName = string.Format("{0}.{1}___GeneratedGetter", property.DeclaringType.FullName, property.PropertyType.FullName);
		//		var method = new DynamicMethod(methodName, typeof(TValue), new Type[] { typeof(TTarget) }, property.DeclaringType, true);
		//		var generator = method.GetILGenerator();

		//		generator.EmitLoadArgument0<TTarget>(property.DeclaringType);
		//		generator.EmitCastOrUnboxToTemp<TTarget>(property.DeclaringType);
		//		generator.EmitCall(property.GetGetMethod(true));
		//		generator.EmitCastOrBox<TValue>(property.PropertyType);
		//		generator.Emit(OpCodes.Ret);

		//		return (Getter<TTarget, TValue>)method.CreateDelegate(typeof(Getter<TTarget, TValue>));
		//	}
		//}

		//public static Setter<TTarget, TValue> CreateSetter<TTarget, TValue>(MemberInfo member)
		//{
		//	var field = member as FieldInfo;

		//	if (field != null)
		//		return CreateSetter<TTarget, TValue>(field);

		//	var property = member as PropertyInfo;

		//	if (property != null)
		//		return CreateSetter<TTarget, TValue>(property);

		//	return null;
		//}

		//public static Setter<TTarget, TValue> CreateSetter<TTarget, TValue>(FieldInfo field)
		//{
		//	if (!field.DeclaringType.Is<TTarget>() || !field.FieldType.Is<TValue>())
		//		throw new ArgumentException(string.Format("Target type {0} and value type {1} are incompatible with field {2}.{3}.", typeof(TTarget).FullName, typeof(TValue).FullName, field.DeclaringType.FullName, field.Name));

		//	if (ApplicationUtility.IsAOT)
		//		return (TTarget target, TValue value) =>
		//		{
		//			field.SetValue(target, value);
		//			return target;
		//		};
		//	else
		//	{
		//		var methodName = string.Format("{0}.{1}___GeneratedSetter", field.DeclaringType.FullName, field.FieldType.FullName);
		//		var method = new DynamicMethod(methodName, typeof(TTarget), new Type[] { typeof(TTarget), typeof(TValue) }, field.DeclaringType, true);
		//		var generator = method.GetILGenerator();

		//		//generator.Emit(OpCodes.Ldarg_0);
		//		generator.EmitLoadArgument0<TTarget>(field.DeclaringType);
		//		generator.EmitCastOrUnboxToTemp<TTarget>(field.DeclaringType);
		//		generator.Emit(OpCodes.Ldarg_1);
		//		generator.EmitCastOrUnbox<TValue>(field.FieldType);
		//		generator.Emit(OpCodes.Stfld, field);
		//		generator.EmitLoadArgumentOrLocal0<TTarget>(field.DeclaringType);
		//		generator.EmitCastOrBox<TTarget>(field.DeclaringType);
		//		generator.Emit(OpCodes.Ret);

		//		return (Setter<TTarget, TValue>)method.CreateDelegate(typeof(Setter<TTarget, TValue>));
		//	}
		//}

		//public static Setter<TTarget, TValue> CreateSetter<TTarget, TValue>(PropertyInfo property)
		//{
		//	if (!property.CanWrite)
		//		throw new ArgumentException(string.Format("Property {0}.{1} is read only.", property.DeclaringType.FullName, property.Name));
		//	if (!property.DeclaringType.Is<TTarget>() || !property.PropertyType.Is<TValue>())
		//		throw new ArgumentException(string.Format("Target type {0} and value type {1} are incompatible with property {2}.{3}.", typeof(TTarget).FullName, typeof(TValue).FullName, property.DeclaringType.FullName, property.Name));

		//	if (typeof(TTarget) == property.DeclaringType && typeof(TValue) == property.PropertyType)
		//	{
		//		var setter = CreateDelegate<Action<TTarget, TValue>>(property.GetSetMethod(true));
		//		return (target, value) =>
		//		{
		//			setter(target, value);
		//			return target;
		//		};
		//	}
		//	else if (property.IsAutoProperty())
		//		return CreateSetter<TTarget, TValue>(property.GetBackingField());
		//	else if (ApplicationUtility.IsAOT)
		//		return (TTarget target, TValue value) =>
		//		{
		//			property.SetValue(target, value, null);
		//			return target;
		//		};
		//	else
		//	{
		//		var methodName = string.Format("{0}.{1}___GeneratedSetter", property.DeclaringType.FullName, property.PropertyType.FullName);
		//		var method = new DynamicMethod(methodName, typeof(TTarget), new Type[] { typeof(TTarget), typeof(TValue) }, property.DeclaringType, true);
		//		var generator = method.GetILGenerator();

		//		generator.EmitLoadArgument0<TTarget>(property.DeclaringType);
		//		generator.EmitCastOrUnboxRefToTemp<TTarget>(property.DeclaringType);
		//		generator.Emit(OpCodes.Ldarg_1);
		//		generator.EmitCastOrUnbox<TValue>(property.PropertyType);
		//		generator.EmitCall(property.GetSetMethod(true));
		//		generator.EmitLoadArgumentOrLocal0<TTarget>(property.DeclaringType);
		//		generator.EmitCastOrBox<TTarget>(property.DeclaringType);
		//		generator.Emit(OpCodes.Ret);

		//		return (Setter<TTarget, TValue>)method.CreateDelegate(typeof(Setter<TTarget, TValue>));
		//	}
		//}

		//public static IInvoker CreateInvoker(MethodInfo method)
		//{
		//	return new Invoker(method);
		//}

		//public static TDelegate CreateDelegate<TDelegate>(MethodInfo method) where TDelegate : class
		//{
		//	return Delegate.CreateDelegate(typeof(TDelegate), method) as TDelegate;
		//}

		//public static TDelegate CreateDelegate<TDelegate>(MethodInfo method, object target) where TDelegate : class
		//{
		//	return Delegate.CreateDelegate(typeof(TDelegate), target, method) as TDelegate;
		//}

		//static void EmitLoadArgument0<T>(this ILGenerator generator, Type type)
		//{
		//	if (typeof(T) == type && type.IsValueType)
		//		generator.Emit(OpCodes.Ldarga_S, 0);
		//	else
		//		generator.Emit(OpCodes.Ldarg_0);
		//}

		//static void EmitLoadArgumentOrLocal0<T>(this ILGenerator generator, Type type)
		//{
		//	if (typeof(T) == type)
		//		generator.Emit(OpCodes.Ldarg_0);
		//	else
		//		generator.Emit(OpCodes.Ldloc_S, 0);
		//}

		//static void EmitCastOrUnbox<T>(this ILGenerator generator, Type type)
		//{
		//	if (typeof(T) != type)
		//	{
		//		// Unbox value
		//		if (type.IsValueType)
		//			generator.Emit(OpCodes.Unbox_Any, type);
		//		// Cast value
		//		else
		//			generator.Emit(OpCodes.Castclass, type);
		//	}
		//}

		//static void EmitCastOrBox<T>(this ILGenerator generator, Type type)
		//{
		//	if (typeof(T) != type)
		//	{
		//		// Box value.
		//		if (type.IsValueType)
		//			generator.Emit(OpCodes.Box, type);
		//		// Cast value.
		//		else
		//			generator.Emit(OpCodes.Castclass, type);
		//	}
		//}

		//static void EmitCastOrUnboxToTemp<T>(this ILGenerator generator, Type type)
		//{
		//	if (typeof(T) != type)
		//	{
		//		// Create a temporary variable of the actual value type
		//		var local = generator.DeclareLocal(type);

		//		if (type.IsValueType)
		//		{
		//			generator.Emit(OpCodes.Unbox_Any, type);
		//			generator.Emit(OpCodes.Stloc_0, local);
		//			generator.Emit(OpCodes.Ldloca_S, local);
		//		}
		//		// Cast target to its actual type.
		//		else
		//		{
		//			generator.Emit(OpCodes.Castclass, type);
		//			generator.Emit(OpCodes.Stloc_0, local);
		//			generator.Emit(OpCodes.Ldloc_0, local);
		//		}
		//	}
		//}

		//static void EmitCastOrUnboxRefToTemp<T>(this ILGenerator generator, Type type)
		//{
		//	if (type.IsClass)
		//		generator.Emit(OpCodes.Ldind_Ref);

		//	if (typeof(T) != type)
		//	{
		//		// Create a temporary variable of the actual target type
		//		var local = generator.DeclareLocal(type);

		//		// Unbox target
		//		if (type.IsValueType)
		//		{
		//			generator.Emit(OpCodes.Ldind_Ref);
		//			generator.Emit(OpCodes.Unbox_Any, type);
		//			generator.Emit(OpCodes.Stloc_0, local);
		//			generator.Emit(OpCodes.Ldloca_S, local);
		//		}
		//		// Cast target to its actual type.
		//		else
		//		{
		//			generator.Emit(OpCodes.Castclass, type);
		//			generator.Emit(OpCodes.Stloc_0, local);
		//			generator.Emit(OpCodes.Ldloc_S, local);
		//		}
		//	}
		//}

		//static void EmitSetRefFromTemp<T>(this ILGenerator generator, Type type)
		//{
		//	if (typeof(T) != type)
		//	{
		//		generator.Emit(OpCodes.Ldarg_0);
		//		generator.Emit(OpCodes.Ldloc_0);

		//		if (type.IsValueType)
		//			generator.Emit(OpCodes.Box, type);

		//		generator.Emit(OpCodes.Stind_Ref);
		//	}
		//}

		//static void EmitCall(this ILGenerator generator, MethodInfo method)
		//{
		//	if (method.DeclaringType.IsValueType)
		//		generator.Emit(OpCodes.Call, method);
		//	else
		//		generator.Emit(OpCodes.Callvirt, method);
		//}
	}
}
