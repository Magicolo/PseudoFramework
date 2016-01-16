using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public interface IEventDispatcher
	{
		void Subscribe(Delegate receiver);
		void Unsubscribe(Delegate receiver);
		void Trigger();
		void Trigger(object argument);
		void Trigger(object argument1, object argument2);
		void Trigger(object argument1, object argument2, object argument3);
		void Trigger(object argument1, object argument2, object argument3, object argument4);
	}
}
