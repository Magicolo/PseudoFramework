using System;
using Pseudo;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(State), true), CanEditMultipleObjects]
	public class StateEditor : CustomEditorBase
	{
		State state;

		public override void OnEnable()
		{
			base.OnEnable();

			state = (State)target;

			if (state.Machine == null)
			{
				Type layerType = StateMachineUtility.GetLayerTypeFromState(state);
				StateMachine machine = state.CachedGameObject.GetOrAddComponent<StateMachine>();
				StateMachineUtility.AddLayer(machine, layerType, machine);
			}
		}
	}
}