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
	public enum BehaviourEvents
	{
		OnApplicationFocus = 1 << 0,
		OnApplicationQuit = 1 << 1,
		OnApplicationPause = 1 << 2,
		OnBecameVisible = 1 << 3,
		OnBecameInvisible = 1 << 4,
		OnBeforeTransformParentChanged = 1 << 5,
		OnTransformParentChanged = 1 << 6,
		OnTransformChildrenChanged = 1 << 7,
		OnRectTransformRemoved = 1 << 8,
		OnRectTransformDimensionsChange = 1 << 9,
		OnMouseUpAsButton = 1 << 10,
		OnMouseUp = 1 << 11,
		OnMouseOver = 1 << 12,
		OnMouseExit = 1 << 13,
		OnMouseEnter = 1 << 14,
		OnMouseDrag = 1 << 15,
		OnMouseDown = 1 << 16,
		OnCanvasGroupChanged = 1 << 17,
	}

	public class MessageOnBehaviourEvent : ComponentBehaviour
	{
		[Serializable]
		public struct BehaviourMessage
		{
			[EnumFlags]
			public BehaviourEvents Events;
			public EntityMessage Message;
		}

		public BehaviourMessage[] Messages = new BehaviourMessage[0];

		void SendMessage(BehaviourEvents behaviourEvent)
		{
			SendMessage(behaviourEvent, (object)null);
		}

		void SendMessage<T>(BehaviourEvents behaviourEvent, T data)
		{
			for (int i = 0; i < Messages.Length; i++)
			{
				var message = Messages[i];

				if (Active && Entity != null && (message.Events & behaviourEvent) != 0)
					Entity.SendMessage(message.Message, data);
			}
		}

		void OnApplicationFocus(bool focus)
		{
			SendMessage(BehaviourEvents.OnApplicationFocus, focus);
		}

		void OnApplicationQuit()
		{
			SendMessage(BehaviourEvents.OnApplicationQuit);
		}

		void OnApplicationPause(bool pause)
		{
			SendMessage(BehaviourEvents.OnApplicationPause, pause);
		}

		void OnBecameVisible()
		{
			SendMessage(BehaviourEvents.OnBecameVisible);
		}

		void OnBecameInvisible()
		{
			SendMessage(BehaviourEvents.OnBecameInvisible);
		}

		void OnBeforeTransformParentChanged()
		{
			SendMessage(BehaviourEvents.OnBeforeTransformParentChanged);
		}

		void OnTransformParentChanged()
		{
			SendMessage(BehaviourEvents.OnTransformParentChanged);
		}

		void OnTransformChildrenChanged()
		{
			SendMessage(BehaviourEvents.OnTransformChildrenChanged);
		}

		void OnRectTransformRemoved()
		{
			SendMessage(BehaviourEvents.OnRectTransformRemoved);
		}

		void OnRectTransformDimensionsChange()
		{
			SendMessage(BehaviourEvents.OnRectTransformDimensionsChange);
		}

		void OnCanvasGroupChanged()
		{
			SendMessage(BehaviourEvents.OnCanvasGroupChanged);
		}

		void OnMouseUpAsButton()
		{
			SendMessage(BehaviourEvents.OnMouseUpAsButton);
		}

		void OnMouseUp()
		{
			SendMessage(BehaviourEvents.OnMouseUp);
		}

		void OnMouseOver()
		{
			SendMessage(BehaviourEvents.OnMouseOver);
		}

		void OnMouseExit()
		{
			SendMessage(BehaviourEvents.OnMouseExit);
		}

		void OnMouseEnter()
		{
			SendMessage(BehaviourEvents.OnMouseEnter);
		}

		void OnMouseDrag()
		{
			SendMessage(BehaviourEvents.OnMouseDrag);
		}

		void OnMouseDown()
		{
			SendMessage(BehaviourEvents.OnMouseDown);
		}
	}
}