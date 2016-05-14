using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class MemberInjector : IMemberInjector
	{
		public void Inject(InjectionContext context, ITypeInfo info)
		{
			// Inject Fields
			for (int i = 0; i < info.Fields.Length; i++)
				info.Fields[i].Inject(context);

			// Inject Properties
			for (int i = 0; i < info.Properties.Length; i++)
				info.Properties[i].Inject(context);

			// Inject Methods
			for (int i = 0; i < info.Methods.Length; i++)
				info.Methods[i].Inject(context);
		}
	}
}
