using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IComponent
	{
		IEntity Entity { get; set; }
		bool Active { get; set; }
	}
}