using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling;
using UnityEngine.Assertions;

namespace Pseudo.Injection
{
	public static class PoolingExtensions
	{
		public static IBindingCondition ToPool(this IBindingContract contract, IPool pool)
		{
			Assert.IsTrue(pool.Type.Is(contract.ContractType));

			return contract.ToMethod(c => pool.Create()).AsTransient();
		}

		public static IBindingCondition ToPool<TContract, TConcrete>(this IBindingContract<TContract> contract, IPool<TConcrete> pool) where TConcrete : class, TContract
		{
			return contract.ToPool(pool);
		}
	}
}
