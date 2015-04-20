using UnityEngine;
using System.Collections;

public class ExitFoyerMenu : MonoBehaviour {

	private XRayInterface xrayInterface;

	// Use this for initialization
	void Start () {
		xrayInterface = GameObject.FindObjectOfType<XRayInterface> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if(this.camera.enabled && xrayInterface.showExitButton) {
			GUI.Window (7, new Rect (20, 90, 250, 45), menuFunc, "");
		}
	}

	void menuFunc(int id) {
		if (GUILayout.Button ("Exit to Foyer")) {
			AppController.instance.toggleRollOvers(true);
			AppController.instance.setCamera(AppController.X_RAY_ROOM_LEFT_ENTRANCE, AppController.X_RAY_ROOM_WINDOW, true, false);
		}
	}
}
