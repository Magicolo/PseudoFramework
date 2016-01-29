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
		public Transform DrawArea;
		public UIFactory factory;

		public Architect architect;


		[Button("Refresh", "Refresh")]
		public bool refresh;
		public void Refresh()
		{
			DestroyChilds();

			string[] files = Directory.GetFiles(Application.dataPath + "/map", "*.arc");

			int y = 0;
			foreach (var file in files)
			{
				string filename = Path.GetFileName(file);
				Button button = factory.CreateButton(DrawArea, Vector3.zero, new Vector2(100, 10), filename, () => OpenFile(file));
				button.SetAnchors(Vector2.zero, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f));
				button.transform.SetLocalScale(Vector3.one);
				button.transform.SetPosition(Vector3.zero);

				button.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20f);
				button.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
				button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);

				y += 22;
			}
		}

		void OpenFile(string path)
		{
			architect.Open(path);
		}

		private void DestroyChilds()
		{
			foreach (var item in DrawArea.gameObject.GetChildren())
			{
				item.Destroy();
			}
		}
	}

}
