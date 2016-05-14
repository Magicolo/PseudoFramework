using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public struct InjectionArgument : IEquatable<InjectionArgument>
	{
		public struct Enumerator : IEnumerator<InjectionArgument>
		{
			public InjectionArgument Current
			{
				get { return current; }
			}
			object IEnumerator.Current
			{
				get { return Current; }
			}

			InjectionArgument[] arguments;
			ContextTypes filter;
			InjectionArgument current;
			int currentIndex;

			public Enumerator(InjectionArgument[] arguments, ContextTypes filter) : this()
			{
				this.arguments = arguments;
				this.filter = filter;
				currentIndex = arguments.Length;
			}

			public bool MoveNext()
			{
				while (currentIndex >= 0)
				{
					current = arguments[currentIndex];
					currentIndex--;

					if (current.ContextType.Contains(filter))
						return true;
				}

				return false;
			}

			public void Reset()
			{
				currentIndex = 0;
			}

			public void Dispose()
			{
				filter = ContextTypes.None;
				arguments = null;
				Reset();
			}
		}

		public readonly object Value;
		public readonly ContextTypes ContextType;

		public InjectionArgument(object value, ContextTypes contextType)
		{
			Value = value;
			ContextType = contextType;
		}

		public bool Equals(InjectionArgument other)
		{
			return
				ContextType == other.ContextType &&
				Value == other.Value;
		}
	}
}
