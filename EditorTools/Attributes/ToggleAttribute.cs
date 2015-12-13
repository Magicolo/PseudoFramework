using System;
using UnityEngine;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ToggleAttribute : CustomAttributeBase
	{
		public GUIContent trueLabel;
		public GUIContent falseLabel;

		public ToggleAttribute() { }

		public ToggleAttribute(string trueLabel, string falseLabel)
		{
			this.trueLabel = trueLabel.ToGUIContent();
			this.falseLabel = falseLabel.ToGUIContent();
		}

		public ToggleAttribute(GUIContent trueLabel, GUIContent falseLabel)
		{
			this.trueLabel = trueLabel;
			this.falseLabel = falseLabel;
		}
	}
}