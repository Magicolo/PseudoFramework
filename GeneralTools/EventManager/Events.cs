using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	[Serializable]
	public partial class Events : PEnumFlag<Events>
	{
		protected Events(params byte[] values) : base(values) { }

		public override bool Equals(Events other)
		{
			return HasAny(other);
		}
	}
}