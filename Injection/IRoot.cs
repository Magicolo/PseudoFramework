using System.Collections.Generic;

namespace Pseudo.Injection
{
	public interface IRoot
	{
		IContainer Container { get; }
		List<IBindingInstaller> Installers { get; }

		void InstallAll();
		void InjectAll();
	}
}