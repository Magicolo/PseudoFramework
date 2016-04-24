﻿using UnityEngine;
using System.Collections.Generic;
using Pseudo.Input.Internal;
using UnityEngine.UI;
using Pseudo.UI.Internal;

namespace Pseudo
{
	public class ArchitectMenus : MonoBehaviour
	{

		private ArchitectBehavior architectBehavior;

		public Button SaveButton;
		public Button OpenButton;
		public Button UndoButton;
		public Button RedoButton;

		public OpenFileBrowser OpenFileBrowser;

		public GameObject NewFile;
		public InputField NewMapName;
		public InputField NewMapWidth;
		public InputField NewMapHeight;

		void Awake()
		{
			architectBehavior = GetComponentInParent<ArchitectBehavior>();
		}

		public void Save()
		{
			//architect.Save();
		}

		public void OpenNewPanel()
		{
			if (NewFile.activeInHierarchy)
			{
				NewFile.SetActive(false);
			}
			else
			{
				NewFile.SetActive(true);
				NewFile.GetComponent<FieldsGroupControler>().ResetToDefault();
			}
		}

		public void CreateNewMap()
		{
			int width = InputFieldUtility.GetInt(NewMapWidth);
			int height = InputFieldUtility.GetInt(NewMapHeight);

			architectBehavior.CreateNewMap(NewMapName.text, width, height);

			NewFile.SetActive(false);
		}

		public void Open()
		{
			if (OpenFileBrowser.gameObject.activeInHierarchy)
				OpenFileBrowser.gameObject.SetActive(false);
			else
			{
				OpenFileBrowser.gameObject.SetActive(true);
				OpenFileBrowser.Refresh();
			}

		}

		public void Redo()
		{
			//architect.Redo();
		}

		public void Undo()
		{
			//architect.Undo();
		}

		public void Refresh()
		{
			//architect.UISkin.SetEnabled(UndoButton, architect.HasHistory);
			//architect.UISkin.SetEnabled(RedoButton, architect.HasRedoHistory);
		}
	}

}

