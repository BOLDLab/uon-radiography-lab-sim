using UnityEngine;
using System.Collections;

public class FirstPersonActions : MonoBehaviour {

	private CamTrigger camTrigger;

	private UserActions userActions;
	private XRayInterface xrayInterface;

	public void Start() {
		userActions = new UserActions ();
		xrayInterface = GameObject.FindObjectOfType<XRayInterface> ();
	}

	public void Update() {

	}

	public void SetCamTrigger(CamTrigger camTrigger) {
		this.camTrigger = camTrigger;
	}	

	public void Layout(string context) {
		try {
			if (context == "sink") {
				GUILayout.Button ("Wash your hands");
			}

			if (context == "plate shelf") {
				userActions.perform (context);
			}

			if (context == "standing plate") {
				xrayInterface.showGUIInterface("stand");
			}

			if (context == "x-ray table") {
				xrayInterface.showGUIInterface("table");
			}

			if(GUILayout.Button ("Exit to room")) {
				this.camTrigger.camOff();
			}
		}	catch(System.SystemException exc) {
			Debug.Log ("Caught exception: "+exc);
		}
	}

}