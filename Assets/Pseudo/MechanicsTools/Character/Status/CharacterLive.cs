using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class CharacterLive : PStateLayer
{
	new public CharacterStatus Layer { get { return ((CharacterStatus)base.Layer); } }
}
