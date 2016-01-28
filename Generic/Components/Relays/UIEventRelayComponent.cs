using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.EventSystems;

namespace Pseudo
{
	public class UIEventRelayComponent : ComponentBehaviour,
		IPointerEnterHandler,
		IPointerExitHandler,
		IPointerDownHandler,
		IPointerUpHandler,
		IPointerClickHandler,
		IBeginDragHandler,
		IInitializePotentialDragHandler,
		IDragHandler,
		IEndDragHandler,
		IDropHandler,
		IScrollHandler,
		IUpdateSelectedHandler,
		ISelectHandler,
		IDeselectHandler,
		IMoveHandler,
		ISubmitHandler,
		ICancelHandler
	{
		public static readonly Pool<UIEventData> EventDataPool = new Pool<UIEventData>(new UIEventData(), () => new UIEventData(), 0);
		public static readonly Queue<UIEventData> QueuedEvents = new Queue<UIEventData>();

		public class UIEventData
		{
			public UIEvents Event;
			public IEntity Entity;
			public BaseEventData Data;
		}

		public UIEvents Events;

		public void EnqueueEvent(UIEvents identifier, BaseEventData data)
		{
			if (Events.HasAll(identifier))
			{
				var uiEvent = TypePoolManager.Create<UIEventData>();
				uiEvent.Event = identifier;
				uiEvent.Entity = Entity.Entity;
				uiEvent.Data = data;

				QueuedEvents.Enqueue(uiEvent);
			}
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnBeginDrag, eventData);
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			EnqueueEvent(UIEvents.OnCancel, eventData);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			EnqueueEvent(UIEvents.OnDeselect, eventData);
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnDrag, eventData);
		}

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnDrop, eventData);
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnEndDrag, eventData);
		}

		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnInitializePotentialDrag, eventData);
		}

		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			EnqueueEvent(UIEvents.OnMove, eventData);
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnPointerClick, eventData);
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnPointerDown, eventData);
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnPointerEnter, eventData);
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnPointerExit, eventData);
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnPointerUp, eventData);
		}

		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			EnqueueEvent(UIEvents.OnScroll, eventData);
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			EnqueueEvent(UIEvents.OnSelect, eventData);
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			EnqueueEvent(UIEvents.OnSubmit, eventData);
		}

		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			EnqueueEvent(UIEvents.OnUpdateSelected, eventData);
		}
	}
}