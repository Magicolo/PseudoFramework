﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling
{
	public interface IPoolFactory : IFactory<Type, IPool> { }
}
