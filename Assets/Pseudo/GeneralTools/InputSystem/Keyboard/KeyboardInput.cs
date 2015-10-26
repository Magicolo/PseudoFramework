using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	[System.Serializable]
	public class KeyboardInput {

		readonly string keyboardName;
		public string KeyboardName {
			get {
				return keyboardName;
			}
		}
		
		readonly string inputName;
		public string InputName {
			get {
				return inputName;
			}
		}
		
		public KeyboardInput(string keyboardName, string inputName) {
			this.keyboardName = keyboardName;
			this.inputName = inputName;
		}
		
		public override string ToString() {
			return string.Format("{0}({1}, {2})", GetType().Name, KeyboardName, InputName);
		}
	}
}