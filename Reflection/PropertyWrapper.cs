using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System.Reflection.Emit;

namespace Pseudo.Reflection.Internal
{
	public class PropertyWrapper : IPropertyWrapper
	{
		public string Name
		{
			get { return property.Name; }
		}
		public Type Type
		{
			get { return property.PropertyType; }
		}

		readonly PropertyInfo property;

		public PropertyWrapper(PropertyInfo property)
		{
			this.property = property;
		}

		public object Get(ref object target)
		{
			return property.GetValue(target, null);
		}

		public void Set(ref object target, object value)
		{
			property.SetValue(target, value, null);
		}
	}

	public class PropertyWrapper<TTarget, TValue> : IPropertyWrapper
	{
		public string Name
		{
			get { return property.Name; }
		}
		public Type Type
		{
			get { return property.PropertyType; }
		}

		readonly PropertyInfo property;
		readonly Getter<TTarget, TValue> getter;
		readonly Setter<TTarget, TValue> setter;

		public PropertyWrapper(PropertyInfo property)
		{
			this.property = property;

			getter = CreateGetter(property);
			setter = CreateSetter(property);
		}

		public object Get(ref object target)
		{
			var castedTarget = (TTarget)target;
			var result = getter(ref castedTarget);
			target = castedTarget;

			return result;
		}

		public void Set(ref object target, object value)
		{
			var castedTarget = (TTarget)target;
			setter(ref castedTarget, (TValue)value);
			target = castedTarget;
		}

		static Getter<TTarget, TValue> CreateGetter(PropertyInfo property)
		{
			if (!property.CanRead)
				return null;

			var dynamicMethodName = string.Format("{0}.{1}.{2}___GeneratedPropertyGetter", property.DeclaringType.FullName, property.PropertyType.FullName, property.Name);
			var dynamicMethod = new DynamicMethod(dynamicMethodName, property.PropertyType, new Type[] { property.DeclaringType.MakeByRefType() }, property.DeclaringType, true);
			var generator = dynamicMethod.GetILGenerator();

			generator.Emit(OpCodes.Ldarg_0);

			if (property.DeclaringType.IsValueType)
				generator.Emit(OpCodes.Call, property.GetGetMethod(true));
			else
			{
				generator.Emit(OpCodes.Ldind_Ref);
				generator.Emit(OpCodes.Callvirt, property.GetGetMethod(true));
			}

			generator.Emit(OpCodes.Ret);

			return (Getter<TTarget, TValue>)dynamicMethod.CreateDelegate(typeof(Getter<TTarget, TValue>));
		}

		static Setter<TTarget, TValue> CreateSetter(PropertyInfo property)
		{
			if (!property.CanWrite)
				return null;

			var dynamicMethodName = string.Format("{0}.{1}.{2}___GeneratedPropertySetter", property.DeclaringType.FullName, property.PropertyType.FullName, property.Name);
			var dynamicMethod = new DynamicMethod(dynamicMethodName, typeof(void), new Type[] { property.DeclaringType.MakeByRefType(), property.PropertyType }, property.DeclaringType, true);
			var generator = dynamicMethod.GetILGenerator();

			generator.Emit(OpCodes.Ldarg_0);

			if (property.DeclaringType.IsValueType)
			{
				generator.Emit(OpCodes.Ldarg_1);
				generator.Emit(OpCodes.Call, property.GetSetMethod(true));
			}
			else
			{
				generator.Emit(OpCodes.Ldind_Ref);
				generator.Emit(OpCodes.Ldarg_1);
				generator.Emit(OpCodes.Callvirt, property.GetSetMethod(true));
			}

			generator.Emit(OpCodes.Ret);

			return (Setter<TTarget, TValue>)dynamicMethod.CreateDelegate(typeof(Setter<TTarget, TValue>));
		}
	}
}
