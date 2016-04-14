
using System.Collections.Generic;

namespace Pseudo.Injection
{
	public interface IRoot
	{
		IContainer Container { get; }
		IEnumerable<IBindingInstaller> Installers { get; }

		void AddInstaller(IBindingInstaller installer);
		void RemoveInstaller(IBindingInstaller installer);
		void InstallAll();
		void InjectAll();
	}
}