using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	public class PRoutineHolder : IPoolable, ICopyable<PRoutineHolder>
	{
		readonly List<IEnumerator> routines = new List<IEnumerator>();

		public static readonly PRoutineHolder Default = new PRoutineHolder();

		public void Update()
		{
			for (int i = 0; i < routines.Count; i++)
			{
				if (!routines[i].MoveNext())
					routines.RemoveAt(i--);
			}
		}

		public void StartRoutine(IEnumerator routine)
		{
			routines.Add(routine);
		}

		public void StopRoutine(IEnumerator routine)
		{
			routines.Remove(routine);
		}

		public void StopAllRoutines()
		{
			routines.Clear();
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
			routines.Clear();
		}

		public void Copy(PRoutineHolder reference)
		{
		}
	}
}