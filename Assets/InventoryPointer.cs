using UnityEngine;
using System.Collections;

public class InventoryPointer : MonoBehaviour {
	
	private AppController app;

	private GameObject focusHere;
	bool mouseOver;
	InventoryItem item;
	public float clickDistance = 3.0f;

	public void setMouseOver(bool mouseOver) {
		this.mouseOver = mouseOver;
	}
	// Use this for initialization
	void Start () {
		app = AppController.instance;
	}

	void initComponent() {
		item = GetComponent<InventoryItem> ();
		

			focusHere = gameObject;
	
	}

	public void OnMouseEnter () {
		if (item == null) {
			initComponent();
		}

		if (item.getIgnoreCollider ()) {
			return;
		}

		if (!enabled || !app.checkRenderersVisible(focusHere) || app.thirdPersonCamera.enabled)
						return;

		if (Vector3.Distance (focusHere.transform.position, Camera.main.transform.position) > clickDistance)
			return;

		if (app.lastPointer != null) {
			if(app.lastPointer.GetComponent<InventoryPointer>() == null) {
				app.lastPointer.GetComponent<ArrowPointer>().setMouseOver(false);
				app.pointer.SetActive(false);
			} else {
				InventoryPointer ip = app.lastPointer.GetComponent<InventoryPointer>();
				if(!ip.gameObject.Equals(focusHere)) {
					ip.mouseOver = false;
				}
			}
		}
		
		app.invPointer.SetActive (true);
		app.invPointer.GetComponentInChildren<UnityEngine.UI.Text> ().text = item.displayName;

		mouseOver = true;

		app.lastPointer = focusHere;
	}
	
	/*void OnMouseExit() {
		mouseOver = false;
	}*/

	// Update is called once per frame
	void Update () {
	
		if (!mouseOver || app.thirdPersonCamera.enabled) {
			//app.pointer.SetActive (false);
			return;
		}

		if (!app.invPointer.activeInHierarchy)
			return;

		if (Vector3.Distance (focusHere.transform.position, Camera.main.transform.position) > clickDistance)
						return;

		app.invPointer.transform.position = Camera.main.WorldToScreenPoint (focusHere.transform.position);
		app.setCameraLook (gameObject.transform);
		//app.invPointer.transform.position = (Camera.main.transform.position + focusHere) / 2;
	}
}
