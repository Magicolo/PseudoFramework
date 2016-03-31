using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class InputFieldUtility
	{

		public static int GetInt(InputField inputField)
		{
			int value = 0;
			if (!int.TryParse(inputField.text, out value))
				value = 0;
			return value;
		}
	}
}
