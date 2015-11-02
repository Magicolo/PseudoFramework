using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(TimeComponent))]
	public class CharacterEntity : EntityBase, ICopyable<CharacterEntity>
	{
		public TimeComponent Time;
		readonly CachedValue<TimeComponent> cachedTimeComponent;
		public TimeComponent CachedTimeComponent { get { return cachedTimeComponent; } }

		public CharacterEntity()
		{
			cachedTimeComponent = new CachedValue<TimeComponent>(GetComponent<TimeComponent>);
		}

		public void Copy(CharacterEntity reference)
		{
			base.Copy(reference);

			Time = reference.Time;
		}
	}
}