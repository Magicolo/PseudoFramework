using UnityEngine;
using System.Collections.Generic;
using Pseudo.Internal.Input;
using UnityEngine.UI;

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
			architect.New();
		}

		public void Open()
		{
			architect.Open("Assets\\Tests\\DesignTools\\Architectmap.arc");
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
