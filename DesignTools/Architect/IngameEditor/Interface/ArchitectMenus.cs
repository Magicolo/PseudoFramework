using UnityEngine;
using System.Collections.Generic;
using Pseudo.Internal.Input;
using UnityEngine.UI;
using Pseudo.Internal.UI;

namespace Pseudo
{
	public class ArchitectMenus : MonoBehaviour
	{

		private Architect architect;

		public Button NewButton;
		public Button SaveButton;
		public Button OpenButton;
		public Button UndoButton;
		public Button RedoButton;

		public OpenFileBrowser OpenFileBrowser;
		public GameObject NewFile;

		void Awake()
		{
			architect = GetComponentInParent<Architect>();
		}

		public void Save()
		{
			architect.Save();
		}

		public void New()
		{
			NewFile.SetActive(!NewFile.activeInHierarchy);

		}

		public void NewMap(InputField input)
		{
			architect.New(input.text);
			input.text = "";
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
			architect.Redo();
		}

		public void Undo()
		{
			architect.Undo();
		}

		public void Refresh()
		{
			architect.UISkin.SetEnabled(UndoButton, architect.HasHistory);
			architect.UISkin.SetEnabled(RedoButton, architect.HasRedoHistory);
		}
	}

}
