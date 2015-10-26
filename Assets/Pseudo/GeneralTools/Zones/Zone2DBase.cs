using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public abstract class Zone2DBase : MonoBehaviourExtended
{
	public abstract Vector2 GetRandomLocalPoint();
	public abstract Vector2 GetRandomWorldPoint();
}
