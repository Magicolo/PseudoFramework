using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Pool
{
	public interface IPoolInitializable
	{
		/// <summary>
		/// Called just before this instance will be reset to its initial state by the pool.
		/// This callback is not garanteed to be called on the main thread; use with care.
		/// </summary>
		void OnPrePoolInitialize();
		/// <summary>
		/// Called just after this instance has been reset to its initial state by the pool.
		/// This callback is not garanteed to be called on the main thread; use with care.
		/// </summary>
		void OnPostPoolInitialize();
	}
}
