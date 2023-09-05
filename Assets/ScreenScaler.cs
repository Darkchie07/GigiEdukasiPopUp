using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScaler : MonoBehaviour
{
	RectTransform rt;
	void Start()
	{
		rt = GetComponent<RectTransform>();
		// Get the width and height of the screen
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;

		rt.sizeDelta = new Vector2(screenWidth, screenHeight);
	}
}
