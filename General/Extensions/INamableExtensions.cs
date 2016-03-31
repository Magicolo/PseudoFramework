using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	public static class INamableExtensions
	{
		static readonly Func<INamable, string> namableToName = namable => namable.Name;

		public static void Sort(this INamable[] namables)
		{
			Array.Sort(namables, (source, target) => string.Compare(source.Name, target.Name, StringComparison.Ordinal));
		}

		public static string GetUniqueName(this INamable namable, IList<INamable> array, string newName, string oldName)
		{
			int suffix = 0;
			bool uniqueName = false;
			string currentName = "";

			while (!uniqueName)
			{
				uniqueName = true;
				currentName = newName;
				if (suffix > 0) currentName += suffix.ToString();

				for (int i = 0; i < array.Count; i++)
				{
					INamable element = array[i];

					if (element != namable && element.Name == currentName && element.Name != oldName)
					{
						uniqueName = false;
						break;
					}
				}

				suffix += 1;
			}
			return currentName;
		}

		public static string GetUniqueName(this INamable namable, IList<INamable> array, string newName, string oldName, string emptyName)
		{
			string name = namable.GetUniqueName(array, newName, oldName);

			if (string.IsNullOrEmpty(newName))
				name = namable.GetUniqueName(array, emptyName, oldName);

			return name;
		}

		public static string GetUniqueName(this INamable namable, IList<INamable> array, string newName)
		{
			return namable.GetUniqueName(array, newName, namable.Name);
		}

		public static void SetUniqueName(this INamable namable, IList<INamable> array, string newName, string oldName, string emptyName)
		{
			namable.Name = namable.GetUniqueName(array, newName, oldName, emptyName);
		}

		public static void SetUniqueName(this INamable namable, IList<INamable> array, string newName, string oldName)
		{
			namable.Name = namable.GetUniqueName(array, newName, oldName);
		}

		public static void SetUniqueName(this INamable namable, IList<INamable> array, string newName)
		{
			namable.Name = namable.GetUniqueName(array, newName, namable.Name);
		}

		public static string[] GetNames(this IList<INamable> namables)
		{
			return namables.Convert(namableToName);
		}
	}
}