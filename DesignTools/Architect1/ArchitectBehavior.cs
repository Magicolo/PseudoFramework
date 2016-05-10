using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;
using System.Reflection;

namespace Pseudo.Architect
{
	[Serializable]
	public class ArchitectBehavior : MonoBehaviour
	{
		[Inject(), Disable(), NonSerialized()]
		ArchitectControler Architect = null;
		[Inject(), Disable(), NonSerialized()]
		ArchitectCameraControler CameraControler = null;
		[Inject(), Disable(), NonSerialized()]
		ArchitectLayerControler LayerControler = null;
		[Inject(), Disable(), NonSerialized()]
		ArchitectToolControler ToolControler = null;
		[Inject(), Disable(), NonSerialized()]
		DrawingControler DrawingControler = null;

		public UISkin Skin;
		public UIFactory UIFactory;

		void Start()
		{
			callMethod(CameraControler, "Start");
			callMethod(LayerControler, "Start");
			callMethod(ToolControler, "Start");
			callMethod(DrawingControler, "Start");
		}

		void Update()
		{
			callMethod(CameraControler, "Update");
			callMethod(LayerControler, "Update");
			callMethod(ToolControler, "Update");
			callMethod(DrawingControler, "Update");
		}

		void callMethod(System.Object obj, string methodName)
		{
			Type type = obj.GetType();
			MethodInfo mi = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if (mi != null)
				mi.Invoke(obj, null);
		}
	}
}
