using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Text;
using System.IO;
using System.Reflection;

namespace Pseudo.Internal.Entity
{
	public static class ComponentSerializer
	{
		static Dictionary<Type, FieldInfo[]> typeFields = new Dictionary<Type, FieldInfo[]>();

		public static string SerializeComponents(List<IComponentOld> components)
		{
			var writer = new StringBuilder();
			writer.AppendLine(components.Count.ToString());

			for (int i = 0; i < components.Count; i++)
			{
				var component = components[i];
				writer.AppendLine(component.GetType().FullName);
				writer.AppendLine(JsonUtility.ToJson(component));
			}

			return writer.ToString();
		}

		public static void SerializeComponents(List<IComponentOld> components, out string data, out ReferenceData[] references)
		{
			var referenceList = new List<ReferenceData>();

			for (int i = 0; i < components.Count; i++)
			{
				var component = components[i];
				ExtractReferences(component, i, "", referenceList);
			}

			data = SerializeComponents(components);
			references = referenceList.ToArray();
			InjectReferences(components, references);
		}

		public static List<IComponentOld> DeserializeComponents(string data)
		{
			List<IComponentOld> components;

			using (var reader = new StringReader(data))
			{
				int count = int.Parse(reader.ReadLine());
				components = new List<IComponentOld>(count);

				for (int i = 0; i < count; i++)
				{
					var type = Type.GetType(reader.ReadLine());
					var line = reader.ReadLine();

					if (type != null)
					{
						var component = TypePoolManager.Create(type);
						JsonUtility.FromJsonOverwrite(line, component);
						components.Add((IComponentOld)component);
					}
				}
			}

			return components;
		}

		public static List<IComponentOld> DeserializeComponents(string data, ReferenceData[] references)
		{
			var components = DeserializeComponents(data);
			InjectReferences(components, references);

			return components;
		}

		public static void ExtractReferences(object instance, int index, string path, List<ReferenceData> references)
		{
			var fields = GetFields(instance.GetType());

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				var fieldPath = string.IsNullOrEmpty(path) ? field.Name : path + "." + field.Name;
				var value = field.GetValue(instance);

				if (value == null)
					continue;
				else if (value is UnityEngine.Object)
				{
					references.Add(new ReferenceData
					{
						Index = index,
						Path = fieldPath,
						Reference = (UnityEngine.Object)value
					});

					field.SetValue(instance, null);
				}
				else if (value is IList)
				{
					var list = (IList)value;

					for (int j = 0; j < list.Count; j++)
					{
						var element = list[j];

						if (element == null)
							continue;
						else if (element is UnityEngine.Object)
						{
							references.Add(new ReferenceData
							{
								Index = index,
								Path = fieldPath + "." + j,
								Reference = (UnityEngine.Object)element
							});

							list[j] = null;
						}
						else if (element.GetType().IsClass)
							ExtractReferences(element, index, fieldPath + "." + j, references);
					}
				}
				else if (field.FieldType.IsClass)
					ExtractReferences(value, index, fieldPath, references);
			}
		}

		public static void InjectReferences(List<IComponentOld> components, ReferenceData[] references)
		{
			for (int i = 0; i < references.Length; i++)
			{
				var reference = references[i];

				try { components[reference.Index].SetValueToFieldAtPath(reference.PathSplit, reference.Reference); }
				catch { }
			}
		}

		static FieldInfo[] GetFields(Type type)
		{
			FieldInfo[] fields;

			if (!typeFields.TryGetValue(type, out fields))
			{
				var fieldList = new List<FieldInfo>(type.GetFields());

				for (int i = fieldList.Count - 1; i >= 0; i--)
				{
					var field = fieldList[i];

					if (field.IsInitOnly)
						fieldList.RemoveAt(i);
				}

				fields = fieldList.ToArray();
				typeFields[type] = fields;
			}

			return fields;
		}
	}
}