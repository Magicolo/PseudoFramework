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
		static readonly IBindingSelector defaultSelector = new BindingSelector();

		public IContainer Container
		{
			get { return container; }
		}
		public IBindingSelector BindingSelector
		{
			get { return selector; }
			set { selector = value ?? defaultSelector; }
		}

		readonly IContainer container;
		IBindingSelector selector = defaultSelector;
		readonly HashSet<IBinding> allBindings = new HashSet<IBinding>();
		readonly Dictionary<Type, List<IBinding>> contractTypeToBindings = new Dictionary<Type, List<IBinding>>();

		public Binder(IContainer container)
		{
			this.container = container;
		}

		public void Bind(IBinding binding)
		{
			Assert.IsNotNull(binding);

			if (allBindings.Add(binding))
			{
				GetBindingList(binding.ContractType).Add(binding);

				for (int i = 0; i < binding.BaseTypes.Length; i++)
					GetBindingList(binding.BaseTypes[i]).Add(binding);
			}
		}

		public void Bind(Assembly assembly)
		{
			Assert.IsNotNull(assembly);

			var types = assembly.GetTypes();

			for (int i = 0; i < types.Length; i++)
			{
				var type = types[i];

				// Prevents expensive and useless analysis of types that have no BindAttributeBase.
				if (!type.IsDefined(typeof(BindAttributeBase), true))
					continue;

				var installers = container.Analyzer.Analyze(types[i]).Installers;

				for (int j = 0; j < installers.Length; j++)
					installers[j].Install(container);
			}
		}

		public IBindingContract Bind(Type contractType, params Type[] baseTypes)
		{
			Assert.IsNotNull(contractType);
			Assert.IsNotNull(baseTypes);
			Assert.IsTrue(Array.TrueForAll(baseTypes, t => contractType.Is(t)));

			return new BindingContract(contractType, baseTypes, container);
		}

		public IBindingContract<TContract> Bind<TContract>(params Type[] baseTypes)
		{
			Assert.IsNotNull(baseTypes);
			Assert.IsTrue(Array.TrueForAll(baseTypes, t => typeof(TContract).Is(t)));

			return new BindingContract<TContract>(baseTypes, container);
		}

		public IBindingContract BindAll(Type contractType)
		{
			Assert.IsNotNull(contractType);

			return Bind(contractType, TypeUtility.GetBaseTypes(contractType, false, true).ToArray());
		}

		public IBindingContract<TContract> BindAll<TContract>()
		{
			return Bind<TContract>(TypeUtility.GetBaseTypes(typeof(TContract), false, true).ToArray());
		}

		public void Unbind(IBinding binding)
		{
			Assert.IsNotNull(binding);

			if (allBindings.Remove(binding))
			{
				GetBindingList(binding.ContractType).Remove(binding);

				for (int i = 0; i < binding.BaseTypes.Length; i++)
					GetBindingList(binding.BaseTypes[i]).Remove(binding);
			}
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

		public void UnbindAll(Type contractType)
		{
			Assert.IsNotNull(contractType);

			Unbind(contractType);
			Unbind(TypeUtility.GetBaseTypes(contractType, false, true).ToArray());
		}

		public void UnbindAll()
		{
			allBindings.Clear();
			contractTypeToBindings.Clear();
		}

		public bool HasBinding(IBinding binding)
		{
			return allBindings.Contains(binding);
		}

		public bool HasBinding(InjectionContext context)
		{
			Assert.IsNotNull(context.ContractType);

			return GetBindingList(context.ContractType).Count > 0;
		}

		public IBinding GetBinding(InjectionContext context)
		{
			Assert.IsNotNull(context.ContractType);

			return selector.Select(context, GetBindingList(context.ContractType));
		}

		public IEnumerable<IBinding> GetBindings(InjectionContext context)
		{
			Assert.IsNotNull(context.ContractType);

			return selector.SelectAll(context, GetBindingList(context.ContractType));
		}

		public IEnumerable<IBinding> GetAllBindings()
		{
			return allBindings.AsEnumerable();
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
