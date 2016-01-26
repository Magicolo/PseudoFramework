using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class TriggerEvent<TId> : IEvent
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, (object)null, (object)null, (object)null);
		}
	}

	public class TriggerEvent<TId, TArg> : IEvent
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg Argument;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument, (object)null, (object)null);
		}
	}

	public class TriggerEvent<TId, TArg1, TArg2> : IEvent
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg1 Argument1;
		public TArg2 Argument2;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument1, Argument2, (object)null);
		}
	}

	public class TriggerEvent<TId, TArg1, TArg2, TArg3> : IEvent
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg1 Argument1;
		public TArg2 Argument2;
		public TArg3 Argument3;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument1, Argument2, Argument3);
		}
	}
}
