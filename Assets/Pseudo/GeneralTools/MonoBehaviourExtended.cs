using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract class MonoBehaviourExtended : MonoBehaviour
	{
		bool _gameObjectCached;
		GameObject _gameObject;
		new public GameObject gameObject
		{
			get
			{
				_gameObject = _gameObjectCached ? _gameObject : base.gameObject;
				_gameObjectCached = true;
				return _gameObject;
			}
		}

		bool _transformCached;
		Transform _transform;
		new public Transform transform
		{
			get
			{
				_transform = _transformCached ? _transform : GetComponent<Transform>();
				_transformCached = true;
				Logger.Log("YAY");
				return _transform;
			}
		}
	}
}

