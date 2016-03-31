using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Pooling
{
	public interface IPoolFieldsInitializable
	{
		/// <summary>
		/// Called just before the field setters are initialized for this reference instance.
		/// This callback is not garanteed to be called on the main thread; use with care.
		/// </summary>
		void OnPrePoolFieldsInitialize(IFieldInitializer initializer);
		/// <summary>
		/// Called just after the field setters have beed initialized for this reference instance.
		/// This callback is not garanteed to be called on the main thread; use with care.
		/// </summary>
		void OnPostPoolFieldsInitialize(IFieldInitializer initializer);
	}
}
