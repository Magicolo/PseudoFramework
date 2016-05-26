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
	public abstract class ConstructorWrapperBase : IConstructorWrapper
	{
		public string Name
		{
			get { return constructor.Name; }
		}
		public Type Type
		{
			get { return constructor.DeclaringType; }
		}

		protected readonly ConstructorInfo constructor;

		protected ConstructorWrapperBase(ConstructorInfo constructor)
		{
			this.constructor = constructor;
		}

		public virtual object Invoke()
		{
			return Invoke(ReflectionUtility.EmptyArguments);
		}

		public abstract object Invoke(params object[] arguments);
	}

	public abstract class ConstructorWrapperBase<TDelegate> : ConstructorWrapperBase where TDelegate : class
	{
		protected readonly TDelegate invoker;

		protected ConstructorWrapperBase(ConstructorInfo constructor) : base(constructor)
		{
			invoker = CreateInvoker(constructor);
		}

		static TDelegate CreateInvoker(ConstructorInfo constructor)
		{
			var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
			var dynamicMethodName = string.Format("{0}.{1}___GeneratedConstructor", constructor.DeclaringType.FullName, constructor.Name);
			var dynamicMethod = new DynamicMethod(dynamicMethodName, constructor.DeclaringType, parameterTypes, constructor.DeclaringType, true);
			var generator = dynamicMethod.GetILGenerator();

			if (constructor.DeclaringType.IsValueType && parameterTypes.Length == 0)
			{
				generator.DeclareLocal(constructor.DeclaringType);
				generator.Emit(OpCodes.Ldloca_S, 0);
				generator.Emit(OpCodes.Initobj, constructor.DeclaringType);
				generator.Emit(OpCodes.Ldloc_0);
			}
			else
			{
				EmitLoadParameters(generator, parameterTypes);
				generator.Emit(OpCodes.Newobj, constructor);
			}

			generator.Emit(OpCodes.Ret);

			return dynamicMethod.CreateDelegate(typeof(TDelegate)) as TDelegate;
		}

		static void EmitLoadParameters(ILGenerator generator, Type[] parameterTypes)
		{
			if (parameterTypes.Length > 0)
				generator.Emit(OpCodes.Ldarg_0);

			if (parameterTypes.Length > 1)
				generator.Emit(OpCodes.Ldarg_1);

			if (parameterTypes.Length > 2)
				generator.Emit(OpCodes.Ldarg_2);
		}
	}
}
