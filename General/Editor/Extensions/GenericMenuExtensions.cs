using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Editor.Internal;

namespace Pseudo.Editor.Internal
{
	public static class GenericMenuExtensions
	{
		public static void AddItem(this GenericMenu menu, FlagsOption option, GenericMenu.MenuFunction2 callback)
		{
			menu.AddItem(option.Label, option.IsSelected, callback, option);
		}
	}
}