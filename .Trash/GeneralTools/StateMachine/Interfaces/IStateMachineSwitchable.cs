using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public interface IStateMachineSwitchable {

		T SwitchState<T>(int index = 0) where T : IState ;
				
		IState SwitchState(System.Type stateType, int index = 0);
		
		IState SwitchState(string stateName, int index = 0);
		
		IState[] SwitchStates<T>(params int[] indices) where T : IState;
		
		IState[] SwitchStates(System.Type stateType, params int[] indices);
		
		IState[] SwitchStates(string stateName, params int[] indices);
		
		bool StateIsActive<T>(int index = 0) where T : IState;
		
		bool StateIsActive(System.Type stateType, int index = 0);
		
		bool StateIsActive(string stateName, int index = 0);
		
		T GetActiveState<T>(int index = 0) where T : IState;
		
		IState GetActiveState(int index = 0);
		
		IState[] GetActiveStates();
	}
}

