using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using System.Linq;
using System;
using System.Diagnostics;

namespace Pseudo
{
	public static class PDebug
	{
		public static bool LogToScreen
		{
			get { return logToScreen; }
			set
			{
				logToScreen = value;
				ScreenLogger.Initialize();
			}
		}
		public static bool LogToConsole { get { return logToConsole; } set { logToConsole = value; } }

		static bool logToScreen;
		static bool logToConsole = true;
		static int indent;
		static readonly Dictionary<Type, int> instanceDict = new Dictionary<Type, int>();
		static readonly Stopwatch timer = new Stopwatch();

		public static float RoundPrecision = 0.001F;

		public static void Log(params object[] toLog)
		{
			if (logToScreen)
				ScreenLogger.Log(LogToString(toLog));

			if (logToConsole)
				UnityEngine.Debug.Log(LogToString(toLog));
		}

		public static void LogWarning(params object[] toLog)
		{
			if (LogToScreen)
				ScreenLogger.LogWarning(LogToString(toLog));

			if (logToConsole)
				UnityEngine.Debug.LogWarning(LogToString(toLog));
		}

		public static void LogError(params object[] toLog)
		{
			if (LogToScreen)
				ScreenLogger.LogError(LogToString(toLog));

			if (logToConsole)
				UnityEngine.Debug.LogError(LogToString(toLog));
		}

		public static void LogMethod(params object[] toLog)
		{
			var trace = new StackTrace();
			var callerFrame = trace.GetFrame(1);
			var method = callerFrame.GetMethod();

			indent++;
			UnityEngine.Debug.Log(string.Format("{0}.{1}\n{2}", method.DeclaringType, method.Name, LogToString(toLog)));
			indent--;
		}

		public static void LogSingleInstance(UnityEngine.Object instanceToLog, params object[] toLog)
		{
			int instanceId;

			if (instanceDict.TryGetValue(instanceToLog.GetType(), out instanceId))
			{
				if (instanceId == instanceToLog.GetInstanceID())
					Log(toLog);
			}
			else
			{
				instanceDict[instanceToLog.GetType()] = instanceToLog.GetInstanceID();
				Log(toLog);
			}
		}

		public static void LogTest(string testName, Action test, int iterations)
		{
			test();

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			long total = 0L;
			long min = long.MaxValue;
			long max = long.MinValue;

			for (int i = 0; i < iterations; i++)
			{
				timer.Reset();
				timer.Start();
				test();
				timer.Stop();
				total += timer.ElapsedTicks;
				min = Math.Min(min, timer.ElapsedTicks);
				max = Math.Max(max, timer.ElapsedTicks);
			}

			Log(string.Format("{0}\nAverage = {1} | Min: {2} | Max: {3} | Total: {4}", testName, total / (double)iterations, FormatNumber(min), FormatNumber(max), FormatNumber(total)));
		}

		static string LogToString(object[] toLog)
		{
			string log = "";

			if (toLog != null)
			{
				for (int i = 0; i < toLog.Length; i++)
				{
					object item = toLog[i];
					bool isEnumerable = item is IEnumerable && !(item is string);

					if (i > 0 && isEnumerable && log.Last() != '\n')
						log += "\n" + GetIndentString();

					log += ToString(item);

					if (i < toLog.Length - 1)
						log += "," + (isEnumerable ? "\n" + GetIndentString() : " ");
				}
			}

			return log;
		}

		static string FormatNumber(long number)
		{
			var formattedTicks = number.ToString();

			for (int i = formattedTicks.Length - 3; i > 0; i -= 3)
				formattedTicks = formattedTicks.Insert(i, " ");

			return formattedTicks;
		}

		static string FormatMemory(long memory)
		{
			if (memory > 100000000000)
				return memory / 1000000000000d + " TB";
			else if (memory > 100000000)
				return memory / 1000000000d + " GB";
			else if (memory > 100000)
				return memory / 1000000d + " MB";
			else if (memory > 100)
				return memory / 1000d + " KB";
			else
				return memory + "B";
		}

