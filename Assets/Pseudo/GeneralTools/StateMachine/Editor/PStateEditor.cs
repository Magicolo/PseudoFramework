using System;
using Pseudo;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(PState), true), CanEditMultipleObjects]
	public class PStateEditor : CustomEditorBase
	{
		PState state;

		public override void OnEnable()
		{
			base.OnEnable();

			state = (PState)target;

			if (state.Machine == null)
			{
				Type layerType = PStateMachineUtility.GetLayerTypeFromState(state);
				PStateMachine machine = state.CachedGameObject.GetOrAddComponent<PStateMachine>();
				PStateMachineUtility.AddLayer(machine, layerType, machine);
			}
		}
	}
}