using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo.Audio
{
	public class AudioValue<T>
	{
		T value;

		public T Value { get { return value; } set { this.value = value; } }
	}
}