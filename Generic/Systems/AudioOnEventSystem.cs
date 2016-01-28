using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class AudioOnEventSystem : SystemBase
	{
		public override IEntityGroup GetEntities()
		{
			return EntityManager.Entities.Filter(typeof(AudioOnEventComponent));
		}

		public override void OnActivate()
		{
			base.OnActivate();

			EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			EventManager.UnsubscribeAll((Action<Events, IEntity>)OnEvent);
		}

		void OnEvent(Events identifier, IEntity entity)
		{
			if (!Entities.Contains(entity))
				return;

			var audio = entity.GetComponent<AudioOnEventComponent>();

			for (int i = 0; i < audio.Events.Length; i++)
			{
				var audioEvent = audio.Events[i];

				if (audioEvent.Event.HasAll(identifier))
					AudioManager.CreateItem(audioEvent.Audio, audio.CachedTransform.position).Play();
			}
		}
	}
}