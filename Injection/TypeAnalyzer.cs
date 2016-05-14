using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection;

namespace Pseudo.Injection.Internal
{
	public class TypeAnalyzer : ITypeAnalyzer
	{
		readonly Dictionary<Type, ITypeInfo> typeToInjectionInfo = new Dictionary<Type, ITypeInfo>();

		public ITypeInfo GetAnalysis(Type type)
		{
			ITypeInfo info;

			if (!typeToInjectionInfo.TryGetValue(type, out info))
			{
				info = CreateTypeInfo(type);
				typeToInjectionInfo[type] = info;
			}

			return info;
		}

		ITypeInfo CreateTypeInfo(Type type)
		{
			var baseTypes = TypeUtility.GetBaseTypes(type, false, false).ToArray();

			return new TypeInfo
			{
				Type = type,
				BaseTypes = baseTypes,
				Installers = CreateAttributeInstallers(type),
				Constructors = CreateInjectableConstructors(type),
				Fields = CreateInjectableFields(type, baseTypes),
				Properties = CreateInjectableProperties(type, baseTypes),
				Methods = CreateInjectableMethods(type, baseTypes)
			};
		}

		IBindingInstaller[] CreateAttributeInstallers(Type type)
		{
			return type.GetAttributes<BindAttributeBase>(true)
				.Select(a => CreateAttributeInstaller(a, type))
				.ToArray();
		}

		IInjectableConstructor[] CreateInjectableConstructors(Type type)
		{
			var injectableConstructors = new List<IInjectableConstructor>();
			var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// At least one constructor has an [Inject].
			if (constructors.Any(c => c.IsDefined(typeof(InjectAttribute), true)))
				injectableConstructors.AddRange(constructors
					.Where(c => c.IsDefined(typeof(InjectAttribute), true))
					.Select(c => CreateInjectableConstructor(c)));
			else
			{
				injectableConstructors.AddRange(constructors
					.Select(c => CreateInjectableConstructor(c)));

				if (type.IsValueType)
					injectableConstructors.Add(CreateInjectableConstructor(type));
			}

			return injectableConstructors
				.OrderByDescending(c => c.Parameters.Length)
				.ToArray();
		}

		IInjectableField[] CreateInjectableFields(Type type, Type[] baseTypes)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Concat(baseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(f => f.IsPrivate))
				.Where(f => !f.IsSpecialName && f.IsDefined(typeof(InjectAttribute), true))
				.Select(f => CreateInjectableField(f))
				.ToArray();
		}

		IInjectableProperty[] CreateInjectableProperties(Type type, Type[] baseTypes)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Concat(baseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(p => p.IsPrivate()))
				.Where(p => !p.IsSpecialName && p.IsDefined(typeof(InjectAttribute), true))
				.Select(p => CreateInjectableProperty(p))
				.ToArray();
		}

		IInjectableMethod[] CreateInjectableMethods(Type type, Type[] baseTypes)
		{
			return type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Concat(baseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(m => m.IsPrivate))
				.Where(m => !m.IsSpecialName && !m.IsConstructor && m.IsDefined(typeof(InjectAttribute), true))
				.Select(m => CreateInjectableMethod(m))
				.ToArray();
		}

		IBindingInstaller CreateAttributeInstaller(BindAttributeBase attribute, Type type)
		{
			return new BindAttributeInstaller(attribute, type);
		}

		IInjectableConstructor CreateInjectableConstructor(ConstructorInfo constructor)
		{
			return new InjectableConstructor(constructor, CreateInjectableParameters(constructor.GetParameters()));
		}

		IInjectableConstructor CreateInjectableConstructor(Type type)
		{
			return new InjectableEmptyConstructor(type);
		}

		IInjectableField CreateInjectableField(FieldInfo field)
		{
			if (field.IsInitOnly)
				throw new ArgumentException(string.Format("Can not inject in the readonly field {0}.{1}", field.DeclaringType.Name, field.Name));

			return new InjectableField(field);
		}

		IInjectableProperty CreateInjectableProperty(PropertyInfo property)
		{
			if (!property.CanWrite)
				throw new ArgumentException(string.Format("Can not inject in the readonly property {0}.{1}", property.DeclaringType.Name, property.Name));

			return new InjectableProperty(property);
		}

		IInjectableMethod CreateInjectableMethod(MethodInfo method)
		{
			return new InjectableMethod(method, CreateInjectableParameters(method.GetParameters()));
		}

		IInjectableParameter[] CreateInjectableParameters(IEnumerable<ParameterInfo> parameters)
		{
			return parameters.Select(p => (IInjectableParameter)new InjectableParameter(p)).ToArray();
		}
	}
}
