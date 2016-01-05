using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;
using System.Reflection;

namespace Pseudo.Internal.Entity
{
	[CustomPropertyDrawer(typeof(GroupDefinition), true)]
	public class GroupDefinitionDrawer : CustomPropertyDrawerBase
	{
		static List<GroupData> groups;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			ShowGroups();

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			if (groups == null)
				InitializeGroups();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			return 16f;
		}

		void ShowGroups()
		{
			var groupProperty = currentProperty.FindPropertyRelative("groups");
			var flags = groupProperty.GetValue<ByteFlag>();
			var options = new FlagsOption[groups.Count];

			for (int i = 0; i < options.Length; i++)
			{
				var group = groups[i];
				var name = group.GroupName.Replace('_', '/').ToGUIContent();
				options[i] = new FlagsOption(name, group, EntityMatch.Matches(flags, group.Group.Groups));
			}

			Flags(currentPosition, groupProperty, options, OnGroupSelected, currentLabel);
		}

		void InitializeGroups()
		{
			groups = new List<GroupData>();
			var types = TypeExtensions.GetSubclasses(typeof(EntityGroupDefinition));

			foreach (var type in types)
			{
				foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.ExactBinding))
				{
					if (field.IsStatic && typeof(EntityGroupDefinition).IsAssignableFrom(field.FieldType))
						groups.Add(new GroupData(type.GetName(), field.Name, (GroupDefinition)field.GetValue(null)));
				}
			}
		}

		void OnGroupSelected(FlagsOption option, SerializedProperty property)
		{
			var flags = property.GetValue<ByteFlag>();

			switch (option.Type)
			{
				case FlagsOption.OptionTypes.Everything:
					foreach (var group in GroupDefinitionDrawer.groups)
						flags |= group.Group.Groups;
					break;
				case FlagsOption.OptionTypes.Nothing:
					flags = ByteFlag.Nothing;
					break;
				case FlagsOption.OptionTypes.Custom:
					var groups = ((GroupData)option.Value).Group.Groups;

					if (option.IsSelected)
						flags &= ~groups;
					else
						flags |= groups;
					break;
			}

			for (int i = 1; i <= 8; i++)
			{
				var flagName = "f" + i;
				property.FindPropertyRelative(flagName).intValue = flags.GetValueFromMember<int>(flagName);
			}

			property.serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(property.serializedObject.targetObject);
		}

		public class GroupData
		{
			public readonly string OwnerName;
			public readonly string GroupName;
			public readonly GroupDefinition Group;

			public GroupData(string ownerName, string groupName, GroupDefinition group)
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