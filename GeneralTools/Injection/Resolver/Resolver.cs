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
		public IResolver Parent
		{
			get { return parent; }
		}

		readonly IResolver parent;
		readonly Dictionary<Type, List<FactoryData>> typeToFactoryData = new Dictionary<Type, List<FactoryData>>();

		public Resolver(IResolver parent)
		{
			this.parent = parent;
		}

		public object Resolve(Type contractType)
		{
			return Resolve(contractType, InjectionUtility.Empty);
		}

		public object Resolve(Type contractType, params object[] additional)
		{
			var data = GetValidData(contractType);

			if (data != null)
				return data.Factory.Create(additional);
			else if (parent != null)
				return parent.Resolve(contractType, additional);

			throw new ArgumentException(string.Format("No binding was found for type {0}.", contractType.Name));
		}

		public object Resolve(InjectionContext context)
		{
			context.Resolver = this;

			var data = GetValidData(ref context);

			if (data != null)
				return data.Factory.Create(context.Additional);
			else if (parent != null)
				return parent.Resolve(context);

			throw new ArgumentException(string.Format("No binding was found for context {0}.", context));
		}

		public TContract Resolve<TContract>() where TContract : class
		{
			return (TContract)Resolve(typeof(TContract), InjectionUtility.Empty);
		}

		public TContract Resolve<TContract>(params object[] additional) where TContract : class
		{
			return (TContract)Resolve(typeof(TContract), additional);
		}

		public bool CanResolve(Type contractType)
		{
			return GetValidData(contractType) != null || (parent != null && parent.CanResolve(contractType));
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
			return GetValidData(ref context) != null || (parent != null && parent.CanResolve(context));
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
