using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo {
	[System.Serializable] 
	public class ButtonInput {

		readonly string controllerName;
		public string ControllerName {
			get {
				return controllerName;
			}
		}
		
		readonly string inputName;
		public string InputName {
			get {
				return inputName;
			}
		}
		
		readonly ButtonStates state;
		public ButtonStates State {
			get {
				return state;
			}
		}
	
		public ButtonInput(string controllerName, string inputName, ButtonStates state) {
			this.controllerName = controllerName;
			this.inputName = inputName;
			this.state = state;
		}
		
		public override string ToString() {
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, ControllerName, InputName, State);
		}
	}
}