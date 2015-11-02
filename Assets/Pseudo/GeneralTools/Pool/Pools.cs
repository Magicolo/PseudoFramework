using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public static class Pools
	{
		public static readonly BehaviourPoolManager<PMonoBehaviour> BehaviourPool = new BehaviourPoolManager<PMonoBehaviour>();
		public static readonly ComponentPoolManager<Component> ComponentPool = new ComponentPoolManager<Component>();
		public static readonly GameObjectPoolManager GameObjectPool = new GameObjectPoolManager();
		public static readonly ScriptablePoolManager<PScriptableObject> ScriptablePool = new ScriptablePoolManager<PScriptableObject>();
	}
}