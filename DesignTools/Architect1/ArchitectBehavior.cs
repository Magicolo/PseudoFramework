using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;

namespace Pseudo.Architect
{
	[Serializable]
	public class ArchitectBehavior : MonoBehaviour
	{
		[Inject(),Disable()]
		public ArchitectControler Architect;
		public UISkin Skin;
		public UIFactory UIFactory;
		public ArchitectLinker Linker;
		public Camera MapCam;
		public Camera UICam;

		void Start()
		{
			Architect.MapCam = MapCam;
			Architect.UICam = UICam;
			Architect.Linker = Linker;
		}

		public void CreateNewMap(string text, int width, int height)
		{
			Architect.CreateNewMap(text, width, height);
		}
	}
}