		static string FormatType(Type type)
		{
			var formattedType = "";

			if (type.IsArray)
				formattedType += FormatType(type.GetElementType()) + "[]";
			else if (typeof(IList).IsAssignableFrom(type))
				formattedType += "List";
			else if (typeof(IDictionary).IsAssignableFrom(type))
				formattedType += "Dictionary";
			else
				formattedType += type.Name;

			if (type.IsGenericType)
			{
				var genericTypes = type.GetGenericArguments();

				formattedType += "<";

				for (int i = 0; i < genericTypes.Length; i++)
				{
					formattedType += FormatType(genericTypes[i]);

					if (i < genericTypes.Length - 1)
						formattedType += ", ";
				}

				formattedType += ">";
			}

			return formattedType;
		}

		static string GetIndentString()
		{
			var str = "";

			for (int i = 0; i < indent; i++)
				str += "   ";

			return str;
		}

		public static string ToString(object obj)
		{
			string str = "";

			if (obj is System.Array)
			{
				str += FormatType(obj.GetType()) + "{ ";

				foreach (object item in (Array)obj)
					str += ToString(item) + ", ";

				if (((System.Array)obj).Length > 0)
					str = str.Substring(0, str.Length - 2);

				str += " }";
			}
			else if (obj is IList)
			{
				str += FormatType(obj.GetType()) + "{ ";

				foreach (object item in (IList)obj)
					str += ToString(item) + ", ";

				if (((IList)obj).Count > 0)
					str = str.Substring(0, str.Length - 2);

				str += " }";
			}
			else if (obj is IDictionary)
			{
				str += FormatType(obj.GetType()) + "\n" + GetIndentString() + "{\n";

				indent++;

				foreach (object key in ((IDictionary)obj).Keys)
					str += GetIndentString() + ToString(key) + " : " + ToString(((IDictionary)obj)[key]) + ",\n";

				indent--;

				if (((IDictionary)obj).Count > 0)
					str = str.Substring(0, str.Length - 2);

				str += GetIndentString() + "\n" + GetIndentString() + "}";
			}
			else if (obj is IEnumerator)
				str += ToString(((IEnumerator)obj).Current);
			else if (obj is Vector2 || obj is Vector3 || obj is Vector4 || obj is Color || obj is Quaternion || obj is Rect)
				str += VectorToString(obj);
			else if (obj is LayerMask)
				str += ((LayerMask)obj).value.ToString();
			else if (obj != null)
				str += obj.ToString();
			else
				str += "null";

			return str;
		}

		public static string VectorToString(object obj)
		{
			var str = "";

			if (obj is Vector2)
			{
				var vector2 = ((Vector2)obj).Round(RoundPrecision);
				str += "Vector2(" + vector2.x + ", " + vector2.y;
			}
			else if (obj is Vector3)
			{
				var vector3 = ((Vector3)obj).Round(RoundPrecision);
				str += "Vector3(" + vector3.x + ", " + vector3.y + ", " + vector3.z;
			}
			else if (obj is Vector4)
			{
				var vector4 = ((Vector4)obj).Round(RoundPrecision);
				str += "Vector4(" + vector4.x + ", " + vector4.y + ", " + vector4.z + ", " + vector4.w;
			}
			else if (obj is Quaternion)
			{
				var quaternion = ((Quaternion)obj).Round(RoundPrecision);
				str += "Quaternion(" + quaternion.x + ", " + quaternion.y + ", " + quaternion.z + ", " + quaternion.w;
			}
			else if (obj is Color)
			{
				var color = ((Color)obj).Round(RoundPrecision);
				str += "Color(" + color.r + ", " + color.g + ", " + color.b + ", " + color.a;
			}
			else if (obj is Rect)
			{
				var rect = ((Rect)obj).Round(RoundPrecision);
				str += "Rect(" + rect.x + ", " + rect.y + ", " + rect.width + ", " + rect.height;
			}

			return str + ")";
		}
	}
}
