using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	[System.Serializable]
	public class InputCombinaisonChecker
	{
		KeyCode[] keyCodes;

		bool waitForUnPush;
		public bool mustWaitForKeyDownAfterKeyUp;

		public InputCombinaisonChecker(bool mustWaitForKeyDownAfterKeyUp, params KeyCode[] keyCodesToCheck)
		{
			this.mustWaitForKeyDownAfterKeyUp = mustWaitForKeyDownAfterKeyUp;
			keyCodes = (KeyCode[])keyCodesToCheck.Clone();
		}
		public InputCombinaisonChecker(params KeyCode[] keyCodesToCheck)
		{
			keyCodes = (KeyCode[])keyCodesToCheck.Clone();
		}

		public void Update()
		{
			if (mustWaitForKeyDownAfterKeyUp && waitForUnPush)
			{
				for (int i = 0; i < keyCodes.Length; i++)
				{
					bool getKey = Input.GetKey(keyCodes[i]);
					if (!getKey)
					{
						waitForUnPush = false;
						return;
					}
				}
			}

		}

		public bool GetKeyCombinaison()
		{
			if (waitForUnPush) return false;

			bool allPushed = true;
			for (int i = 0; i < keyCodes.Length; i++)
			{
				bool getKey = Input.GetKey(keyCodes[i]);
				if (!getKey)
					allPushed = false;

			}
			if (allPushed)
			{
				waitForUnPush = mustWaitForKeyDownAfterKeyUp;
			}
			return allPushed;
		}
	}

}
