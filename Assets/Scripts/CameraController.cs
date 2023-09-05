using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float zoomSpeed = 2.0f;
	public float minFOV = 10.0f;
	public float maxFOV = 60.0f;
	public float dragSpeed = 1.0f;
	private Vector2 dragLimitMin;
	private Vector2 dragLimitMax;

	private Camera mainCamera;
	private Vector2 lastTouchPosition;
	private bool isDragging;

	private void Start()
	{
		mainCamera = Camera.main;
		dragLimitMin = new Vector2(-5f, -5f);
		dragLimitMax = new Vector2(5f, 5f);
	}

	private void Update()
	{
		// Detect pinch gesture for zooming.
		if (Input.touchCount == 2)
		{
			Touch touch0 = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);

			// Calculate the initial and current distance between the touches.
			Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
			Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
			float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
			float touchDeltaMag = (touch0.position - touch1.position).magnitude;

			// Calculate the change in distance.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// Adjust the camera's field of view based on the pinch gesture.
			float newFOV = mainCamera.fieldOfView + deltaMagnitudeDiff * zoomSpeed;

			// Clamp the FOV within the minFOV and maxFOV range.
			newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

			// Set the camera's field of view to the new value.
			mainCamera.fieldOfView = newFOV;
		}
		else if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);

			switch (touch.phase)
			{
				case TouchPhase.Began:
					lastTouchPosition = touch.position;
					isDragging = true;
					break;

				case TouchPhase.Moved:
					if (isDragging)
					{
						// Calculate the delta position of the touch.
						Vector2 deltaPosition = touch.position - lastTouchPosition;

						// Convert Vector2 to Vector3 and specify the Z component as 0.
						Vector3 deltaPosition3D = new Vector3(deltaPosition.x, deltaPosition.y, 0);

						// Apply drag speed.
						Vector3 newPosition = mainCamera.transform.position - deltaPosition3D * dragSpeed * Time.deltaTime;

						// Clamp the camera position within the specified limits.
						newPosition.x = Mathf.Clamp(newPosition.x, dragLimitMin.x, dragLimitMax.x);
						newPosition.y = Mathf.Clamp(newPosition.y, dragLimitMin.y, dragLimitMax.y);

						// Update the camera position.
						mainCamera.transform.position = newPosition;

						lastTouchPosition = touch.position;
					}
					break;

				case TouchPhase.Ended:
					isDragging = false;
					break;
			}
		}
	}
}