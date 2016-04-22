using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection
{
	public static class UnityExtensions
	{
		public static IBindingScope ToPrefab(this IBindingContract contract, UnityEngine.Object prefab)
		{
			Assert.IsNotNull(prefab);
			Assert.IsTrue(prefab.GetType().Is(contract.ContractType));

			return contract.ToMethod(c => UnityEngine.Object.Instantiate(prefab));
		}

		public static IBindingScope ToPrefab(this IBindingContract contract, GameObject prefab, bool injectHierarchy = true)
		{
			Assert.IsNotNull(prefab);
			Assert.IsTrue(contract.ContractType.Is<GameObject>() || prefab.GetComponent(contract.ContractType) != null);

			return contract.ToMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);

				if (injectHierarchy)
				{
					var components = instance.GetComponentsInChildren<MonoBehaviour>();

					for (int i = 0; i < components.Length; i++)
						c.Container.Injector.Inject(components[i]);
				}

				if (contract.ContractType.Is<GameObject>())
					return instance;
				else
					return instance.GetComponent(contract.ContractType);
			});
		}

		public static IBindingScope ToPrefab<TContract, TConcrete>(this IBindingContract<TContract> contract, TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return ((IBindingContract)contract).ToPrefab(prefab);
		}
	}
}
