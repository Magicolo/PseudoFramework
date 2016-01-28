using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface ISystem
	{
		bool Active { get; set; }

		void OnInitialize();
		void OnDestroy();
	}
}
