using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal;

namespace Pseudo
{
	[ExecuteInEditMode]
	[AddComponentMenu("Pseudo/General/Combo System")]
	public class ComboSystem : PMonoBehaviour
	{
		[SerializeField]
		ComboSequenceManager comboManager;
		/// <summary>
		/// Used internally. You should not use this.
		/// </summary>
		public ComboSequenceManager ComboManager
		{
			get
			{
				return comboManager;
			}
		}

		[SerializeField]
		ComboInputManager inputManager;
		/// <summary>
		/// Used internally. You should not use this.
		/// </summary>
		public ComboInputManager InputManager
		{
			get
			{
				return inputManager;
			}
		}

		[SerializeField]
		ComboMessenger messenger;
		/// <summary>
		/// Used internally. You should not use this.
		/// </summary>
		public ComboMessenger Messenger
		{
			get
			{
				return messenger;
			}
		}

		public bool ComboIsStarted
		{
			get
			{
				return InputManager.comboStarted;
			}
		}

		void Initialize()
		{
			InitializeManagers();

			if (Application.isPlaying)
			{
				InitializeRuntime();
				StartAll();
			}
		}

		void InitializeManagers()
		{
			comboManager = comboManager ?? new ComboSequenceManager(this);
			comboManager.Initialize(this);
		}

		void InitializeRuntime()
		{
			inputManager = new ComboInputManager(this);
			inputManager.Initialize(this);
			messenger = new ComboMessenger(this);
			messenger.Initialize(this);
		}

		void StartAll()
		{
			ComboManager.Start();
			InputManager.Start();
			Messenger.Start();
		}

		void Awake()
		{
			Initialize();
		}

		void Update()
		{
			if (Application.isPlaying)
			{
				InputManager.Update();
			}
		}

		public void Input(int input)
		{
			InputManager.Input(input);
		}

		public void Input(System.Enum input)
		{
			InputManager.Input(input);
		}

		public void ResetCombo()
		{
			InputManager.ResetCombo();
		}

		public void AddListener(IComboListener listener)
		{
			Messenger.AddListener(listener);
		}

		public void RemoveListener(IComboListener listener)
		{
			Messenger.RemoveListener(listener);
		}

		public int[] GetComboInput(string comboName)
		{
			return ComboManager.GetComboInput(comboName);
		}

		public T[] GetComboInput<T>(string comboName)
		{
			return ComboManager.GetComboInput<T>(comboName);
		}

		public int[] GetCurrentInput()
		{
			return InputManager.GetCurrentInput();
		}

		public T[] GetCurrentInput<T>()
		{
			return InputManager.GetCurrentInput<T>();
		}

		public int[] GetValidInput()
		{
			return InputManager.GetValidInput();
		}

		public T[] GetValidInput<T>()
		{
			return InputManager.GetValidInput<T>();
		}

		public string[] GetAllCombos()
		{
			return ComboManager.GetCombos().ToStringArray();
		}

		public string[] GetValidCombos()
		{
			return InputManager.GetValidCombos().ToStringArray();
		}

		public string[] GetLockedCombos()
		{
			return ComboManager.GetLockedCombos().ToStringArray();
		}

		public string[] GetUnlockedCombos()
		{
			return ComboManager.GetUnlockedCombos().ToStringArray();
		}

		public string GetLastSuccessfulCombo()
		{
			return InputManager.lastSuccessfulCombo == null ? "" : InputManager.lastSuccessfulCombo.Name;
		}

		public void SetComboLocked(string comboName, bool locked)
		{
			ComboManager.SetComboLocked(comboName, locked);
		}

		public void SetComboTimeConstraints(string comboName, bool enable, float minDelay, float maxDelay)
		{
			ComboManager.SetComboTimeConstraints(comboName, enable, minDelay, maxDelay);
		}

		public void SetComboTimeConstraints(string comboName, bool enable)
		{
			ComboManager.SetComboTimeConstraints(comboName, enable);
		}

		public void SetComboTimeConstraints(string comboName, int inputIndex, bool enable, float minDelay, float maxDelay)
		{
			ComboManager.SetComboTimeConstraints(comboName, inputIndex, enable, minDelay, maxDelay);
		}

		public void SetComboTimeConstraints(string comboName, int inputIndex, bool enable)
		{
			ComboManager.SetComboTimeConstraints(comboName, inputIndex, enable);
		}
	}
}
