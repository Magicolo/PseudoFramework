using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo {
	[System.Serializable] 
	public class AxisInput {

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
		
		readonly float value;
		public float Value {
			get {
				return value;
			}
		}
	
		public AxisInput(string controllerName, string inputName, float value) {
			this.controllerName = controllerName;
			this.inputName = inputName;
			this.value = value;
		}
		
		public override string ToString() {
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, ControllerName, InputName, Value);
		}
	}
}