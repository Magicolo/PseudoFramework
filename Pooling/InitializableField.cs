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
	public class InitializableField : InitializableMemberBase<FieldInfo>, IInitializableField
	{
		readonly IFieldOrPropertyWrapper wrapper;

		public InitializableField(FieldInfo field) : base(field)
		{
			wrapper = field.CreateWrapper();
		}

		public override void Initialize(object instance, object value)
		{
			wrapper.Set(ref instance, value);
		}
	}
}
