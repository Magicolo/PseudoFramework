using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public struct MessageInfo
	{
		public string Message;
		public object Value;
		public SendMessageOptions Options;
	}

	public class SendMessageHierarchyAction : HierarchyAction<MessageInfo, object>
	{
		public override bool ApplyToSelf(Transform transform, ref MessageInfo input, out object output)
		{
			output = null;
			transform.SendMessage(input.Message, input.Options);
			return false;
		}

		public override bool ApplyUpwards(Transform transform, ref MessageInfo input, out object output)
		{
			output = null;
			transform.SendMessageUpwards(input.Message, input.Options);
			return false;
		}

		public override bool ApplyDownwards(Transform transform, ref MessageInfo input, out object output)
		{
			output = null;
			transform.BroadcastMessage(input.Message, input.Options);
			return false;
		}
	}

	public class SendMessageWithArgumentHierarchyAction : HierarchyAction<MessageInfo, object>
	{
		public override bool ApplyToSelf(Transform transform, ref MessageInfo input, out object output)
		{
			output = null;
			transform.SendMessage(input.Message, input.Value, input.Options);
			return false;
		}

		public override bool ApplyUpwards(Transform transform, ref MessageInfo input, out object output)
		{
			output = null;
			transform.SendMessageUpwards(input.Message, input.Value, input.Options);
			return false;
		}

		public override bool ApplyDownwards(Transform transform, ref MessageInfo input, out object output)
		{
			output = null;
			transform.BroadcastMessage(input.Message, input.Value, input.Options);
			return false;
		}
	}
}
