using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class LifeTimeSystem : SystemBase, IUpdateable
	{
		public override IEntityGroup GetEntities()
		{
			return EntityManager.Entities.Filter(new[]
			{
				typeof(LifeTimeComponent),
				typeof(TimeComponent)
			});
		}

		public void Update()
		{
			for (int i = 0; i < Entities.Count; i++)
			{
				var entity = Entities[i];
				var lifeTime = entity.GetComponent<LifeTimeComponent>();
				var time = entity.GetComponent<TimeComponent>();

				lifeTime.LifeCounter += time.DeltaTime;

				if (lifeTime.LifeCounter >= lifeTime.LifeTime)
					EventManager.Trigger(lifeTime.DieEvent, entity);
			}
		}
	}
}