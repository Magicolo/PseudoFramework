using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.PhysicsTools
{
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsZone : PMonoBehaviour
	{
		public List<Rigidbody> Rigidbodies = new List<Rigidbody>();
		List<int> rigidbodyCount = new List<int>();

		public virtual void OnRigidbodyEnter(Rigidbody attachedRigidbody)
		{

		}

		public virtual void OnRigidbodyExit(Rigidbody attachedRigidbody)
		{

		}

		void OnTriggerEnter(Collider collision)
		{
			Rigidbody attachedRigidbody = collision.attachedRigidbody;

			if (attachedRigidbody == null)
				return;

			int index = Rigidbodies.IndexOf(attachedRigidbody);

			if (index == -1)
			{
				Rigidbodies.Add(attachedRigidbody);
				rigidbodyCount.Add(1);
				OnRigidbodyEnter(attachedRigidbody);
			}
			else
				rigidbodyCount[index]++;
		}

		void OnTriggerExit(Collider collision)
		{
			Rigidbody attachedRigidbody = collision.attachedRigidbody;

			if (attachedRigidbody == null)
				return;

			int index = Rigidbodies.IndexOf(attachedRigidbody);


			if (rigidbodyCount[index] == 1)
			{
				Rigidbodies.RemoveAt(index);
				rigidbodyCount.RemoveAt(index);
			}
			else
				rigidbodyCount[index]--;
		}
	}
}