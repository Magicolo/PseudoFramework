using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;


namespace Pseudo.Architect
{
	public abstract class ArchitectControlerBase : MonoBehaviour
	{
		[Inject(), Disable()]
		protected ArchitectControler Architect;
		[Inject(), Disable()]
		protected ArchitectCameraControler CameraControler;
		[Inject(), Disable()]
		protected ArchitectLayerControler LayerControler;
		[Inject(), Disable()]
		protected ArchitectToolControler ToolControler;
		[Inject(), Disable()]
		protected DrawingControler DrawingControler;

	}
}