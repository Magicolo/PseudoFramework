using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Pseudo.Internal.Editor
{
	public class CustomWindowBase<T> : EditorWindow where T : CustomWindowBase<T>
	{
		protected const char keySeparator = '¦';

		public static T Instance;

		public virtual void OnSelectionChange()
		{
		}

		public virtual void SetDefaultValues()
		{
		}

		protected void Save()
		{
			FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];
				SetValue(field.Name, field.GetValue(this), GetType());
			}
		}

		protected void Load()
		{
			FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];

				if (HasKey(field.Name, GetType()))
					field.SetValue(this, GetValue(field.Name, field.FieldType, GetType()));
			}
		}

		protected static object GetValue(string key, System.Type valueType, System.Type settingsType)
		{
			key = settingsType.Name + " " + key;

			object value = null;

			if (valueType == typeof(int))
				value = EditorPrefs.GetInt(key);
			else if (valueType == typeof(float))
				value = EditorPrefs.GetFloat(key);
			else if (valueType == typeof(bool))
				value = EditorPrefs.GetBool(key);
			else if (valueType == typeof(string))
				value = EditorPrefs.GetString(key);

			return value;
		}

		protected static TV GetValue<TV>(string key, System.Type settingsType)
		{
			return (TV)GetValue(key, typeof(TV), settingsType);
		}

		protected static void SetValue(string key, object value, System.Type settingsType)
		{
			key = settingsType.Name + " " + key;

			List<string> keyList = new List<string>(GetKeys(settingsType));

			if (!keyList.Contains(key))
			{
				keyList.Add(key);
				EditorPrefs.SetString(settingsType.Name + " keys", keyList.Concat(keySeparator));
			}

			if (value is int)
				EditorPrefs.SetInt(key, (int)value);
			else if (value is float)
				EditorPrefs.SetFloat(key, (float)value);
			else if (value is bool)
				EditorPrefs.SetBool(key, (bool)value);
			else if (value is string)
				EditorPrefs.SetString(key, (string)value);
		}

		protected static bool HasKey(string key, System.Type settingsType)
		{
			return EditorPrefs.HasKey(settingsType.Name + " " + key);
		}

		protected static string[] GetKeys(System.Type settingsType)
		{
			return EditorPrefs.GetString(settingsType.Name + " keys").Split(keySeparator);
		}

		protected static void DeleteKey(string key, System.Type settingsType)
		{
			List<string> keyList = new List<string>(GetKeys(settingsType));
			keyList.Remove(key);
			EditorPrefs.SetString(settingsType.Name + " keys", keyList.Concat(keySeparator));
			EditorPrefs.DeleteKey(key);
		}

		public static T CreateWindow(string name, Vector2 size)
		{
			Instance = GetWindow<T>(name, true);
			Instance.position = new Rect(Screen.currentResolution.width / 2 - size.x / 2, Screen.currentResolution.height / 2 - size.y / 2, size.x, size.y);
			Instance.minSize = size;
			Instance.SetDefaultValues();
			Instance.Load();
			Instance.OnSelectionChange();

			return Instance;
		}
	}
}
