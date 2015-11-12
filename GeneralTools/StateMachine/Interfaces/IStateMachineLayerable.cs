using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public interface IStateMachineLayerable {

		T GetLayer<T>() where T : IStateLayer;
		
		IStateLayer GetLayer(System.Type layerType);
		
		IStateLayer GetLayer(string layerName);
		
		IStateLayer[] GetLayers();
		
		bool ContainsLayer<T>() where T : IStateLayer;
		
		bool ContainsLayer(System.Type layerType);
		
		bool ContainsLayer(string layerName);
	}
}