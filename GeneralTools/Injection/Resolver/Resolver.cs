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

		public TContract Resolve<TContract>() where TContract : class
		{
			return (TContract)Resolve(typeof(TContract));
		}

		public object[] ResolveAll(Type contractType)
		{
			return ResolveAll<object>(contractType);
		}

		public TContract[] ResolveAll<TContract>() where TContract : class
		{
			return ResolveAll<TContract>(typeof(TContract));
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

		public void Register(Type contractType, FactoryData data)
		{
			GetFactoryDataList(contractType).Add(data);
		}

		public void Unregister(Type contractType)
		{
			typeToFactoryData.Remove(contractType);
		}

		public void UnregisterAll()
		{
			typeToFactoryData.Clear();
		}

		public void Sort(Type contractType)
		{
			GetFactoryDataList(contractType).Sort((a, b) => b.Conditions.Count.CompareTo(a.Conditions.Count));
		}

		T[] ResolveAll<T>(Type contractType) where T : class
		{
			var context = new InjectionContext
			{
				Binder = binder,
				ContractType = contractType
			};

			return GetFactoryDataList(contractType)
				.Select(data => (T)data.Factory.Create(context))
				.ToArray();
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

					if (CheckConditions(data, ref context))
						return data;
				}
			}

			return null;
		}

		bool CheckConditions(FactoryData data, ref InjectionContext context)
		{
			for (int i = 0; i < data.Conditions.Count; i++)
			{
				if (!data.Conditions[i](context))
					return false;
			}

			return true;
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
