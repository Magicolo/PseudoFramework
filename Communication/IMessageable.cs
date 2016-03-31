using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Communication
{
	public interface IMessageable
	{
		void OnMessage<TId>(TId message);
	}

	public interface IMessageable<TId>
	{
		void OnMessage(TId message);
	}
}
