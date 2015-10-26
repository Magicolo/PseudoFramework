﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo {
	public static class EnumExtensions {
	
		public static T ConvertByName<T>(this Enum e) {
			return (T)Enum.Parse(typeof(T), e.ToString());
		}
		
		public static T ConvertByIndex<T>(this Enum e) {
			string[] enumNames = Enum.GetNames(typeof(T));
			return (T)Enum.Parse(typeof(T), enumNames[Mathf.Clamp(e.GetHashCode(), 0, Mathf.Max(enumNames.Length - 1, 0))]);
		}
		
		public static T ConvertByValue<T>(this Enum e) {
			return (T)(object)e;
		}

		#region Axis
		public static bool Contains(this Axes axes, Axes axis) {
			return (axes & axis) != 0;
		}
		#endregion
		
		#region Channels
		public static bool Contains(this Channels channels, Channels otherChannels) {
			return (channels & otherChannels) != 0;
		}
		#endregion
		
		#region TransformModes
		public static bool Contains(this TransformModes mode, TransformModes otherMode) {
			return (mode & otherMode) != 0;
		}
		#endregion
	}
}
