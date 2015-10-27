using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class CharacterDie : PStateLayer
{
	bool _spriteRendererCached;
	SpriteRenderer _spriteRenderer;
	public SpriteRenderer spriteRenderer
	{
		get
		{
			_spriteRenderer = _spriteRendererCached ? _spriteRenderer : GetComponentInChildren<SpriteRenderer>();
			_spriteRendererCached = true;
			return _spriteRenderer;
		}
	}

	new public CharacterStatus Layer { get { return ((CharacterStatus)base.Layer); } }
}
