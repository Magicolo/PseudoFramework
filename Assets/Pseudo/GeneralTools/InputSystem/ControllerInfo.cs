using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[System.Serializable]
	public abstract class ControllerInfo : INamable
	{
		[SerializeField]
		string _name = "";
		public string Name { get { return _name; } set { _name = value; } }

		[SerializeField]
		List<MonoBehaviour> _listenerReferences = new List<MonoBehaviour>();
		List<IInputListener> _listeners = new List<IInputListener>();

		protected ControllerInfo(string name, IInputListener[] listeners)
		{
			_name = name;
			_listeners = new List<IInputListener>(listeners);

			SetListeners();
		}

		public IInputListener[] GetListeners()
		{
			return _listeners.ToArray();
		}

		public void SetListeners(IInputListener[] listeners)
		{
			_listeners = new List<IInputListener>(listeners);

			SetListeners();
		}

		public void SetListeners()
		{
			_listeners = new List<IInputListener>();

			for (int i = 0; i < _listenerReferences.Count; i++)
			{
				IInputListener listener = _listenerReferences[i] as IInputListener;

				if (listener != null)
					_listeners.Add(listener);
			}
		}

		public void AddListener(IInputListener listener)
		{
			if (!_listeners.Contains(listener))
				_listeners.Add(listener);
		}

		public void RemoveListener(IInputListener listener)
		{
			_listeners.Remove(listener);
		}

		public bool HasListeners()
		{
			return _listeners.Count > 0;
		}

		public void CopyListeners(ControllerInfo info)
		{
			SetListeners(info.GetListeners());
		}

		public void SwitchListeners(ControllerInfo info)
		{
			IInputListener[] otherListeners = info.GetListeners();

			info.SetListeners(GetListeners());
			SetListeners(otherListeners);
		}

		public void SendButtonInput(string inputName, ButtonStates state)
		{
			for (int i = 0; i < _listeners.Count; i++)
				_listeners[i].OnButtonInput(new ButtonInput(Name, inputName, state));
		}

		public void SendAxisInput(string inputName, float value)
		{
			for (int i = 0; i < _listeners.Count; i++)
				_listeners[i].OnAxisInput(new AxisInput(Name, inputName, value));
		}
	}
}