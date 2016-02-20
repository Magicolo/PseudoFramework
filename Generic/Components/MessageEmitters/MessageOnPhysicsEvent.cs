using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.EventSystems;

namespace Pseudo
{
	[Flags]
	public enum PhysicsEvents
	{
		OnControllerColliderHit = 1 << 0,
		OnCollisionEnter = 1 << 1,
		OnCollisionStay = 1 << 2,
		OnCollisionExit = 1 << 3,
		OnCollisionEnter2D = 1 << 4,
		OnCollisionStay2D = 1 << 5,
		OnCollisionExit2D = 1 << 6,
		OnTriggerEnter = 1 << 7,
		OnTriggerStay = 1 << 8,
		OnTriggerExit = 1 << 9,
		OnTriggerEnter2D = 1 << 10,
		OnTriggerStay2D = 1 << 11,
		OnTriggerExit2D = 1 << 12,
	}

	public class MessageOnPhysicsEvent : ComponentBehaviour
	{
		[Serializable]
		public struct PhysicsMessage
		{
			[EnumFlags]
			public PhysicsEvents Events;
			public EntityMessage Message;
		}

		public PhysicsMessage[] Messages = new PhysicsMessage[0];

		void SendMessage(PhysicsEvents behaviourEvent)
		{
			SendMessage(behaviourEvent, (object)null);
		}

		void SendMessage<T>(PhysicsEvents behaviourEvent, T data)
		{
			for (int i = 0; i < Messages.Length; i++)
			{
				var message = Messages[i];

				if (Active && Entity != null && (message.Events & behaviourEvent) != 0)
					Entity.SendMessage(message.Message, data);
			}
		}

		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			SendMessage(PhysicsEvents.OnControllerColliderHit, hit);
		}

		void OnCollisionEnter(Collision collision)
		{
			SendMessage(PhysicsEvents.OnCollisionEnter, collision);
		}

		void OnCollisionStay(Collision collision)
		{
			SendMessage(PhysicsEvents.OnCollisionStay, collision);
		}

		void OnCollisionExit(Collision collision)
		{
			SendMessage(PhysicsEvents.OnCollisionExit, collision);
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			SendMessage(PhysicsEvents.OnCollisionEnter2D, collision);
		}

		void OnCollisionStay2D(Collision2D collision)
		{
			SendMessage(PhysicsEvents.OnCollisionStay2D, collision);
		}

		void OnCollisionExit2D(Collision2D collision)
		{
			SendMessage(PhysicsEvents.OnCollisionExit2D, collision);
		}

		void OnTriggerEnter(Collider collision)
		{
			SendMessage(PhysicsEvents.OnTriggerEnter, collision);
		}

		void OnTriggerStay(Collider collision)
		{
			SendMessage(PhysicsEvents.OnTriggerStay, collision);
		}

		void OnTriggerExit(Collider collision)
		{
			SendMessage(PhysicsEvents.OnTriggerExit, collision);
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
			SendMessage(PhysicsEvents.OnTriggerEnter2D, collision);
		}

		void OnTriggerStay2D(Collider2D collision)
		{
			SendMessage(PhysicsEvents.OnTriggerStay2D, collision);
		}

		void OnTriggerExit2D(Collider2D collision)
		{
			SendMessage(PhysicsEvents.OnTriggerExit2D, collision);
		}
	}
}