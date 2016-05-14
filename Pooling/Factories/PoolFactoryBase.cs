using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling
{
	public abstract class PoolFactoryBase : FactoryBase<Type, IPool>, IPoolFactory { }
}
