using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal
{
	public static class CopyUtility
	{
		static readonly Type[] copierTypes = TypeUtility.AllTypes
			.Where(t => t.Is<ICopier>() && t.IsConcrete() && t.HasEmptyConstructor())
			.ToArray();

		public static ICopier<T> CreateCopier<T>() where T : class
		{
			var copierType = Array.Find(copierTypes, t => t.Is<ICopier<T>>());

			if (copierType == null)
				return new DefaultCopier<T>();

			return (ICopier<T>)Activator.CreateInstance(copierType);
		}
	}
}