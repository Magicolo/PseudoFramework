using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;
using System.Reflection;

namespace Pseudo.Pooling2.Internal
{
	public class InitializableProperty : InitializableMemberBase<PropertyInfo>, IInitializableProperty
	{
		readonly IFieldOrPropertyWrapper wrapper;

		public InitializableProperty(PropertyInfo property) : base(property)
		{
			wrapper = property.CreateWrapper();
		}

		public override void Initialize(object instance, object value)
		{
			wrapper.Set(ref instance, value);
		}
	}
}
