using UnityEngine;
using System.Collections;

public class ArrowPointer : MonoBehaviour {
	
	private AppController app;
	//Transform target;
	//Vector3 focusHere;

	bool mouseOver = false;
  	
	public void setMouseOver(bool mouseOver) {
		this.mouseOver = mouseOver;
	}
	// Use this for initialization
	void Start () {
		app = AppController.instance;
		//target = Camera.main.transform;
		//focusHere = gameObject.transform.position;

		//fixedLocation = 
		//app.pointer.SetActive (false);
	}
	
	// Update is called once per frame
	void OnMouseEnter () {
		if (app.thirdPersonCamera.enabled || app.movingObject)
						return;

		if (app.lastPointer != null) {
			if(app.lastPointer.GetComponent<ArrowPointer>() == null) {
				app.lastPointer.GetComponent<InventoryPointer>().setMouseOver(false);
				app.invPointer.SetActive(false);
			} else {
				app.lastPointer.GetComponent<ArrowPointer>().mouseOver = false;
			}
		}

		app.pointer.SetActive (true);
		app.pointer.GetComponentInChildren<UnityEngine.UI.Text>().text = gameObject.name;

		mouseOver = true;
		//DebugConsole.Log ("Called frame ZZZZ");
		app.lastPointer = gameObject;
	}

	void OnMouseExit() {
		mouseOver = false;
	}
	
	void Update ()
	{
		if (!mouseOver || app.thirdPersonCamera.enabled || app.movingObject) {
				//app.pointer.SetActive (false);
				return;
		}

		if (!app.pointer.activeInHierarchy)
			return;

		app.pointer.transform.position = Camera.main.WorldToScreenPoint (transform.position);
		//app.setCameraLook (gameObject.transform);
	}

}
