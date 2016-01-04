<<<<<<< HEAD
﻿using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Pool
{
	public interface IPoolInitializable
	{
		void OnPrePoolInitialize();
		void OnPostPoolInitialize(List<IPoolSetter> setters);
	}
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Pool
{
	public interface IPoolInitializable
	{
		void OnPrePoolInitialize();
		void OnPostPoolInitialize();
	}
}
>>>>>>> Entity2
