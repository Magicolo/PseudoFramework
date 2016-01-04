using UnityEditor;
using UnityEngine;

namespace Pseudo.Internal.Editor
{
	public class FlagsOption
	{
		public enum OptionTypes
		{
			Everything,
			Nothing,
			Custom
		}

		public readonly GUIContent Label;
		public readonly object Value;
		public readonly OptionTypes Type;
		public readonly bool IsSelected;

		public FlagsOption(GUIContent label, object value, bool isSelected) : this(label, value, isSelected, OptionTypes.Custom) { }

		FlagsOption(GUIContent label, object value, bool isSelected, OptionTypes type)
		{
			Label = label;
			Value = value;
			Type = type;
			IsSelected = isSelected;
		}

		public static FlagsOption GetEverything(bool isSelected)
		{
			return new FlagsOption("Everything".ToGUIContent(), null, isSelected, OptionTypes.Everything);
		}

		public static FlagsOption GetNothing(bool isSelected)
		{
			return new FlagsOption("Nothing".ToGUIContent(), null, isSelected, OptionTypes.Nothing);
		}
	}
}