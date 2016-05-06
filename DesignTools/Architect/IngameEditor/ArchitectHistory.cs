using UnityEngine;
using System.Collections.Generic;


namespace Pseudo.Architect
{
	[System.Serializable]
	public class ArchitectHistory
	{
		public Stack<ArchitectCommand> History = new Stack<ArchitectCommand>();
		public Stack<ArchitectCommand> HistoryRedo = new Stack<ArchitectCommand>();

		public void Do(ToolCommandBase tool)
		{
			if (tool.Do())
			{
				History.Push(tool);
				HistoryRedo.Clear();
			}
		}

		public void Undo()
		{
			if (History.Count > 0)
			{
				ArchitectCommand bc = History.Pop();
				bc.Undo();
				HistoryRedo.Push(bc);
			}
		}

		public void Redo()
		{
			if (HistoryRedo.Count > 0)
			{
				ArchitectCommand command = HistoryRedo.Pop();
				command.Do();
				History.Push(command);
			}
		}
	}
}
