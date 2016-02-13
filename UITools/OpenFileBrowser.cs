using UnityEngine;
using System.Collections.Generic;
using Pseudo;
using System;
using System.IO;
using UnityEngine.UI;

namespace Pseudo.Internal.UI
{
	[System.Serializable]
	public class OpenFileBrowser : MonoBehaviour
	{
		public Transform ContentArea;
		public GameObject RowPrefab;

		public string BrowsingPath;
		//public ArchitectOld architect;



		[Button("Refresh", "Refresh")]
		public bool refresh;
		public void Refresh()
		{
			DestroyChilds();

			CreateButton("..", () => BackFolder());

			string[] directories = Directory.GetDirectories(BrowsingPath);
			foreach (var directory in directories)
				CreateSelectDirectoryButton(directory);

			string[] files = Directory.GetFiles(BrowsingPath, "*.arc");
			foreach (var file in files)
				CreateSelectFileButton(file);
		}

		private void CreateSelectDirectoryButton(string file)
		{
			string filename = Path.GetFileName(file);
			CreateButton(filename + "/", () => OpenDirectory(file));
		}

		private void CreateSelectFileButton(string file)
		{
			string filename = Path.GetFileName(file);
			CreateButton(filename, () => OpenFile(file));
		}

		private void CreateButton(string name, UnityEngine.Events.UnityAction action)
		{
			GameObject rowPrefabGo = UnityEngine.Object.Instantiate(RowPrefab);
			rowPrefabGo.SetActive(true);
			Button button = rowPrefabGo.GetComponent<Button>();
			button.GetComponentInChildren<Text>().text = name;
			var rect = rowPrefabGo.GetComponent<RectTransform>();
			rect.SetParent(ContentArea, false);

			button.onClick.AddListener(action);
		}

		void OpenFile(string path)
		{
			Debug.Log(path);
			//architect.Open(path);
		}

		private void OpenDirectory(string directory)
		{
			BrowsingPath = directory;
			Refresh();
		}

		private void BackFolder()
		{
			BrowsingPath = Directory.GetParent(BrowsingPath).FullName;
			Refresh();
		}

		private void DestroyChilds()
		{
			foreach (var item in ContentArea.gameObject.GetChildren())
			{
				item.Destroy();
			}
		}
	}

}
