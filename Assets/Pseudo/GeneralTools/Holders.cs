using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo.Internal
{
	public static class Holders
	{
		static bool _poolsCached;
		static GameObject _pools;
		static bool _managersCached;
		static GameObject _managers;

		public static GameObject Pools
		{
			get
			{
				if (!_poolsCached)
				{
					_pools = GameObject.Find("Pools");

					if (_pools == null)
						_pools = new GameObject("Pools");

					UnityEngine.Object.DontDestroyOnLoad(_pools);
					_poolsCached = true;
				}

				return _pools;
			}
		}
		public static GameObject Managers
		{
			get
			{
				if (!_managersCached)
				{
					_managers = GameObject.Find("Managers");

					if (_managers == null)
						_managers = new GameObject("Managers");

					UnityEngine.Object.DontDestroyOnLoad(_managers);
					_managersCached = true;
				}

				return _managers;
			}
		}
	}
}