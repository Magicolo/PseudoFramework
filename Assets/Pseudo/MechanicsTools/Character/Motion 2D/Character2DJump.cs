using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character2DJump : PStateLayer
{
	public Gravity Gravity { get { return Layer.Gravity; } }
	public bool Grounded { get { return Layer.Grounded; } set { Layer.Grounded = value; } }
	public bool Jumping { get { return Layer.Jumping; } set { Layer.Jumping = value; } }
	public int JumpingHash { get { return Layer.JumpingHash; } }
	public Animator Animator { get { return Layer.Animator; } }
	public Rigidbody2D Rigidbody { get { return Layer.Rigidbody; } }
	public InputSystem InputSystem { get { return Layer.InputSystem; } }
	new public Character2DMotion Layer { get { return ((Character2DMotion)base.Layer); } }
}
