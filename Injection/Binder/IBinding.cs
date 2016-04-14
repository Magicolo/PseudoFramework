using System;

namespace Pseudo.Injection
{
	public interface IBinding
	{
		Type ContractType { get; }
		IInjectionFactory Factory { get; set; }
		Predicate<InjectionContext> Condition { get; set; }
	}
}