using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public static class ReflectionExtensions
	{
		public const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags PublicFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags NonPublicFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags StaticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		public const BindingFlags InstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		public static MemberInfo GetMemberInfo(this object obj, string memberName)
		{
			FieldInfo field = obj.GetType().GetField(memberName, AllFlags);

			if (field != null)
				return field;

			PropertyInfo property = obj.GetType().GetProperty(memberName, AllFlags);

			if (property != null)
				return property;

			return null;
		}

		public static T GetMemberValue<T>(this MemberInfo memberInfo, object obj)
		{
			return (T)memberInfo.GetMemberValue(obj);
		}

		public static object GetMemberValue(this MemberInfo memberInfo, object obj)
		{
			FieldInfo field = memberInfo as FieldInfo;

			if (field != null)
				return field.GetValue(obj);

			PropertyInfo property = memberInfo as PropertyInfo;

			if (property != null)
				return property.GetValue(obj, null);

			return null;
		}

		public static void SetMemberValue(this MemberInfo memberInfo, object obj, object value)
		{
			FieldInfo field = memberInfo as FieldInfo;

			if (field != null)
			{
				field.SetValue(obj, value);
				return;
			}

			PropertyInfo property = memberInfo as PropertyInfo;

			if (property != null)
			{
				property.SetValue(obj, value, null);
				return;
			}
		}

		public static Type GetMemberType(this MemberInfo memberInfo)
		{
			FieldInfo field = memberInfo as FieldInfo;

			if (field != null)
				return field.FieldType;

			PropertyInfo property = memberInfo as PropertyInfo;

			if (property != null)
				return property.PropertyType;

			return null;
		}

		public static MemberInfo GetMemberInfoAtPath(this object obj, string memberPath)
		{
			string[] pathSplit = memberPath.Split('.');

			if (pathSplit.Length <= 1)
				return obj.GetMemberInfo(pathSplit.Pop(out pathSplit));

			object value = obj.GetValueFromMember(pathSplit.Pop(out pathSplit));

			while (pathSplit.Length > 1)
				value = value.GetValueFromMember(pathSplit.Pop(out pathSplit));

			return value.GetMemberInfo(pathSplit.Pop(out pathSplit));
		}

		public static T GetValueFromMember<T>(this object obj, string memberName)
		{
			return (T)obj.GetValueFromMember(memberName);
		}

		public static object GetValueFromMember(this object obj, string memberName)
		{
			if (obj is IList)
				return ((IList)obj)[int.Parse(memberName)];

			MemberInfo member = obj.GetMemberInfo(memberName);

			return member.GetMemberValue(obj);
		}

		public static T GetValueFromMemberAtPath<T>(this object obj, string memberPath)
		{
			return (T)obj.GetValueFromMemberAtPath(memberPath);
		}

		public static object GetValueFromMemberAtPath(this object obj, string memberPath)
		{
			MemberInfo member = obj.GetMemberInfoAtPath(memberPath);
			string[] pathSplit = memberPath.Split('.');

			if (pathSplit.Length <= 1)
			{
				return obj.GetValueFromMember(pathSplit.Pop(out pathSplit));
			}

			int index;
			if (int.TryParse(pathSplit.Last(), out index))
			{
				Array.Resize(ref pathSplit, pathSplit.Length - 1);
				return ((IList)obj.GetValueFromMemberAtPath(pathSplit.Concat(".")))[index];
			}

			Array.Resize(ref pathSplit, pathSplit.Length - 1);

			object container = obj.GetValueFromMemberAtPath(pathSplit.Concat("."));

			return member.GetMemberValue(container);
		}

		public static void SetValueToMember(this object obj, string memberName, object value)
		{
			MemberInfo member = obj.GetMemberInfo(memberName);

			member.SetMemberValue(obj, value);
		}

		public static void SetValueToMemberAtPath(this object obj, string memberPath, object value)
		{
			MemberInfo member = obj.GetMemberInfoAtPath(memberPath);
			string[] pathSplit = memberPath.Split('.');

			if (pathSplit.Length <= 1)
			{
				obj.SetValueToMember(memberPath, value);
				return;
			}

			Array.Resize(ref pathSplit, pathSplit.Length - 1);
			object container = obj.GetValueFromMemberAtPath(pathSplit.Concat("."));

			member.SetMemberValue(container, value);
		}

		public static object InvokeMethod(this object obj, string methodName, params object[] arguments)
		{
			MethodInfo[] methods = obj.GetType().GetMethods(AllFlags);

			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo method = methods[i];

				if (method.Name == methodName && method.GetParameters().Length == arguments.Length)
					return method.Invoke(obj, arguments);
			}

			return null;
		}

		public static string[] GetFieldsPropertiesNames(this object obj, BindingFlags flags, params Type[] filter)
		{
			return obj.GetType().GetFieldsAndPropertiesNames(flags, filter);
		}

		public static string[] GetFieldsPropertiesNames(this object obj, params Type[] filter)
		{
			return obj.GetType().GetFieldsAndPropertiesNames(AllFlags, filter);
		}

		public static string GetTypeName(this object obj)
		{
			return obj.GetType().GetName();
		}

		public static string[] GetFieldsAndPropertiesNames(this Type type, BindingFlags flags, params Type[] filter)
		{
			List<string> names = new List<string>();
			FieldInfo[] fields = type.GetFields(flags);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo field = fields[i];

				if (filter == null || filter.Length == 0 || filter.Any(t => t.IsAssignableFrom(field.FieldType)))
					names.Add(field.Name);
			}

			PropertyInfo[] properties = type.GetProperties(flags);

			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo property = properties[i];

				if (filter == null || filter.Length == 0 || filter.Any(t => t.IsAssignableFrom(property.PropertyType)))
					names.Add(property.Name);
			}

			return names.ToArray();
		}

		public static string[] GetFieldsAndPropertiesNames(this Type type, params Type[] filter)
		{
			return GetFieldsAndPropertiesNames(type, AllFlags, filter);
		}
	}
}