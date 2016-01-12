using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pseudo.Internal.Entity3
{
	public static class ComponentUtility
	{
		public static int TypeCount
		{
			get { return nextIndex; }
		}

		public static Type[] ComponentTypes
		{
			get { return componentTypes; }
		}

		static readonly Dictionary<Type, int> componentIndexDict = new Dictionary<Type, int>();
		static readonly Dictionary<Type, Type[]> subComponentTypeDict = new Dictionary<Type, Type[]>();
		static Type[] componentTypes = new Type[0];
		static int nextIndex;

		public static int GetComponentIndex(Type type)
		{
			int index;

			if (!componentIndexDict.TryGetValue(type, out index))
			{
				index = nextIndex++;
				componentIndexDict[type] = index;

				if (componentTypes.Length <= index)
					Array.Resize(ref componentTypes, Mathf.NextPowerOfTwo(index + 1));

				componentTypes[index] = type;
			}

			return index;
		}

		public static int[] GetComponentIndices(params Type[] componentTypes)
		{
			var componentIndices = new int[componentTypes.Length];

			for (int i = 0; i < componentTypes.Length; i++)
				componentIndices[i] = GetComponentIndex(componentTypes[i]);

			Array.Sort(componentIndices);

			return componentIndices;
		}

		public static Type GetComponentType(int typeIndex)
		{
			return componentTypes[typeIndex];
		}

		public static Type[] GetComponentTypes(IList<int> componentIndices)
		{
			var componentTypes = new Type[componentIndices.Count];

			for (int i = 0; i < componentIndices.Count; i++)
				componentTypes[i] = GetComponentType(componentIndices[i]);

			return componentTypes;
		}

		public static Type[] GetSubComponentTypes(Type componentType)
		{
			Type[] subComponentTypes;

			if (!subComponentTypeDict.TryGetValue(componentType, out subComponentTypes))
			{
				var type = componentType;
				var types = new HashSet<Type>();

				var interfaces = type.FindInterfaces((t, f) => typeof(IComponent).IsAssignableFrom(t), null);

				for (int i = 0; i < interfaces.Length; i++)
					types.Add(interfaces[i]);

				while (type != null && typeof(IComponent).IsAssignableFrom(type))
				{
					types.Add(type);
					type = type.BaseType;
				}

				subComponentTypes = types.ToArray();
				subComponentTypeDict[componentType] = subComponentTypes;
			}

			return subComponentTypes;
		}
	}

	public static class ComponentIndexHolder<T>
	{
		public static int Index = ComponentUtility.GetComponentIndex(typeof(T));
	}
}
