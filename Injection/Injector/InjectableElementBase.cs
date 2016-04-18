using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Injection.Internal
{
	public abstract class InjectableElementBase : IInjectableElement
	{
		protected readonly ICustomAttributeProvider provider;
		protected readonly InjectAttribute attribute;

		protected InjectableElementBase(ICustomAttributeProvider provider)
		{
			this.provider = provider;

			attribute = provider.GetAttribute<InjectAttribute>(true) ?? new InjectAttribute();
		}

		public bool CanInject(InjectionContext context)
		{
			SetupContext(ref context);

			return CanInject(ref context);
		}

		public object Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!attribute.Optional || CanInject(ref context))
				return Inject(ref context);

			return null;
		}

		public object[] GetCustomAttributes(bool inherit)
		{
			return provider.GetCustomAttributes(inherit);
		}

		public object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return provider.GetCustomAttributes(attributeType, inherit);
		}

		public bool IsDefined(Type attributeType, bool inherit)
		{
			return provider.IsDefined(attributeType, inherit);
		}

		protected virtual void SetupContext(ref InjectionContext context)
		{
			context.Element = this;
			context.Identifier = attribute.Identifier;
			context.Optional = attribute.Optional;
		}
		protected abstract bool CanInject(ref InjectionContext context);
		protected abstract object Inject(ref InjectionContext context);
	}
}
