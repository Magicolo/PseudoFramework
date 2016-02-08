using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.UI;

namespace Pseudo.UI
{
	public class FieldsGroupControler : MonoBehaviour
	{
		public InputValue[] InputFieldsDefaultValues;

		public void ResetToDefault()
		{
			for (int i = 0; i < InputFieldsDefaultValues.Length; i++)
			{
				InputValue inputValue = InputFieldsDefaultValues[i];
				inputValue.InputField.text = inputValue.DefautValue;
			}
		}
	}

	[Serializable]
	public struct InputValue
	{
		public InputField InputField;
		public string DefautValue;
	}
}