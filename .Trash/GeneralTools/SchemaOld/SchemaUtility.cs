using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pseudo.Internal.Schema
{
	public static class SchemaUtility
	{
		public static IVariableDefinition[] CreateVariables(object instance, Type type)
		{
			var variableList = new List<IVariableDefinition>();

			var properties = type.GetProperties(GetFlags(instance, type))
				.Where(PropertyIsValid);

			foreach (var property in properties)
			{
				var variable = CreateVariable(instance, property);

				if (variable != null)
					variableList.Add(variable);
			}

			return variableList.ToArray();
		}

		public static IVariableDefinition CreateVariable(object instance, PropertyInfo property)
		{
			var variableType = typeof(VariableDefinition<>).MakeGenericType(property.PropertyType);
			IVariableDefinition variable = null;

			try
			{
				if (property.IsStatic())
					variable = (IVariableDefinition)Activator.CreateInstance(variableType, null, property);
				else
					variable = (IVariableDefinition)Activator.CreateInstance(variableType, instance, property);
			}
			catch { }

			return variable;
		}

		public static IFunctionDefinition[] CreateFunctions(object instance, Type type)
		{
			var functionList = new List<IFunctionDefinition>();
			var methods = type.GetMethods(GetFlags(instance, type))
				.Where(MethodIsValid);

			foreach (var method in methods)
			{
				var function = CreateFunction(instance, method);

				if (function != null)
					functionList.Add(function);
			}

			return functionList.ToArray();
		}

		public static IFunctionDefinition CreateFunction(object instance, MethodInfo method)
		{
			var parameters = method.GetParameters();
			var parameterTypes = method.GetParameters().Convert(p => p.ParameterType);
			int outParameterCount = parameters.Count(p => p.IsOut);
			Type functionType;

			if (method.ReturnType == typeof(void))
			{
				switch (parameterTypes.Length)
				{
					default:
					case 0:
						functionType = typeof(FunctionDefinition);
						break;
					case 1:
						functionType = typeof(FunctionInDefinition<>).MakeGenericType(parameterTypes);
						break;
					case 2:
						if (outParameterCount == 2)
							functionType = typeof(FunctionOutDefinition<,>).MakeGenericType(parameterTypes);
						else
							functionType = typeof(FunctionInDefinition<,>).MakeGenericType(parameterTypes);
						break;
					case 3:
						if (outParameterCount == 3)
							functionType = typeof(FunctionOutDefinition<,,>).MakeGenericType(parameterTypes);
						else
							functionType = typeof(FunctionInDefinition<,,>).MakeGenericType(parameterTypes);
						break;
					case 4:
						functionType = typeof(FunctionInDefinition<,,,>).MakeGenericType(parameterTypes);
						break;
					case 5:
						functionType = typeof(FunctionInDefinition<,,,>).MakeGenericType(parameterTypes);
						break;
				}
			}
			else
			{
				switch (parameterTypes.Length)
				{
					default:
					case 0:
						functionType = typeof(FunctionOutDefinition<>).MakeGenericType(method.ReturnType);
						break;
					case 1:
						functionType = typeof(FunctionInOutDefinition<,>).MakeGenericType(parameterTypes.Joined(method.ReturnType));
						break;
					case 2:
						functionType = typeof(FunctionInOutDefinition<,,>).MakeGenericType(parameterTypes.Joined(method.ReturnType));
						break;
					case 3:
						functionType = typeof(FunctionInOutDefinition<,,,>).MakeGenericType(parameterTypes.Joined(method.ReturnType));
						break;
					case 4:
						functionType = typeof(FunctionInOutDefinition<,,,,>).MakeGenericType(parameterTypes.Joined(method.ReturnType));
						break;
					case 5:
						functionType = typeof(FunctionInOutDefinition<,,,,,>).MakeGenericType(parameterTypes.Joined(method.ReturnType));
						break;
				}
			}

			IFunctionDefinition function = null;

			try
			{
				if (method.IsStatic)
					function = (IFunctionDefinition)Activator.CreateInstance(functionType, null, method);
				else
					function = (IFunctionDefinition)Activator.CreateInstance(functionType, instance, method);
			}
			catch { }

			return function;
		}

		public static IEventDefinition[] CreateEvents(object instance, Type type)
		{
			return new IEventDefinition[0];
		}

		public static IEventDefinition CreateEvent(string name)
		{
			return new EventDefinition(name);
		}

		static BindingFlags GetFlags(object instance, Type type)
		{
			var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

			if (type.IsClass && instance != null)
				flags |= BindingFlags.Instance;

			return flags;
		}

		static bool PropertyIsValid(PropertyInfo property)
		{
			return
				!property.DeclaringType.IsValueType &&
				!property.IsSpecialName &&
				property.DeclaringType != typeof(object) &&
				!property.IsDefined(typeof(CompilerGeneratedAttribute), true) &&
				!property.IsDefined(typeof(ObsoleteAttribute), true);
		}

		static bool MethodIsValid(MethodInfo method)
		{
			return
				method.IsPublic &&
				!method.IsConstructor &&
				!method.IsGenericMethod &&
				method.DeclaringType != typeof(object) &&
				!method.IsDefined(typeof(CompilerGeneratedAttribute), true) &&
				!method.IsDefined(typeof(ObsoleteAttribute), true) &&
				(method.IsStatic || !method.DeclaringType.IsValueType) &&
				(method.IsOperator() || !method.IsSpecialName) &&
				method.GetParameters().Length <= 5;
		}
	}
}
