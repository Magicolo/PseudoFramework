using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection
{
	public interface IInjectableElement : ICustomAttributeProvider
	{
		object Inject(InjectionContext context);
		bool CanInject(InjectionContext context);
	}

	public interface IInjectableMember<TMember> : IInjectableElement where TMember : MemberInfo
	{
		TMember Member { get; }
	}

	public interface IInjectableConstructor : IInjectableMember<ConstructorInfo>
	{
		IInjectableParameter[] Parameters { get; }
	}

	public interface IInjectableField : IInjectableMember<FieldInfo> { }

	public interface IInjectableProperty : IInjectableMember<PropertyInfo> { }

	public interface IInjectableMethod : IInjectableMember<MethodInfo>
	{
		IInjectableParameter[] Parameters { get; }
	}

	public interface IInjectableParameter : IInjectableElement
	{
		ParameterInfo Parameter { get; }
	}
}
