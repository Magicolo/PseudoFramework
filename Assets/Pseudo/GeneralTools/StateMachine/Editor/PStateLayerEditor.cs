using System;
using Pseudo;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(PStateLayer), true), CanEditMultipleObjects]
	public class PStateLayerEditor : CustomEditorBase
	{
		PStateLayer layer;

		public override void OnEnable()
		{
			base.OnEnable();

			layer = (PStateLayer)target;

			if (layer.Machine == null)
			{
				Type layerType = layer.GetType();
				PStateMachine machine = layer.CachedGameObject.GetOrAddComponent<PStateMachine>();
				PStateMachineUtility.AddLayer(machine, layerType, machine);
			}
		}
	}
}

