using System;
using System.Reflection;

namespace Pseudo.PoolingNOOOO
{
	public interface IInitializableElement
	{
		void Initialize(object instance, object value);
	}

	public interface IInitializableMember<TMember> : IInitializableElement where TMember : MemberInfo
	{
		TMember Member { get; }
	}

	public interface IInitializableField : IInitializableMember<FieldInfo> { }

	public interface IInitializableProperty : IInitializableMember<PropertyInfo> { }
}