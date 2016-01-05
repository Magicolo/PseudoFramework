using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public partial class PEntity : IPoolInitializable, IPoolSettersInitializable, ISerializationCallbackReceiver
	{
		[SerializeField, DoNotInitialize]
		ReferenceData[] references = new ReferenceData[0];
		[SerializeField, DoNotInitialize]
		string data;
		[SerializeField]
		bool isDeserialized;

		void Awake()
		{
			ComponentSerializer.InjectReferences(allComponents, references);
		}

		void SerializeComponents()
		{
			ComponentSerializer.SerializeComponents(allComponents, out data, out references);
		}

		void DeserializeComponents()
		{
			if ((isDeserialized && ApplicationUtility.IsPlaying) || string.IsNullOrEmpty(data))
				return;

			RemoveAllComponents(false);
			allComponents = ComponentSerializer.DeserializeComponents(data, references);
			RegisterAllComponents();
			isDeserialized = ApplicationUtility.IsPlaying;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (ApplicationUtility.IsPlaying)
				return;

			SerializeComponents();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			DeserializeComponents();
		}

		void IPoolSettersInitializable.OnPrePoolSettersInitialize()
		{
			DeserializeComponents();
		}

		void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }

		void IPoolInitializable.OnPrePoolInitialize() { }

		void IPoolInitializable.OnPostPoolInitialize()
		{
			RegisterAllComponents();
			ComponentSerializer.InjectReferences(allComponents, references);
		}
	}
}