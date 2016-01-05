using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IStartable
	{
		IEntity Entity { get; }
		bool Active { get; set; }

		void Start();
	}
}
