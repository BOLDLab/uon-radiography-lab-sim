using UnityEngine;
using System.Collections;

public class ThirdPersonActions : MonoBehaviour {
	private static DoorInterface doorInterface;
	private static XRayInterface xrayInterface;
	private string currentAction = "";
	private int currentCam;
	private UserActions userActions;
	private XRayConsole console;
	public Light xRayTableSpot;

	public GameObject arm;

	public Texture2D texture;
	
	// Use this for initialization
	void Start () {
		doorInterface = GameObject.FindObjectOfType<DoorInterface> ();
		xrayInterface = GameObject.FindObjectOfType<XRayInterface> ();
		console = GameObject.FindObjectOfType<XRayConsole> ();
		xRayTableSpot.enabled = false;
		userActions = new UserActions ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void perform(string action, int cam) {
		currentAction = action;
		currentCam = cam;
		if (action == "open door") {
				doorInterface.openDoor();
		}
	}

	public void clear() {
		this.currentAction = "";
	}

	void OnGUI() {
		if (currentAction == "open door" || currentAction == "plate shelf") {
			GUI.Window (4, new Rect (20, 50, 300, 100), mainFunc, "What would you like to do, "+MessageScript.instance.screen_name+"?");
		}

		if (currentAction == "standing plate") {
			xrayInterface.showGUIInterface("stand");
			AppController.instance.cameras[currentCam].enabled = true;
			activateSpotLight("upright");
			resetOtherCameras();
		}
		if (currentAction == "x-ray table") {
			xrayInterface.showGUIInterface("table");
			arm.collider.enabled = false;
			//userActions.perform(currentAction);
			AppController.instance.cameras[currentCam].enabled = true;
			AppController.instance.toggleRollOvers (false);
			arm.collider.enabled = false;
			activateSpotLight("xRaySpot");
			resetOtherCameras();
		}
		if (currentAction == "console") {
			if(!console.isVisible()) {
				AppController.instance.cameras[currentCam].enabled = true;
				console.showPanel(true);
				activateSpotLight("console");
				resetOtherCameras();
			} 
		}
	
	}

	private void activateSpotLight(string id) {
		if (id == "xRaySpot") {
					xRayTableSpot.enabled = true;
		} else {
					xRayTableSpot.enabled = false;
		}
	}

	void mainFunc(int id) {
		if (currentAction == "open door") {
			if(GUILayout.Button ("Enter the X-Ray Room")) {
				doorInterface.closeDoor();
				AppController.instance.cameras[currentCam].enabled = true;
				resetOtherCameras();
			}
		}
		if (currentAction == "plate shelf") {
			AppController.instance.cameras[currentCam].enabled = true;
			if(userActions.perform (currentAction)) {
				resetOtherCameras();
			}
		}
	}

	void resetOtherCameras() {
		int count = 0;
		foreach(Camera _cam in AppController.instance.cameras) {
			if(count++ != currentCam) {
				_cam.enabled = false;
			}
		}
		Debug.Log ("Activated camera:  "+AppController.instance.cameras[currentCam].transform.name);
		
		currentAction = "";
	}
}
