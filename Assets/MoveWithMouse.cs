﻿using UnityEngine;
using System.Collections;

public class MoveWithMouse : MonoBehaviour {

	public Camera myCam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		Vector3 v3 = Input.mousePosition;
		v3.z = 10.0f;
		v3 = myCam.ScreenToWorldPoint(v3);
		
		Debug.Log(v3); //Current Position of mouse in world space
		
		this.gameObject.rigidbody.MovePosition(v3);
		
	}
	
	
	
}