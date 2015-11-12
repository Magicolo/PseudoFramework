using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Copy]
	public class CachedValue<T> : ICopyable<CachedValue<T>>
	{
		Func<T> getValue;
		T value;
		bool isValueCached;

		public T Value
		{
			get
			{
				if (!isValueCached)
				{
					isValueCached = Application.isPlaying;
					value = getValue();
				}

				return value;
			}
			set
			{
				this.value = value;
				isValueCached = true;
			}
		}

		public CachedValue(Func<T> getValue)
		{
			this.getValue = getValue;
		}

		public void Reset()
		{
			isValueCached = false;
			value = default(T);
		}

		public void Copy(CachedValue<T> reference)
		{
			getValue = reference.getValue;
			value = reference.value;
			isValueCached = reference.isValueCached;
		}

		public static implicit operator T(CachedValue<T> cachedValue)
		{
			return cachedValue.Value;
		}
	}
}