using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	public static class ScriptUtility
	{
		public static MonoScript[] AllScripts = MonoImporter.GetAllRuntimeMonoScripts();

		static Dictionary<Type, MonoScript> typeScripts = new Dictionary<Type, MonoScript>();

		public static MonoScript FindScript(Type type)
		{
			MonoScript script;

			if (!typeScripts.TryGetValue(type, out script))
			{
				for (int i = 0; i < AllScripts.Length; i++)
				{
					var typeScript = AllScripts[i];

					if (typeScript.GetClass() == type)
					{
						script = typeScript;
						break;
					}
				}

				typeScripts[type] = script;
			}

			return script;
		}
	}
}