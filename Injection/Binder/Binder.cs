using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;
using UnityEngine.Assertions;
using System.Reflection;

namespace Pseudo.Injection.Internal
{
	public class Binder : IBinder
	{
		public IContainer Container
		{
			get { return container; }
		}

		readonly IContainer container;
		readonly Dictionary<Type, List<IBinding>> contractTypeToBindings = new Dictionary<Type, List<IBinding>>();

		public Binder(IContainer container)
		{
			this.container = container;
		}

		public void Bind(IBinding binding)
		{
			Assert.IsNotNull(binding);

			GetBindingList(binding.ContractType).Add(binding);
		}

		public void Bind(Assembly assembly)
		{
			Assert.IsNotNull(assembly);

			var types = assembly.GetTypes();

			for (int i = 0; i < types.Length; i++)
			{
				var installers = InjectionUtility.GetInjectionInfo(types[i]).Installers;

				for (int j = 0; j < installers.Length; j++)
					installers[j].Install(container);
			}
		}

		public IBindingContext Bind(Type contractType)
		{
			Assert.IsNotNull(contractType);

			return new BindingContext(contractType, container);
		}

		public IBindingContext<TContract> Bind<TContract>()
		{
			return new BindingContext<TContract>(container);
		}

		public IBindingContext Bind(Type contractType, params Type[] baseTypes)
		{
			Assert.IsNotNull(contractType);
			Assert.IsNotNull(baseTypes);
			Assert.IsTrue(Array.TrueForAll(baseTypes, t => contractType.Is(t)));

			return new MultipleBindingContext(contractType, baseTypes, container);
		}

		public IBindingContext<TContract> Bind<TContract, TBase>() where TContract : TBase
		{
			return Bind<TContract>(typeof(TBase));
		}

		public IBindingContext<TContract> Bind<TContract, TBase1, TBase2>() where TContract : TBase1, TBase2
		{
			return Bind<TContract>(typeof(TBase1), typeof(TBase2));
		}

		public IBindingContext<TContract> Bind<TContract, TBase1, TBase2, TBase3>() where TContract : TBase1, TBase2, TBase3
		{
			return Bind<TContract>(typeof(TBase1), typeof(TBase2), typeof(TBase3));
		}

		public IBindingContext<TContract> Bind<TContract>(params Type[] baseTypes)
		{
			Assert.IsNotNull(baseTypes);
			Assert.IsTrue(Array.TrueForAll(baseTypes, t => typeof(TContract).Is(t)));

			return new MultipleBindingContext<TContract>(baseTypes, container);
		}

		public IBindingContext BindAll(Type contractType)
		{
			Assert.IsNotNull(contractType);

			return Bind(contractType, TypeUtility.GetBaseTypes(contractType, false, true).ToArray());
		}

		public IBindingContext<TContract> BindAll<TContract>()
		{
			return Bind<TContract>(TypeUtility.GetBaseTypes(typeof(TContract), false, true).ToArray());
		}

		public void Unbind(IBinding binding)
		{
			Assert.IsNotNull(binding);

			GetBindingList(binding.ContractType).Remove(binding);
		}

		public void Unbind(Type contractType)
		{
			Assert.IsNotNull(contractType);

			contractTypeToBindings.Remove(contractType);
		}

		public void Unbind(params Type[] contractTypes)
		{
			Assert.IsNotNull(contractTypes);

			for (int i = 0; i < contractTypes.Length; i++)
				Unbind(contractTypes[i]);
		}

		public void Unbind<TContract>()
		{
			Unbind(typeof(TContract));
		}

		public void UnbindAll(Type contractType)
		{
			Assert.IsNotNull(contractType);

			Unbind(contractType);
			Unbind(contractType.GetInterfaces());
		}

		public void UnbindAll<TContract>()
		{
			UnbindAll(typeof(TContract));
		}

		public void UnbindAll()
		{
			contractTypeToBindings.Clear();
		}

		public bool HasBinding(Type contractType)
		{
			Assert.IsNotNull(contractType);

			return GetBindingList(contractType).Count > 0;
		}

		public bool HasBinding<TContract>()
		{
			return HasBinding(typeof(TContract));
		}

		public bool HasBinding(IBinding binding)
		{
			return GetBindingList(binding.ContractType).Contains(binding);
		}

		public IBinding GetBinding(InjectionContext context)
		{
			Assert.IsNotNull(context.ContractType);

			var bindings = GetBindingList(context.ContractType);

			for (int i = 0; i < bindings.Count; i++)
			{
				var binding = bindings[i];

				if (binding.Condition(context))
					return binding;
			}

			return null;
		}

		public IBinding GetBinding(Type contractType)
		{
			Assert.IsNotNull(contractType);

			return GetBindingList(contractType).Last();
		}

		public IBinding GetBinding<TContract>()
		{
			return GetBinding(typeof(TContract));
		}

		public IEnumerable<IBinding> GetBindings(InjectionContext context)
		{
			Assert.IsNotNull(context.ContractType);

			return GetBindingList(context.ContractType)
				.Where(b => b.Condition(context));
		}

		public IEnumerable<IBinding> GetBindings(Type contractType)
		{
			Assert.IsNotNull(contractType);

			return GetBindingList(contractType)
				.AsEnumerable();
		}

		public IEnumerable<IBinding> GetBindings<TContract>()
		{
			return GetBindings(typeof(TContract));
		}

		public IEnumerable<IBinding> GetAllBindings()
		{
			return contractTypeToBindings
				.SelectMany(pair => pair.Value);
		}

		List<IBinding> GetBindingList(Type contractType)
		{
			List<IBinding> bindings;

			if (!contractTypeToBindings.TryGetValue(contractType, out bindings))
			{
				bindings = new List<IBinding>();
				contractTypeToBindings[contractType] = bindings;
			}

			return bindings;
		}
	}
}
