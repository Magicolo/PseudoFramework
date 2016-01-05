using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using System.Reflection.Emit;
using System.Reflection;
using UnityEditor;
using Pseudo.Internal.Editor;
using Pseudo.Internal.Unity;

public static class DummyUtility
{
	static Dictionary<Type, Type> dummyTypes = new Dictionary<Type, Type>();

	public static SerializedProperty SerializeDummy(IDummy dummy)
	{
		var serializedDummy = new SerializedObject((ScriptableObject)dummy);
		var valueProperty = serializedDummy.FindProperty("Value");

		CacheDrawers(serializedDummy.GetIterator());

		return valueProperty;
	}

	public static IDummy GetDummy(object value)
	{
		IDummy dummy;

		var scriptableDummy = ScriptableObject.CreateInstance(GetDummyType(value.GetType()));
		dummy = (IDummy)scriptableDummy;
		dummy.Value = value;

		return dummy;
	}

	public static SerializedProperty GetSerializedDummy(object value)
	{
		return SerializeDummy(GetDummy(value));
	}

	static Type GetDummyType(Type type)
	{
		Type dummyType;

		if (!dummyTypes.TryGetValue(type, out dummyType) || dummyType == null)
		{
			dummyType = CreateDummyType(type);
			dummyTypes[type] = dummyType;
		}

		return dummyType;
	}

	static Type CreateDummyType(Type type)
	{
		var typeSignature = "GeneratedDummy" + type.Name;
		var assemblyName = new AssemblyName(typeSignature);
		var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
		var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
		var typeBuilder = moduleBuilder.DefineType(typeSignature,
			TypeAttributes.Public |
			TypeAttributes.Class |
			TypeAttributes.AutoClass |
			TypeAttributes.AnsiClass |
			TypeAttributes.BeforeFieldInit |
			TypeAttributes.AutoLayout,
			typeof(Dummy<>).MakeGenericType(type));

		return typeBuilder.CreateType();
	}

	public static void CacheDrawers(SerializedProperty iterator)
	{
		while (iterator.NextVisible(true))
			CacheDrawer(iterator);
	}

	public static void CacheDrawer(SerializedProperty property)
	{
		if (property == null)
			return;

		var field = (FieldInfo)ScriptAttributeUtility.GetFieldInfoFromPropertyPath.Invoke(null, new object[] { property.serializedObject.targetObject.GetType(), property.propertyPath, null });

		if (field == null)
			return;

		var handlerCache = ScriptAttributeUtility.PropertyHandlerCache.GetValue(null, null);
		if (handlerCache.InvokeMethod("GetHandler", new object[] { property }) == null)
		{
			var handler = Activator.CreateInstance(PropertyHandler.Type);
			var attributes = field.GetCustomAttributes(typeof(PropertyAttribute), true);

			if (attributes.Length > 0)
				for (int i = 0; i < attributes.Length; i++)
					handler.InvokeMethod("HandleAttribute", new object[] { attributes[i], field, field.FieldType });
			else
				handler.InvokeMethod("HandleDrawnType", new object[] { field.FieldType, field.FieldType, field, null });

			handlerCache.InvokeMethod("SetHandler", new object[] { property, handler });
		}
	}


	public class Dummy<T> : ScriptableObject, IDummy
	{
		public T Value;

		object IDummy.Value
		{
			get { return Value; }
			set { Value = (T)value; }
		}
	}

	public interface IDummy
	{
		object Value { get; set; }
	}
}
