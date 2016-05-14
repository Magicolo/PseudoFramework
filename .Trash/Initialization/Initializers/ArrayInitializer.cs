using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization.Internal
{
	public class ArrayInitializer<T> : Initializer<T> where T : class, ICloneable, IEnumerable, ICollection, IList
	{
		protected override IInitializationOperation[] CreateOperations(object reference)
		{
			if (reference == null)
				return base.CreateOperations(reference);
			else
			{
				var operations = new IInitializationOperation[((Array)reference).Length];
				operations.Fill(index => new ArraySetOperation(index, reference));

				return operations;
			}
		}

		protected override bool PreInitialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			bool success = base.PreInitialize(ref instance, reference, toIgnore);

			if (success && instance.Count != reference.Count)
			{
				var clone = reference.Clone() as T;
				Array.Copy(instance as Array, 0, clone as Array, 0, clone.Count > instance.Count ? instance.Count : clone.Count);
				instance = clone;
			}

			return success;
		}

		protected override void Initialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			for (int i = 0; i < instance.Count; i++)
			{
				var instanceValue = instance[i];
				var referenceValue = reference[i];

				if (referenceValue is ValueType || referenceValue != null)
				{
					var initializer = InitializationUtility.GetInitializer(referenceValue.GetType());
					initializer.Initialize(ref instanceValue, referenceValue, toIgnore);
					instance[i] = instanceValue;
				}
				else
					instance[i] = referenceValue;
			}
		}
	}
}
