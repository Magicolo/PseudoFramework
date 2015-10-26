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
		StateLayer _layer;

		public override void OnEnable()
		{
			base.OnEnable();

			_layer = (StateLayer)target;

			if (_layer.Machine == null)
			{
				Type layerType = _layer.GetType();
				StateMachine machine = _layer.GetOrAddComponent<StateMachine>();
				StateMachineUtility.AddLayer(machine, layerType, machine);
			}
		}
	}
}

