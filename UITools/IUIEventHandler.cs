using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.EventSystems;

namespace Pseudo
{
	public interface IUIEventHandler :
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
	{ }
}
