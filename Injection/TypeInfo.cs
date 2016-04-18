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
	public class TypeInfo : ITypeInfo
	{
		static readonly Func<ConstructorInfo, bool> constructorFilter = c => c.IsDefined(typeof(InjectAttribute), true);
		static readonly Func<FieldInfo, bool> fieldFilter = f => !f.IsSpecialName && f.IsDefined(typeof(InjectAttribute), true);
		static readonly Func<PropertyInfo, bool> propertyFilter = p => !p.IsSpecialName && p.CanWrite && p.IsDefined(typeof(InjectAttribute), true);
		static readonly Func<MethodInfo, bool> methodFilter = m => !m.IsSpecialName && !m.IsConstructor && m.IsDefined(typeof(InjectAttribute), true);

		public Type Type { get; private set; }
		public Type[] BaseTypes { get; private set; }
		public IBindingInstaller[] Installers { get; private set; }
		public IInjectableConstructor[] Constructors { get; private set; }
		public IInjectableField[] Fields { get; private set; }
		public IInjectableProperty[] Properties { get; private set; }
		public IInjectableMethod[] Methods { get; private set; }

		public TypeInfo(Type type)
		{
			Type = type;
			BaseTypes = TypeUtility.GetBaseTypes(type, false, false).ToArray();
			Installers = CreateAttributeInstallers();
			Constructors = CreateInjectableConstructors();
			Fields = CreateInjectableFields();
			Properties = CreateInjectableProperties();
			Methods = CreateInjectableMethods();
		}

		IBindingInstaller[] CreateAttributeInstallers()
		{
			return Type.GetAttributes<BindAttributeBase>(true)
				.Select(a => CreateAttributeInstaller(a))
				.ToArray();
		}

		IInjectableConstructor[] CreateInjectableConstructors()
		{
			var injectableConstructors = new List<IInjectableConstructor>();
			var constructors = Type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// At least one constructor has an [Inject].
			if (constructors.Any(constructorFilter))
				injectableConstructors.AddRange(constructors
					.Where(constructorFilter)
					.Select(c => CreateInjectableConstructor(c)));
			else
			{
				injectableConstructors.AddRange(constructors
					.Select(c => CreateInjectableConstructor(c)));

				if (Type.IsValueType)
					injectableConstructors.Add(CreateInjectableConstructor(Type));
			}

			return injectableConstructors
				.OrderByDescending(c => c.Parameters.Length)
				.ToArray();
		}

		IInjectableField[] CreateInjectableFields()
		{
			return Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(fieldFilter)
				.Concat(BaseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(f => fieldFilter(f) && f.IsPrivate))
				.Select(f => CreateInjectableField(f))
				.ToArray();
		}

		IInjectableProperty[] CreateInjectableProperties()
		{
			return Type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(propertyFilter)
				.Concat(BaseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(p => propertyFilter(p) && p.IsPrivate()))
				.Select(p => CreateInjectableProperty(p))
				.ToArray();
		}

		IInjectableMethod[] CreateInjectableMethods()
		{
			return Type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(methodFilter)
				.Concat(BaseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(m => methodFilter(m) && m.IsPrivate))
				.Select(m => CreateInjectableMethod(m))
				.ToArray();
		}

		IBindingInstaller CreateAttributeInstaller(BindAttributeBase attribute)
		{
			return new BindAttributeInstaller(attribute, Type);
		}

		IInjectableConstructor CreateInjectableConstructor(ConstructorInfo constructor)
		{
			return new InjectableConstructor(constructor, CreateInjectableParameters(constructor.GetParameters()));
		}

		IInjectableConstructor CreateInjectableConstructor(Type valueType)
		{
			return new InjectableEmptyStructConstructor(valueType);
		}

		IInjectableField CreateInjectableField(FieldInfo field)
		{
			return new InjectableField(field);
		}

		IInjectableProperty CreateInjectableProperty(PropertyInfo property)
		{
			if (property.IsAutoProperty())
				return new InjectableAutoProperty(property);
			else
				return new InjectableProperty(property);
		}

		IInjectableMethod CreateInjectableMethod(MethodInfo method)
		{
			if (method.ReturnType == typeof(void) && method.GetParameters().Length == 0)
				return new InjectableEmptyMethod(method);
			else
				return new InjectableMethod(method, CreateInjectableParameters(method.GetParameters()));
		}

		IInjectableParameter[] CreateInjectableParameters(IEnumerable<ParameterInfo> parameters)
		{
			return parameters.Select(p => (IInjectableParameter)new InjectableParameter(p)).ToArray();
		}
	}
}
