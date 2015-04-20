using UnityEngine;
using System.Collections;

public class TrackingTriangle : MonoBehaviour {

	//public GameObject track;

	AppController app;
	// Use this for initialization
	void Start () {
		app = AppController.instance;
	}
	
	// Update is called once per frame
	void Update () {

		if (app.lastPointer == null) {
						return;
		}

		Camera myCam;

		myCam = Camera.main;
		if (myCam == null || !myCam.enabled) {
			myCam = app.thirdPersonCamera;
		}

		Vector3 diff = myCam.ScreenToWorldPoint(app.lastPointer.transform.position) - Vector3.up;
		diff.Normalize();
		
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}
}
