using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.PhysicsTools
{
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsZone : PMonoBehaviour
	{
		Dictionary<Rigidbody, int> rigidbodyCountDict;
		public Dictionary<Rigidbody, int> RigidbodyCountDict
		{
			get
			{
				if (rigidbodyCountDict == null)
					rigidbodyCountDict = new Dictionary<Rigidbody, int>();

				return rigidbodyCountDict;
			}
		}

		public virtual void OnRigidbodyEnter(Rigidbody attachedRigidbody)
		{

		}

		public virtual void OnRigidbodyExit(Rigidbody attachedRigidbody)
		{

		}

		void OnTriggerEnter(Collider collision)
		{
			Rigidbody attachedRigidbody = collision.attachedRigidbody;

			if (attachedRigidbody != null)
			{
				if (!RigidbodyCountDict.ContainsKey(attachedRigidbody))
				{
					RigidbodyCountDict[attachedRigidbody] = 1;

					OnRigidbodyEnter(attachedRigidbody);
				}
				else
					RigidbodyCountDict[attachedRigidbody] += 1;
			}
		}

		void OnTriggerExit(Collider collision)
		{
			Rigidbody attachedRigidbody = collision.attachedRigidbody;

			if (attachedRigidbody != null && RigidbodyCountDict.ContainsKey(attachedRigidbody))
			{
				RigidbodyCountDict[attachedRigidbody] -= 1;

				if (RigidbodyCountDict[attachedRigidbody] <= 0)
				{
					OnRigidbodyExit(attachedRigidbody);
					RigidbodyCountDict.Remove(attachedRigidbody);
				}
			}
		}
	}
}