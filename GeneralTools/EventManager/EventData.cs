using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class EventData<TId> : IEventData
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier);
		}
	}

	public class EventData<TId, TArg> : IEventData
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg Argument;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument);
		}
	}

	public class EventData<TId, TArg1, TArg2> : IEventData
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg1 Argument1;
		public TArg2 Argument2;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument1, Argument2);
		}
	}

	public class EventData<TId, TArg1, TArg2, TArg3> : IEventData
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

	public class EventData<TId, TArg1, TArg2, TArg3, TArg4> : IEventData
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg1 Argument1;
		public TArg2 Argument2;
		public TArg3 Argument3;
		public TArg4 Argument4;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument1, Argument2, Argument3, Argument4);
		}
	}
}
