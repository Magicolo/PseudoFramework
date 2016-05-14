using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public static class BinderExtensions
	{
		public static IBindingContract<TContract> Bind<TContract, TBase>(this IBinder binder) where TContract : TBase
		{
			return binder.Bind<TContract>(typeof(TBase));
		}

		public static IBindingContract<TContract> Bind<TContract, TBase1, TBase2>(this IBinder binder) where TContract : TBase1, TBase2
		{
			return binder.Bind<TContract>(typeof(TBase1), typeof(TBase2));
		}

		public static IBindingContract<TContract> Bind<TContract, TBase1, TBase2, TBase3>(this IBinder binder) where TContract : TBase1, TBase2, TBase3
		{
			return binder.Bind<TContract>(typeof(TBase1), typeof(TBase2), typeof(TBase3));
		}

		public static void Unbind<TContract>(this IBinder binder)
		{
			binder.Unbind(typeof(TContract));
		}

		public static void UnbindAll<TContract>(this IBinder binder)
		{
			binder.UnbindAll(typeof(TContract));
		}

		public static bool HasBinding(this IBinder binder, Type contractType)
		{
			return binder.HasBinding(new InjectionContext
			{
				Container = binder.Container,
				ContractType = contractType
			});
		}

		public static bool HasBinding<TContract>(this IBinder binder)
		{
			return binder.HasBinding(typeof(TContract));
		}

		public static IBinding GetBinding(this IBinder binder, Type contractType)
		{
			return binder.GetBinding(new InjectionContext
			{
				Container = binder.Container,
				ContractType = contractType
			});
		}

		public static IBinding GetBinding<TContract>(this IBinder binder)
		{
			return binder.GetBinding(typeof(TContract));
		}

		public static IEnumerable<IBinding> GetBindings(this IBinder binder, Type contractType)
		{
			return binder.GetBindings(new InjectionContext
			{
				Container = binder.Container,
				ContractType = contractType
			});
		}

		public static IEnumerable<IBinding> GetBindings<TContract>(this IBinder binder)
		{
			return binder.GetBindings(typeof(TContract));
		}
	}
}
