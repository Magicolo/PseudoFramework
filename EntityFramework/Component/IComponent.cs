using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework
{
	public interface IComponent
	{
		bool Active { get; set; }
		IEntity Entity { get; set; }

		void OnActivated();
		void OnDeactivated();
		void OnAdded();
		void OnRemoved();
		void OnEntityActivated();
		void OnEntityDeactivated();
	}
}
