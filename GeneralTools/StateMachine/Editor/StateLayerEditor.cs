using System;
using Pseudo;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(StateLayer), true), CanEditMultipleObjects]
	public class StateLayerEditor : CustomEditorBase
	{
		StateLayer layer;

		public override void OnEnable()
		{
			base.OnEnable();

			layer = (StateLayer)target;

			if (layer.Machine == null)
			{
				Type layerType = layer.GetType();
				StateMachine machine = layer.CachedGameObject.GetOrAddComponent<StateMachine>();
				StateMachineUtility.AddLayer(machine, layerType, machine);
			}
		}
	}
}

