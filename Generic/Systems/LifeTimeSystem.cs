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
		IEntityGroup entities;

		public override void OnInitialize()
		{
			base.OnInitialize();

			entities = EntityManager.Entities.Filter(new[]
			{
			typeof(LifeTimeComponent),
			typeof(TimeComponent)
		});
		}

		public void Update()
		{
			for (int i = 0; i < entities.Count; i++)
			{
				var entity = entities[i];
				var lifeTime = entity.GetComponent<LifeTimeComponent>();
				var time = entity.GetComponent<TimeComponent>();

				lifeTime.LifeCounter += time.DeltaTime;

				if (lifeTime.LifeCounter >= lifeTime.LifeTime)
					EventManager.Trigger(lifeTime.DieEvent, entity);
			}
		}
	}
}