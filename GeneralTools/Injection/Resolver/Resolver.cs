using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class Resolver : IResolver
	{
		public IBinder Binder
		{
			get { return binder; }
		}

		readonly IBinder binder;
		readonly Dictionary<Type, List<FactoryData>> typeToFactoryData = new Dictionary<Type, List<FactoryData>>();

		public Resolver(IBinder binder)
		{
			this.binder = binder;
		}

		public object Resolve(Type contractType)
		{
			var data = GetValidData(contractType);

			if (data != null)
				return data.Factory.Create(new InjectionContext
				{
					Binder = binder,
					ContractType = contractType
				});
			else if (binder.Parent != null)
				return binder.Parent.Resolver.Resolve(contractType);

			throw new ArgumentException(string.Format("No binding was found for type {0}.", contractType.Name));
		}

		public object Resolve(InjectionContext context)
		{
			var data = GetValidData(ref context);

			if (data != null)
				return data.Factory.Create(context);
			else if (binder.Parent != null)
				return binder.Parent.Resolver.Resolve(context);

			throw new ArgumentException(string.Format("No binding was found for context {0}.", context));
		}

		public TContract Resolve<TContract>()
		{
			return (TContract)Resolve(typeof(TContract));
		}

		public IEnumerable<object> ResolveAll(Type contractType)
		{
			var context = new InjectionContext
			{
				Binder = binder,
				ContractType = contractType
			};

			if (binder.Parent == null)
				return GetFactoryDataList(contractType)
					.Select(data => data.Factory.Create(context));
			else
				return GetFactoryDataList(contractType)
					.Select(data => data.Factory.Create(context))
					.Concat(binder.Parent.Resolver.ResolveAll(contractType));
		}

		public IEnumerable<TContract> ResolveAll<TContract>()
		{
			var context = new InjectionContext
			{
				Binder = binder,
				ContractType = typeof(TContract)
			};

			if (binder.Parent == null)
				return GetFactoryDataList(typeof(TContract))
					.Select(data => (TContract)data.Factory.Create(context));
			else
				return GetFactoryDataList(typeof(TContract))
					.Select(data => (TContract)data.Factory.Create(context))
					.Concat(binder.Parent.Resolver.ResolveAll<TContract>());
		}

		public bool CanResolve(Type contractType)
		{
			return
				GetValidData(contractType) != null ||
				(binder.Parent != null && binder.Parent.Resolver.CanResolve(contractType));
		}

		public bool CanResolve(params Type[] contractTypes)
		{
			for (int i = 0; i < contractTypes.Length; i++)
			{
				if (!CanResolve(contractTypes[i]))
					return false;
			}

			return true;
		}

		public bool CanResolve(InjectionContext context)
		{
			return
				GetValidData(ref context) != null ||
				(binder.Parent != null && binder.Parent.Resolver.CanResolve(context));
		}

		public void AddFactory(Type contractType, FactoryData data)
		{
			GetFactoryDataList(contractType).Add(data);
		}

		public void RemoveFactory(Type contractType, FactoryData data)
		{
			GetFactoryDataList(contractType).Remove(data);
		}

		public void RemoveFactories(Type contractType)
		{
			typeToFactoryData.Remove(contractType);
		}

		public void RemoveAllFactories()
		{
			typeToFactoryData.Clear();
		}

		FactoryData GetValidData(Type contractType)
		{
			List<FactoryData> dataList;

			if (typeToFactoryData.TryGetValue(contractType, out dataList))
				return dataList.Last();

			return null;
		}

		FactoryData GetValidData(ref InjectionContext context)
		{
			List<FactoryData> dataList;

			if (typeToFactoryData.TryGetValue(context.ContractType, out dataList))
			{
				for (int i = 0; i < dataList.Count; i++)
				{
					var data = dataList[i];

					if (data.Condition(context))
						return data;
				}
			}

			return null;
		}

		List<FactoryData> GetFactoryDataList(Type contractType)
		{
			List<FactoryData> dataList;

			if (!typeToFactoryData.TryGetValue(contractType, out dataList))
			{
				dataList = new List<FactoryData>();
				typeToFactoryData[contractType] = dataList;
			}

			return dataList;
		}
	}
}
