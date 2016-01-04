using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IFixedUpdateable
	{
		bool Active { get; set; }

		void FixedUpdate();
	}
}
