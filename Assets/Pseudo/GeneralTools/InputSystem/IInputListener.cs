using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo {
	public interface IInputListener {
		
		void OnButtonInput(ButtonInput input);
		
		void OnAxisInput(AxisInput input);
	}
}
