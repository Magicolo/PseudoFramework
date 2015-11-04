using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.Tests
{
	public class GizmosTests : MonoBehaviour
	{
		void OnDrawGizmos()
		{
			DrawUtility.DrawText(transform.position, "KWAME est content");
		}
	}
}