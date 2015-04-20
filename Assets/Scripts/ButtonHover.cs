using UnityEngine;
using System.Collections;

public class ButtonHover : MonoBehaviour {

	public GameObject draggableObject;
	public Camera activeCamera;

	public GameObject positionToTrigger;
	public Transform moveCamTo;
	public Transform returnCamTo;
	public string message = "Click/Touch Unit and Drag to Move Left or Right";

	public string dragId = "";
	public int dragButtonReference = 0;

	private Drag dragToActivate;
	public GameObject originEmpty;

	private MoveCamera moveCamera;
	// Use this for initialization
	void Start () {
		if (dragId != "") {
				Drag[] drags = draggableObject.GetComponents<Drag> ();
				
				foreach(Drag drag in drags) {
					if(drag.id == dragId) {
						dragToActivate = drag;
						break;
					}
				}

		} else {
				dragToActivate = draggableObject.GetComponent<Drag> ();
		}

		moveCamera = activeCamera.GetComponent<MoveCamera> ();
	}

	public void resetCamPostionOnDragRelease() {
		if (moveCamTo != null) {
			if(positionToTrigger != null) {
				positionToTrigger.transform.position = returnCamTo.position;
				positionToTrigger.transform.rotation = returnCamTo.rotation;
			} else {
				activeCamera.transform.position = returnCamTo.position;
				activeCamera.transform.rotation = returnCamTo.rotation;
			}
		}

		if (null != positionToTrigger) {
			activeCamera.transform.position = originEmpty.transform.position;
			activeCamera.transform.rotation = originEmpty.transform.rotation;
		}
	}

	void OnMouseEnter() {
		AppController.instance.setInfoUIText(message);
	}

	void OnMouseDown() {
		if(moveCamera != null)
		moveCamera.enabled = false;	

		dragToActivate.enabled = true;
		dragToActivate.collider.enabled = true;
		//dragToActivate.activateButton(dragButtonReference);

		if (null != positionToTrigger) {
			activeCamera.transform.position = positionToTrigger.transform.position;
			activeCamera.transform.rotation = positionToTrigger.transform.rotation;
		}

		if (moveCamTo != null) {
			if(positionToTrigger != null) {
				positionToTrigger.transform.position = moveCamTo.position;
				positionToTrigger.transform.rotation = moveCamTo.rotation;
			} else {
				activeCamera.transform.position = moveCamTo.position;
				activeCamera.transform.rotation = moveCamTo.rotation;
			}
		}

	}
	
/*	void OnGUI() {
		if (null == dragToActivate)
						return;

		if (dragToActivate.enabled) {
			//GUI.Label(new Rect((Screen.height/2) - 320, (Screen.width/2)-200, 300, 160), message, AppController.instance.generalStyle);
			AppController.instance.setInfoUIText(message);
		}
	}*/
	
}
