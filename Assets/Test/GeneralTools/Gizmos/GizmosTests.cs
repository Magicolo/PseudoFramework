using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

public class GizmosTests : MonoBehaviour
{

	void OnDrawGizmos()
	{
		DrawUtility.DrawText(transform.position, "KWAME est content");
	}
}
