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
		readonly List<IAudioItem> activeItems = new List<IAudioItem>();

		public override IEntityGroup GetEntities()
		{
			return EntityManager.Entities.Filter(typeof(AudioOnEventComponent));
		}

		public override void OnActivate()
		{
			base.OnActivate();

			EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
			EventManager.SubscribeAll((Action<BehaviourEvents, IEntity>)OnBehaviourEvent);
			EventManager.SubscribeAll((Action<UIEvents, IEntity>)OnUIEvent);
			EventManager.SubscribeAll((Action<PhysicsEvents, IEntity>)OnPhysicsEvent);
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			EventManager.UnsubscribeAll((Action<Events, IEntity>)OnEvent);
			EventManager.UnsubscribeAll((Action<BehaviourEvents, IEntity>)OnBehaviourEvent);
			EventManager.UnsubscribeAll((Action<UIEvents, IEntity>)OnUIEvent);
			EventManager.UnsubscribeAll((Action<PhysicsEvents, IEntity>)OnPhysicsEvent);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();

			for (int i = activeItems.Count - 1; i >= 0; i--)
				activeItems[i].StopImmediate();
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
					PlayAudio(audio, audioEvent);
			}
		}

		void OnBehaviourEvent(BehaviourEvents identifier, IEntity entity)
		{
			if (!Entities.Contains(entity))
				return;

			var audio = entity.GetComponent<AudioOnEventComponent>();

			for (int i = 0; i < audio.Events.Length; i++)
			{
				var audioEvent = audio.Events[i];

				if (audioEvent.BehaviourEvent.HasAll(identifier))
					PlayAudio(audio, audioEvent);
			}
		}

		void OnUIEvent(UIEvents identifier, IEntity entity)
		{
			if (!Entities.Contains(entity))
				return;

			var audio = entity.GetComponent<AudioOnEventComponent>();

			for (int i = 0; i < audio.Events.Length; i++)
			{
				var audioEvent = audio.Events[i];

				if (audioEvent.UIEvent.HasAll(identifier))
					PlayAudio(audio, audioEvent);
			}
		}

		void OnPhysicsEvent(PhysicsEvents identifier, IEntity entity)
		{
			if (!Entities.Contains(entity))
				return;

			var audio = entity.GetComponent<AudioOnEventComponent>();

			for (int i = 0; i < audio.Events.Length; i++)
			{
				var audioEvent = audio.Events[i];

				if (audioEvent.PhysicsEvent.HasAll(identifier))
					PlayAudio(audio, audioEvent);
			}
		}

		void PlayAudio(AudioOnEventComponent audio, AudioOnEventComponent.EventData data)
		{
			IAudioItem item;

			switch (data.Spatialization)
			{
				default:
					item = AudioManager.CreateItem(data.Audio);
					break;
				case AudioOnEventComponent.SpatializationModes.Static:
					item = AudioManager.CreateItem(data.Audio, audio.CachedTransform.position);
					break;
				case AudioOnEventComponent.SpatializationModes.Dynamic:
					item = AudioManager.CreateItem(data.Audio, audio.CachedTransform);
					break;
			}

			activeItems.Add(item);
			item.OnStop += i => activeItems.Remove(i);
			item.Play();
		}
	}
}