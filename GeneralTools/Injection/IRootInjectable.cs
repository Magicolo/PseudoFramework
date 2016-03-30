﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IRootInjectable
	{
		void OnPreRootInject(IRoot binder);
		void OnPostRootInject(IRoot binder);
	}
}