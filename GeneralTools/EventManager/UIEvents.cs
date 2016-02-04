using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class UIEvents : PEnum<UIEvents, int>
	{
		public static readonly UIEvents OnPointerEnter = new UIEvents(1);
		public static readonly UIEvents OnPointerExit = new UIEvents(2);
		public static readonly UIEvents OnPointerDown = new UIEvents(3);
		public static readonly UIEvents OnPointerUp = new UIEvents(4);
		public static readonly UIEvents OnPointerClick = new UIEvents(5);
		public static readonly UIEvents OnDrag = new UIEvents(6);
		public static readonly UIEvents OnDrop = new UIEvents(7);
		public static readonly UIEvents OnScroll = new UIEvents(8);
		public static readonly UIEvents OnUpdateSelected = new UIEvents(9);
		public static readonly UIEvents OnSelect = new UIEvents(10);
		public static readonly UIEvents OnDeselect = new UIEvents(11);
		public static readonly UIEvents OnMove = new UIEvents(12);
		public static readonly UIEvents OnInitializePotentialDrag = new UIEvents(13);
		public static readonly UIEvents OnBeginDrag = new UIEvents(14);
		public static readonly UIEvents OnEndDrag = new UIEvents(15);
		public static readonly UIEvents OnSubmit = new UIEvents(16);
		public static readonly UIEvents OnCancel = new UIEvents(17);

		protected UIEvents(int value) : base(value) { }
	}
}
