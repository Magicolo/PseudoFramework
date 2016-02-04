using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IComponent
	{
		bool Active { get; set; }
		IEntity Entity { get; set; }
	}
}
