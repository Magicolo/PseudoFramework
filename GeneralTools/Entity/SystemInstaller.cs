using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Pseudo
{
	public class SystemInstaller : MonoBehaviour, ISerializationCallbackReceiver
	{
		public ISystemManager SystemManager;
		public Type[] Systems = new Type[0];

		[SerializeField]
		string[] systems = new string[0];

		[PostInject]
		void Initialize(ISystemManager systemManager)
		{
			SystemManager = systemManager;

			for (int i = 0; i < Systems.Length; i++)
			{
				var type = Systems[i];

				if (type != null)
					systemManager.AddSystem(type);
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Systems = systems.Convert(typeName => Type.GetType(typeName));
		}
	}
}
