using UnityEngine;
using System;

namespace Pseudo.Architect
{
	[Serializable]
	public class ArchitectCameraControler
	{
		ArchitectControler Architect;

		bool middleCliçked;
		Vector2 lastMousePosition;
		public float CamMouvementFactor = 0.01f;
		public float ArrowCamMouvementSpeed = 2f;

		Camera Cam { get { return Architect.MapCam; } }

		void Start()
		{

		}


		void Update()
		{
			handleArrowCamMouvement();
			handleMiddleMouse();
		}

		void handleArrowCamMouvement()
		{
			float xMouvement = 0;
			float yMouvement = 0;
			if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
			{
				xMouvement -= ArrowCamMouvementSpeed;
			}
			if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
			{
				xMouvement += ArrowCamMouvementSpeed;
			}
			if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
			{
				yMouvement -= ArrowCamMouvementSpeed;
			}
			if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
			{
				yMouvement += ArrowCamMouvementSpeed;
			}
			Cam.transform.Translate(new Vector3(xMouvement, yMouvement, 0) * Time.deltaTime);
		}

		void handleMiddleMouse()
		{
			if (UnityEngine.Input.GetMouseButton(2) && middleCliçked)
			{
				Vector2 newMousePosition = UnityEngine.Input.mousePosition;
				Vector2 diff = lastMousePosition - newMousePosition;
				Cam.transform.Translate(diff * CamMouvementFactor);
				lastMousePosition = newMousePosition;

			}
			else if (UnityEngine.Input.GetMouseButtonDown(2))
			{
				lastMousePosition = UnityEngine.Input.mousePosition;
				middleCliçked = true;
			}
			else if (UnityEngine.Input.GetMouseButtonUp(2))
			{
				middleCliçked = false;
			}


		}
	}
}
