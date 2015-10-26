using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Pseudo {
	public static class LayerMaskExtensions {

		public static bool Contains(this LayerMask layerMask, int layer) {
			return (layerMask.value & (1 << layer)) != 0;
		}
		
		public static bool Contains(this LayerMask layerMask, string layer) {
			return (layerMask.value & LayerMask.NameToLayer(layer)) != 0;
		}
		
		public static LayerMask Inverse(this LayerMask layerMask) {
			return ~layerMask;
		}
 
		public static LayerMask AddToMask(this LayerMask layerMask, params int[] layers) {
			foreach (int layer in layers) {
				layerMask |= (1 << layer);
			}
			
			return layerMask;
		}
	
		public static LayerMask AddToMask(this LayerMask layerMask, params string[] layers) {
			foreach (string layer in layers) {
				layerMask |= (1 << LayerMask.NameToLayer(layer));
			}
			
			return layerMask;
		}
 
		public static LayerMask RemoveFromMask(this LayerMask layerMask, params string[] layers) {
			layerMask = layerMask.Inverse();
			layerMask = layerMask.AddToMask(layers);
			
			return layerMask.Inverse();
		}
	
		public static LayerMask RemoveFromMask(this LayerMask layerMask, params int[] layers) {
			layerMask = layerMask.Inverse();
			layerMask = layerMask.AddToMask(layers);
			
			return layerMask.Inverse();
		}
	
		public static string[] LayerNames(this LayerMask layerMask) {
			var names = new List<string>();
 
			for (int i = 0; i < 32; ++i) {
				int shifted = 1 << i;
				if ((layerMask & shifted) == shifted) {
					string layerName = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(layerName)) {
						names.Add(layerName);
					}
				}
			}
			
			return names.ToArray();
		}
	}
}
