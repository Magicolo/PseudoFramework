using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Initialization.Internal;

namespace Pseudo.Initialization
{
	public delegate void Initialize(ref object instance, HashSet<object> toIgnore);

	public abstract class Initializer<T> : IInitializer<T>
	{
		public static IInitializer<T> Default
		{
			get
			{
				if (defaultInitializer == null)
					defaultInitializer = InitializationUtility.GetInitializer<T>();

				return defaultInitializer;
			}
		}

		static IInitializer<T> defaultInitializer;
		protected IEqualityComparer<T> comparer = PEqualityComparer<T>.Default;
		protected bool isValue = typeof(T).IsValueType;

		public IInitialization<T> Cache(T reference)
		{
			return new Initialization<T>(CreateOperations(reference));
		}

		public void Initialize(ref object instance, object reference)
		{
			((IInitializer)this).Initialize(ref instance, reference, new HashSet<object>());
		}

		public void Initialize(ref T instance, T reference)
		{
			((IInitializer<T>)this).Initialize(ref instance, reference, new HashSet<object>());
		}

		IInitializationOperation[] IInitializer.CreateOperations(object reference)
		{
			return CreateOperations(reference);
		}

		void IInitializer.Initialize(ref object instance, object reference, HashSet<object> toIgnore)
		{
			if (PreInitialize(ref instance, reference, toIgnore))
				Initialize(ref instance, reference, toIgnore);
		}

		void IInitializer<T>.Initialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			if (PreInitialize(ref instance, reference, toIgnore))
				Initialize(ref instance, reference, toIgnore);
		}

		protected virtual IInitializationOperation[] CreateOperations(object reference)
		{
			return new[] { new InitializeOperation(reference, this) };
		}

		protected virtual bool PreInitialize(ref object instance, object reference, HashSet<object> toIgnore)
		{
			var unboxedInstance = (T)instance;
			var unboxedReference = (T)reference;

			if (PreInitialize(ref unboxedInstance, unboxedReference, toIgnore))
				return true;
			else
			{
				instance = unboxedInstance;
				return false;
			}
		}

		protected virtual bool PreInitialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			if (isValue)
				return true;
			else if (comparer.Equals(instance, reference) ||
				comparer.Equals(reference, default(T)) ||
				comparer.Equals(instance, default(T)))
			{
				instance = reference;
				return false;
			}
			else
				return isValue || toIgnore.Add(instance);
		}

		protected virtual void Initialize(ref object instance, object reference, HashSet<object> toIgnore)
		{
			var unboxedInstance = (T)instance;
			var unboxedReference = (T)reference;

			Initialize(ref unboxedInstance, unboxedReference, toIgnore);
			instance = unboxedInstance;
		}

		protected abstract void Initialize(ref T instance, T reference, HashSet<object> toIgnore);
	}
}
