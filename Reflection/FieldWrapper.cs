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
	public class FieldWrapper : IMemberWrapper
	{
		readonly FieldInfo field;

		public FieldWrapper(FieldInfo field)
		{
			this.field = field;
		}

		public object Get(ref object target)
		{
			return field.GetValue(target);
		}

		public void Set(ref object target, object value)
		{
			field.SetValue(target, value);
		}
	}

	public class FieldWrapper<TTarget, TValue> : IMemberWrapper
	{
		readonly Getter<TTarget, TValue> getter;
		readonly Setter<TTarget, TValue> setter;

		public FieldWrapper(FieldInfo field)
		{
			getter = CreateGetter(field);
			setter = CreateSetter(field);
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

		static Getter<TTarget, TValue> CreateGetter(FieldInfo field)
		{
			var methodName = string.Format("{0}.{1}.{2}___GeneratedFieldGetter", field.DeclaringType.FullName, field.FieldType.FullName, field.Name);
			var method = new DynamicMethod(methodName, field.FieldType, new Type[] { field.DeclaringType.MakeByRefType() }, field.DeclaringType, true);
			var generator = method.GetILGenerator();

			generator.Emit(OpCodes.Ldarg_0);

			if (field.DeclaringType.IsClass)
				generator.Emit(OpCodes.Ldind_Ref);

			generator.Emit(OpCodes.Ldfld, field);
			generator.Emit(OpCodes.Ret);

			return (Getter<TTarget, TValue>)method.CreateDelegate(typeof(Getter<TTarget, TValue>));
		}

		static Setter<TTarget, TValue> CreateSetter(FieldInfo field)
		{
			var methodName = string.Format("{0}.{1}.{2}___GeneratedFieldSetter", field.DeclaringType.FullName, field.FieldType.FullName, field.Name);
			var method = new DynamicMethod(methodName, typeof(void), new Type[] { field.DeclaringType.MakeByRefType(), field.FieldType }, field.DeclaringType, true);
			var generator = method.GetILGenerator();

			generator.Emit(OpCodes.Ldarg_0);

			if (field.DeclaringType.IsClass)
				generator.Emit(OpCodes.Ldind_Ref);

			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Stfld, field);
			generator.Emit(OpCodes.Ret);

			return (Setter<TTarget, TValue>)method.CreateDelegate(typeof(Setter<TTarget, TValue>));
		}
	}
}
