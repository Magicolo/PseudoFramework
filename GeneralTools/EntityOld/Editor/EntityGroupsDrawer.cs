using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEditor;
using System.Reflection;
using Pseudo.Internal.EntityOld;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.EntityOld
{
	[CustomPropertyDrawer(typeof(EntityGroupsAttribute))]
	public class EntityGroupsDrawer : CustomAttributePropertyDrawerBase
	{
		static List<GroupData> groupData;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			ShowGroups();

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			if (groupData == null)
				InitializeGroups();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			return 16f;
		}

		void ShowGroups()
		{
			var flags = currentProperty.GetValue<ByteFlag>();
			var options = new FlagsOption[groupData.Count];

			for (int i = 0; i < options.Length; i++)
			{
				var group = groupData[i];
				var name = group.GroupName.Replace('_', '/').ToGUIContent();
				options[i] = new FlagsOption(name, group, EntityMatchOld.Matches(flags, group.Group));
			}

			Flags(currentPosition, currentProperty, options, OnGroupSelected, currentLabel);
		}

		void InitializeGroups()
		{
			groupData = new List<GroupData>();
			var types = typeof(EntityGroupsAttribute).GetDefinedTypes();

			foreach (var type in types)
			{
				if (type.IsSealed && type.IsAbstract)
				{
					foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
					{
						if (field.IsStatic && field.IsInitOnly && typeof(ByteFlag).IsAssignableFrom(field.FieldType))
							groupData.Add(new GroupData(type.GetName(), field.Name, (ByteFlag)field.GetValue(null)));
					}
				}
			}
		}

		void OnGroupSelected(FlagsOption option, SerializedProperty property)
		{
			var groups = property.GetValue<ByteFlag>();

			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					foreach (var data in groupData)
						groups |= data.Group;
					break;
				case FlagsOption.OptionTypes.Nothing:
					groups = ByteFlag.Nothing;
					break;
				case FlagsOption.OptionTypes.Custom:
					var group = ((GroupData)option.Value).Group;

					if (option.IsSelected)
						groups &= ~group;
					else
						groups |= group;
					break;
			}

			for (int i = 1; i <= 8; i++)
			{
				var flagName = "f" + i;
				property.FindPropertyRelative(flagName).intValue = groups.GetValueFromMember<int>(flagName);
			}

			property.serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(property.serializedObject.targetObject);
		}

		public class GroupData
		{
			public readonly string OwnerName;
			public readonly string GroupName;
			public readonly ByteFlag Group;

			public GroupData(string ownerName, string groupName, ByteFlag group)
			{
				OwnerName = ownerName;
				GroupName = groupName;
				Group = group;
			}

			public override string ToString()
			{
				return string.Format("{0}({1}, {2})", GetType().Name, OwnerName, GroupName);
			}
		}
	}
}