using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.EventSystems;
using Pseudo.EntityFramework;

namespace Pseudo
{
	[Flags]
	public enum UIEvents
	{
		OnPointerClick = 1 << 0,
		OnPointerDown = 1 << 1,
		OnPointerUp = 1 << 2,
		OnPointerEnter = 1 << 3,
		OnPointerExit = 1 << 4,
		OnBeginDrag = 1 << 5,
		OnEndDrag = 1 << 6,
		OnDrag = 1 << 7,
		OnDrop = 1 << 8,
		OnInitializePotentialDrag = 1 << 9,
		OnSelect = 1 << 10,
		OnDeselect = 1 << 11,
		OnMove = 1 << 12,
		OnScroll = 1 << 13,
		OnSubmit = 1 << 14,
		OnCancel = 1 << 15,
		OnUpdateSelected = 1 << 16,
	}

	public class MessageOnUIEvent : ComponentBehaviour, IUIEventHandler
	{
		[Serializable]
		public struct UIMessage
		{
			[EnumFlags]
			public UIEvents Events;
			public EntityMessage Message;
		}

		public UIMessage[] Messages = new UIMessage[0];

		void SendMessage(UIEvents uiEvent, BaseEventData eventData)
		{
			for (int i = 0; i < Messages.Length; i++)
			{
				var message = Messages[i];

				if (Active && Entity != null && (message.Events & uiEvent) != 0)
					Entity.SendMessage(message.Message, eventData);
			}
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnPointerClick, eventData);
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnPointerDown, eventData);
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnPointerUp, eventData);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnPointerEnter, eventData);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnPointerExit, eventData);
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnBeginDrag, eventData);
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnEndDrag, eventData);
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnDrag, eventData);
		}

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnDrop, eventData);
		}

		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnInitializePotentialDrag, eventData);
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			SendMessage(UIEvents.OnSelect, eventData);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			SendMessage(UIEvents.OnDeselect, eventData);
		}

		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			SendMessage(UIEvents.OnMove, eventData);
		}

		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			SendMessage(UIEvents.OnScroll, eventData);
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			SendMessage(UIEvents.OnSubmit, eventData);
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			SendMessage(UIEvents.OnCancel, eventData);
		}

		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			SendMessage(UIEvents.OnUpdateSelected, eventData);
		}
	}
}