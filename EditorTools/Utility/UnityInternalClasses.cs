﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Unity
{
	public static class ScriptAttributeUtility
	{
		public static readonly Type Type = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
		public static readonly PropertyInfo PropertyHandlerCache = Type.GetProperty("propertyHandlerCache", ReflectionExtensions.AllFlags);
		public static readonly MethodInfo GetFieldInfoFromPropertyPath = Type.GetMethod("GetFieldInfoFromPropertyPath", BindingFlags.NonPublic | BindingFlags.Static);
		public static readonly MethodInfo GetPropertyDrawerMethod = Type.GetMethod("GetDrawerTypeForType", ReflectionExtensions.AllFlags);
	}

	public static class PropertyHandler
	{
		public static readonly Type Type = Type.GetType("UnityEditor.PropertyHandler, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
	}

	public static class PropertyHandlerCache
	{
		public static readonly Type Type = Type.GetType("UnityEditor.PropertyHandlerCache, UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
		public static readonly MethodInfo GetPropertyHash = Type.GetMethod("GetPropertyHash", BindingFlags.NonPublic | BindingFlags.Static);
	}
}