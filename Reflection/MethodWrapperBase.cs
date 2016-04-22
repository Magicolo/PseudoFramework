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
	public abstract class MethodWrapperBase : IMethodWrapper
	{
		public string Name
		{
			get { return method.Name; }
		}
		public Type Type
		{
			get { return method.ReturnType; }
		}

		protected readonly MethodInfo method;

		protected MethodWrapperBase(MethodInfo method)
		{
			this.method = method;
		}

		public virtual object Invoke(ref object target)
		{
			return Invoke(ref target, ReflectionUtility.EmptyArguments);
		}

		public abstract object Invoke(ref object target, params object[] arguments);
	}

	public abstract class MethodWrapperBase<TDelegate> : MethodWrapperBase where TDelegate : class
	{
		protected readonly TDelegate invoker;

		protected MethodWrapperBase(MethodInfo method) : base(method)
		{
			this.invoker = CreateInvoker(method);
		}

		static TDelegate CreateInvoker(MethodInfo method)
		{
			var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
			var dynamicMethodName = string.Format("{0}.{1}___GeneratedMethod", method.DeclaringType.FullName, method.Name);
			var dynamicMethod = new DynamicMethod(dynamicMethodName, method.ReturnType, new[] { method.DeclaringType.MakeByRefType() }.Concat(parameterTypes).ToArray(), method.DeclaringType, true);
			var generator = dynamicMethod.GetILGenerator();

			generator.Emit(OpCodes.Ldarg_0);

			if (method.DeclaringType.IsValueType)
			{
				EmitLoadParameters(generator, parameterTypes);
				generator.Emit(OpCodes.Call, method);
			}
			else
			{
				generator.Emit(OpCodes.Ldind_Ref);
				EmitLoadParameters(generator, parameterTypes);
				generator.Emit(OpCodes.Callvirt, method);
			}

			generator.Emit(OpCodes.Ret);

			return dynamicMethod.CreateDelegate(typeof(TDelegate)) as TDelegate;
		}

		static void EmitLoadParameters(ILGenerator generator, Type[] parameterTypes)
		{
			if (parameterTypes.Length > 0)
				generator.Emit(OpCodes.Ldarg_1);

			if (parameterTypes.Length > 1)
				generator.Emit(OpCodes.Ldarg_2);

			if (parameterTypes.Length > 2)
				generator.Emit(OpCodes.Ldarg_3);
		}
	}
}
