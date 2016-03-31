using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public abstract class ReturnNodeBase : NodeBase
	{
		public Type ReturnType
		{
			get { return returnType; }
		}

		[SerializeField]
		PType returnType;

		protected virtual void Initialize(string name, Type returnType, Schema schema)
		{
			Initialize(name, schema);
			this.returnType = returnType;
		}

		public override bool IsValid()
		{
			return ReturnType != null && base.IsValid();
		}
	}
}
