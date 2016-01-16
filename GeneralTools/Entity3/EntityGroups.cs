using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	[Serializable]
	public partial class EntityGroups : PEnumFlag<EntityGroups>
	{
		protected EntityGroups(params byte[] values) : base(values) { }
	}
}
