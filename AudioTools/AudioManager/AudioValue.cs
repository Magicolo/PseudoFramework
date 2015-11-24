using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	[Copy]
	public class AudioValue<T> : IPoolable, ICopyable<AudioValue<T>>
	{
		public static readonly Pool<AudioValue<T>> Pool = new Pool<AudioValue<T>>(new AudioValue<T>());

		T value;

		public T Value { get { return value; } set { this.value = value; } }

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(AudioValue<T> reference)
		{
			value = reference.value;
		}
	}
}