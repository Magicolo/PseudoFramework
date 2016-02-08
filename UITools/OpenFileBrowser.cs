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

		//public ArchitectOld architect;


		[Button("Refresh", "Refresh")]
		public bool refresh;
		public void Refresh()
		{
			DestroyChilds();

			string[] files = Directory.GetFiles(Application.dataPath + "/map", "*.arc");

			Debug.Log(files.Length);
			foreach (var file in files)
			{
				string filename = Path.GetFileName(file);
				GameObject rowPrefabGo = UnityEngine.Object.Instantiate(RowPrefab);
				Toggle toggle = rowPrefabGo.GetComponent<Toggle>();
				toggle.GetComponentInChildren<Text>().text = filename;
				var rect = rowPrefabGo.GetComponent<RectTransform>();
				rect.SetParent(ContentArea, false);

				//filename, () => OpenFile(file));

			}
		}

		void OpenFile(string path)
		{
			//architect.Open(path);
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
