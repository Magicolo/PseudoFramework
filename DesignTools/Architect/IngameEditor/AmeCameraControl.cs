using UnityEngine;
using System.Collections.Generic;
using Pseudo;

public class AmeCameraControl : MonoBehaviour 
{

	public Camera Cam;

	bool middleCliçked;
	Vector2 lastMousePosition;
	public float CamMouvementFactor = 0.01f;
	public float ArrowCamMouvementSpeed = 2f;

	void Start () 
	{
	
	}
	
	
	void Update () 
	{
		handleArrowCamMouvement();
		handleMiddleMouse();
	}

	void handleArrowCamMouvement()
	{
		float xMouvement = 0;
		float yMouvement = 0;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			xMouvement -= ArrowCamMouvementSpeed;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			xMouvement += ArrowCamMouvementSpeed;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			yMouvement -= ArrowCamMouvementSpeed;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			yMouvement += ArrowCamMouvementSpeed;
		}
		Cam.transform.Translate(new Vector3(xMouvement, yMouvement, 0) * Time.deltaTime);
	}

	void handleMiddleMouse()
	{
		if (Input.GetMouseButton(2) && middleCliçked)
		{
			Vector2 newMousePosition = Input.mousePosition;
			Vector2 diff = lastMousePosition - newMousePosition;
			Cam.transform.Translate(diff * CamMouvementFactor);
			lastMousePosition = newMousePosition;

		}
		else if (Input.GetMouseButtonDown(2))
		{
			lastMousePosition = Input.mousePosition;
			middleCliçked = true;
		}
		else if (Input.GetMouseButtonUp(2))
		{
			middleCliçked = false;
		}


	}
}
