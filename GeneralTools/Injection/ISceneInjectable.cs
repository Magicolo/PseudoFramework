﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface ISceneInjectable
	{
		void OnPreSceneInject(IBinder binder);
		void OnPostSceneInject(IBinder binder);
	}
}
