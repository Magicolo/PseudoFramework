using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class TriggerEvent<TId> : IEvent where TId : IEquatable<TId>
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier);
		}
	}

	public class TriggerEvent<TId, TArg> : IEvent where TId : IEquatable<TId>
	{
		public EventGroup<TId> EventGroup;
		public TId Identifier;
		public TArg Argument;

		public void Resolve()
		{
			EventGroup.Trigger(Identifier, Argument);
		}
	}

	public class TriggerEvent<TId, TArg1, TArg2> : IEvent where TId : IEquatable<TId>
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

	public class TriggerEvent<TId, TArg1, TArg2, TArg3> : IEvent where TId : IEquatable<TId>
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
