using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class CharacterLive : StateLayer
{
	new public CharacterStatus Layer { get { return ((CharacterStatus)base.Layer); } }
}
