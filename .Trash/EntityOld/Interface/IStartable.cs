using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.EntityOld
{
	public interface IStartable
	{
		IEntityOld Entity { get; }
		bool Active { get; set; }

		void Start();
	}
}
