using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization.Internal
{
	public static class InitializationUtility
	{
		static readonly Type[] initializerTypes = TypeUtility.AllTypes
			.Where(t => t.Is<IInitializer>() && t.IsConcrete() && t.HasDefaultConstructor())
			.ToArray();
		static readonly Dictionary<Type, IInitializer> typeToInitializer = new Dictionary<Type, IInitializer>();

		public static IInitializer GetInitializer(Type type)
		{
			IInitializer initializer;

			if (!typeToInitializer.TryGetValue(type, out initializer))
			{
				initializer = CreateInitializer(type);
				typeToInitializer[type] = initializer;
			}

			return initializer;
		}

		public static IInitializer<T> GetInitializer<T>()
		{
			return (IInitializer<T>)GetInitializer(typeof(T));
		}

		static IInitializer CreateInitializer(Type type)
		{
			var initializerAbstractType = typeof(IInitializer<>).MakeGenericType(type);
			var initializerType = initializerTypes.FirstOrDefault(t => t.Is(initializerAbstractType));

			if (initializerType == null)
			{
				if (type.Is(typeof(IInitializable<>), type))
					initializerType = typeof(GenericInitializer<>).MakeGenericType(type);
				else if (type.IsPureValueType())
					initializerType = typeof(PureInitializer<>).MakeGenericType(type);
				else if (type.IsArray)
					initializerType = typeof(ArrayInitializer<>).MakeGenericType(type);
				else
					initializerType = typeof(ReflectionInitializer<>).MakeGenericType(type);
			}

			return (IInitializer)Activator.CreateInstance(initializerType);
		}
	}
}
