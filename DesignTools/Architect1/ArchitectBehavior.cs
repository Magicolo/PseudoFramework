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
	public class ArchitectBehavior : MonoBehaviour
	{
		[Inject(),Disable()]
		public ArchitectControler Architect;
		[Inject(), Disable()]
		public ArchitectCameraControler CameraControler;
		[Inject(), Disable()]
		public ArchitectLayerControler LayerControler;
		[Inject(), Disable()]
		public ArchitectToolControler ToolControler;
		[Inject(), Disable()]
		public DrawingControler DrawingControler;
		

		void Start()
		{
			callMethod(CameraControler, "Start");
			callMethod(LayerControler, "Start");
			callMethod(ToolControler, "Start");
			callMethod(DrawingControler, "Start");
		}

		void Update() 
		{
			callMethod(CameraControler,"Update");
			callMethod(LayerControler, "Update");
			callMethod(ToolControler, "Update");
			callMethod(DrawingControler, "Update");
		}

		void callMethod(System.Object obj, string methodName)
		{
			Type type = obj.GetType();
			MethodInfo mi = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if(mi != null)
				mi.Invoke(obj, null);
		}
	}
}
