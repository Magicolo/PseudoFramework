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

		static readonly Dictionary<Type, int> componentIndices = new Dictionary<Type, int>();
		static Type[] componentTypes = new Type[0];
		static int nextIndex;

		public static int GetComponentIndex(Type type)
		{
			int index;

			if (!componentIndices.TryGetValue(type, out index))
			{
				index = nextIndex++;
				componentIndices[type] = index;

				if (componentTypes.Length <= index)
					Array.Resize(ref componentTypes, Mathf.NextPowerOfTwo(index + 1));

				componentTypes[index] = type;
			}

			return index;
		}

		public static Type GetComponentType(int typeIndex)
		{
			return componentTypes[typeIndex];
		}

		public static int[] GetComponentIndices(params Type[] componentTypes)
		{
			var componentIndices = new int[componentTypes.Length];

			for (int i = 0; i < componentTypes.Length; i++)
				componentIndices[i] = GetComponentIndex(componentTypes[i]);

			Array.Sort(componentIndices);

			return componentIndices;
		}
	}

	public static class ComponentIndexHolder<T>
	{
		public static int Index = ComponentUtility.GetComponentIndex(typeof(T));
	}
}
