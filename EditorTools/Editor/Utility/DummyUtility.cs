using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection.Emit;
using System.Reflection;
using UnityEditor;

public static class DummyUtility
{
	static Dictionary<Type, Type> dummyTypes = new Dictionary<Type, Type>();

	public static SerializedObject SerializeDummy(IDummy dummy)
	{
		return new SerializedObject((ScriptableObject)dummy);
	}

	public static IDummy GetDummy(Type type)
	{
		var dummy = ScriptableObject.CreateInstance(GetDummyType(type));
		dummy.hideFlags = HideFlags.DontSave;

		return (IDummy)dummy;
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

	class Dummy<T> : ScriptableObject, IDummy
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
