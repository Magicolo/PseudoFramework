using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Pseudo
{
	public class SystemInstaller : MonoBehaviour
	{
		[Serializable]
		public class SystemData
		{
			public string TypeName;
			public bool Active;

			public Type Type
			{
				get { return type ?? (type = Type.GetType(TypeName)); }
			}

			Type type;
		}

		public ISystemManager SystemManager;
		public SystemData[] Systems = new SystemData[0];

		[PostInject]
		void Initialize(ISystemManager systemManager)
		{
			SystemManager = systemManager;

			if (Systems == null)
				return;

			for (int i = 0; i < Systems.Length; i++)
			{
				var system = Systems[i];

				if (system != null && system.Type != null)
					systemManager.AddSystem(system.Type, system.Active);
			}
		}

		void OnDestroy()
		{
			SystemManager.RemoveAllSystems();
		}
	}
}
