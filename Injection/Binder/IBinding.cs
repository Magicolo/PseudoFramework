using System;

namespace Pseudo.Injection
{
	public interface IBinding
	{
		Type ContractType { get; }
		Type[] BaseTypes { get; }
		IInjectionFactory Factory { get; set; }
		IInjectionScope Scope { get; set; }
		Predicate<InjectionContext> Condition { get; set; }
	}
}