using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection
{
	public interface IInjectableElement
	{
		object Inject(InjectionContext context);
		bool CanInject(InjectionContext context);
	}

	public interface IInjectableMember : IInjectableElement
	{
		MemberInfo Member { get; }
		InjectAttribute Attribute { get; }
	}

	public interface IInjectableConstructor : IInjectableMember
	{
		ConstructorInfo Constructor { get; }
		IInjectableParameter[] Parameters { get; }
	}

	public interface IInjectableField : IInjectableMember
	{
		FieldInfo Field { get; }
	}

	public interface IInjectableProperty : IInjectableMember
	{
		PropertyInfo Property { get; }
	}

	public interface IInjectableMethod : IInjectableMember
	{
		MethodInfo Method { get; }
		IInjectableParameter[] Parameters { get; }
	}

	public interface IInjectableParameter : IInjectableElement
	{
		ParameterInfo Parameter { get; }
		InjectAttribute Attribute { get; }
	}
}
