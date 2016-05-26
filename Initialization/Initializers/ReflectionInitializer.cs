using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;

namespace Pseudo.Initialization.Internal
{
	public class ReflectionInitializer<T> : Initializer<T>
	{
		static readonly IFieldWrapper[] fields = ReflectionUtility.CreateFieldWrappers(typeof(T), filter: f => !f.IsInitOnly).ToArray();

		protected override IInitializationOperation[] CreateOperations(object reference)
		{
			if (reference == null)
				return base.CreateOperations(reference);
			else
				return fields.Convert(f => new FieldSetOperation(f, reference));
		}

		protected override void Initialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			object boxedInstance = instance;
			object boxedReference = reference;

			Initialize(ref boxedInstance, boxedReference, toIgnore);
			instance = (T)boxedInstance;
		}

		protected override void Initialize(ref object instance, object reference, HashSet<object> toIgnore)
		{
			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				var instanceValue = field.Get(ref instance);
				var referenceValue = field.Get(ref reference);

				if (isValue || referenceValue != null)
				{
					var initializer = InitializationUtility.GetInitializer(referenceValue.GetType());
					initializer.Initialize(ref instanceValue, referenceValue, toIgnore);
					field.Set(ref instance, instanceValue);
				}
				else
					field.Set(ref instance, referenceValue);
			}
		}
	}
}
