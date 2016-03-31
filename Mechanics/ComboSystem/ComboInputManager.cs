using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Mechanics.Internal
{
	[Serializable]
	public class ComboInputManager
	{
		public List<ComboSequence> ValidCombos = new List<ComboSequence>();
		public List<int> CurrentInput = new List<int>();
		public ComboSequence LastSuccessfulCombo;
		public bool ComboStarted;
		public ComboSystem ComboSystem;

		int currentInputIndex;
		float inputCounter;

		public ComboInputManager(ComboSystem comboSystem)
		{
			this.ComboSystem = comboSystem;
		}

		public void Initialize(ComboSystem comboSystem)
		{
			this.ComboSystem = comboSystem;
		}

		public void Start()
		{
			ResetCombo();
		}

		public void Update()
		{
			if (!ComboStarted)
			{
				return;
			}

			inputCounter += UnityEngine.Time.deltaTime;

			for (int i = ValidCombos.Count - 1; i >= 0; i--)
			{
				var sequence = ValidCombos[i];

				if (currentInputIndex >= sequence.items.Length)
				{
					ValidCombos.RemoveAt(i);
					continue;
				}

				var sequenceItem = sequence.items[currentInputIndex];

				if (sequenceItem.timeConstraints && inputCounter > sequenceItem.maxDelay)
				{
					ValidCombos.RemoveAt(i);
					ComboSystem.Messenger.SendOnComboFail(sequence);
				}
			}

			if (ValidCombos.Count == 0)
			{
				EndCombo();
			}
			else
			{
				ComboSystem.Messenger.SendOnComboStay();
			}
		}

		public void Input(int input)
		{
			if (!ComboStarted)
			{
				BeginCombo();
			}

			CurrentInput.Add(input);

			for (int i = ValidCombos.Count - 1; i >= 0; i--)
			{
				var sequence = ValidCombos[i];

				if (currentInputIndex >= sequence.items.Length)
				{
					ValidCombos.RemoveAt(i);
					continue;
				}

				var sequenceItem = sequence.items[currentInputIndex];

				if (sequenceItem.inputIndex == input && sequenceItem.TimingIsValid(inputCounter))
				{
					if (currentInputIndex == sequence.items.Length - 1)
					{
						LastSuccessfulCombo = ValidCombos.Pop(i);
						ComboSystem.Messenger.SendOnComboSuccess(sequence);
					}
				}
				else if (currentInputIndex > 0)
				{
					ValidCombos.RemoveAt(i);
					ComboSystem.Messenger.SendOnComboFail(sequence);
				}
				else
				{
					ValidCombos.RemoveAt(i);
				}
			}

			inputCounter = 0;
			currentInputIndex += 1;

			if (ValidCombos.Count == 0)
			{
				EndCombo();
			}
		}

		public void Input(Enum input)
		{
			Input(input.GetHashCode());
		}

		public int[] GetCurrentInput()
		{
			var input = new int[CurrentInput.Count];

			for (int i = 0; i < input.Length; i++)
			{
				input[i] = CurrentInput[i];
			}

			return input;
		}

		public T[] GetCurrentInput<T>()
		{
			if (typeof(T) != ComboSystem.ComboManager.inputEnumType)
			{
				Debug.LogError(string.Format("Type of 'T' must be {0}.", ComboSystem.ComboManager.inputEnumType.Name));
				return null;
			}

			var inputs = new T[CurrentInput.Count];

			for (int i = 0; i < inputs.Length; i++)
				inputs[i] = (T)ComboSystem.ComboManager.inputEnumValues.GetValue(CurrentInput[i]);

			return inputs;
		}

		public int[] GetValidInput()
		{
			var inputs = new List<int>();

			for (int i = ValidCombos.Count - 1; i >= 0; i--)
			{
				int value = ValidCombos[i].items[currentInputIndex].inputIndex;

				if (!inputs.Contains(value))
					inputs.Add(value);
			}

			return inputs.ToArray();
		}

		public T[] GetValidInput<T>()
		{
			if (typeof(T) != ComboSystem.ComboManager.inputEnumType)
			{
				Debug.LogError(string.Format("Type of 'T' must be {0}.", ComboSystem.ComboManager.inputEnumType.Name));
				return null;
			}

			var input = new List<T>();

			for (int i = ValidCombos.Count - 1; i >= 0; i--)
			{
				var value = (T)ComboSystem.ComboManager.inputEnumValues.GetValue(ValidCombos[i].items[currentInputIndex].inputIndex);

				if (!input.Contains(value))
					input.Add(value);
			}

			return input.ToArray();
		}

		public ComboSequence[] GetValidCombos()
		{
			return ValidCombos.ToArray();
		}

		public void BeginCombo()
		{
			ResetCombo();

			if (!ComboStarted)
			{
				ComboStarted = true;
				ComboSystem.Messenger.SendOnComboEnter();
			}

		}

		public void EndCombo()
		{
			if (ComboStarted)
			{
				ComboStarted = false;
				ComboSystem.Messenger.SendOnComboExit();
			}

			ResetCombo();
		}

		public void ResetCombo()
		{
			ValidCombos = new List<ComboSequence>(ComboSystem.ComboManager.GetUnlockedCombos());
			CurrentInput.Clear();
			//lastSuccessfulCombo = null;
			currentInputIndex = 0;
			inputCounter = 0;
		}
	}
}
