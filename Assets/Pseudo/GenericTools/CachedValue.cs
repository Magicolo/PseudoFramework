using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class CachedValue<T> : ICopyable<CachedValue<T>>
	{
		Func<T> getValue;
		bool valueCached;
		T value;

		public T Value
		{
			get
			{
				if (!valueCached)
				{
					valueCached = Application.isPlaying;
					value = getValue();
				}

				return value;
			}
		}

		public CachedValue(Func<T> getValue)
		{
			this.getValue = getValue;
		}

		public void Reset()
		{
			valueCached = false;
		}

		public void Copy(CachedValue<T> reference)
		{
			getValue = reference.getValue;
			valueCached = reference.valueCached;
			value = reference.value;
		}

		public static implicit operator T(CachedValue<T> cachedValue)
		{
			return cachedValue.Value;
		}
	}
}