using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	[Serializable]
	public partial class Events : PEnum<Events, int>
	{
		protected Events(int value) : base(value) { }
	}
}