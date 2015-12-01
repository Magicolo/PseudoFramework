﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public interface IComponentGroupMatcher
	{
		bool Matches(PEntity entity, BitArray componentBits);
	}
}