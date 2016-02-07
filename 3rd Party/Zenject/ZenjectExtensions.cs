using UnityEngine;
using System.Collections;
using ModestTree.Util;
using System;

namespace Zenject
{
	public static class ZenjectExtensions
	{
		public static void BindSinglePrefabOrInstance<TContract, TConcrete>(this DiContainer container, TConcrete prefab) where TConcrete : Component, TContract
		{
			var instance = UnityEngine.Object.FindObjectOfType<TConcrete>();

			if (instance != null)
			{
				container.Bind<TContract>().ToSingleMonoBehaviour<TConcrete>(instance.gameObject);
				instance.transform.parent = container.DefaultParent;
			}
			else if (prefab != null)
				container.Bind<TContract>().ToSinglePrefab<TConcrete>(prefab.gameObject);
		}

		public static void BindInitializable<T>(this DiContainer container, int priority = 0) where T : IInitializable
		{
			container.Bind<Tuple<Type, int>>().ToInstance(Tuple.New(typeof(T), priority)).WhenInjectedInto<InitializableManager>();
		}

		public static void BindDisposable<T>(this DiContainer container, int priority = 0) where T : IDisposable
		{
			container.Bind<Tuple<Type, int>>().ToInstance(Tuple.New(typeof(T), priority)).WhenInjectedInto<DisposableManager>();
		}

		public static void BindTickablePriority<T>(this DiContainer container, int priority = 0) where T : ITickable
		{
			container.Bind<Tuple<Type, int>>().ToInstance(Tuple.New(typeof(T), priority)).WhenInjectedInto<TickableManager>();
		}

		public static void BindFixedTickablePriority<T>(this DiContainer container, int priority = 0) where T : IFixedTickable
		{
			container.Bind<Tuple<Type, int>>("Fixed").ToInstance(Tuple.New(typeof(T), priority)).WhenInjectedInto<TickableManager>();
		}

		public static void BindLateTickablePriority<T>(this DiContainer container, int priority = 0) where T : ILateTickable
		{
			container.Bind<Tuple<Type, int>>("Late").ToInstance(Tuple.New(typeof(T), priority)).WhenInjectedInto<TickableManager>();
		}
	}
}