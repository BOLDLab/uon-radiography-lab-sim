using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class LineInterface : MonoBehaviour {
	/*
	 * Code adapted from:
	 * http://answers.unity3d.com/questions/184442/drawing-lines-from-mouse-position.html
	 * 
	*/

	public Camera camToDrawTo;
	public GameObject objectToPointTo;
	public Texture lineTexture;
	public int frameInterval = 100;
	private int frameCount = 0;

	private Vector2 lastMousePos;
	private Vector2 lastCamPos;

	// Use this for initialization
	void Start () {
		lastMousePos = Input.mousePosition;
		lastCamPos = camToDrawTo.WorldToScreenPoint (objectToPointTo.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (frameCount == frameInterval) {
						lastMousePos = Input.mousePosition;
						lastCamPos = camToDrawTo.WorldToScreenPoint (objectToPointTo.transform.position);
				}
	}

	void OnGUI() {

		if (frameCount++ == frameInterval) {
						//Debug.Log (lastCamPos + "  " + lastMousePos + " screen height: " + Screen.height);
						Drawing.DrawLine (lastMousePos, lastCamPos, Color.cyan, 5);
						//DrawLine (lastMousePos, lastCamPos, 3);
						if(frameCount > frameInterval) {
								frameCount = 0;
						}
				}
	}


}
