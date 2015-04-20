using UnityEngine;
using System.Collections;

public class DisableOnThirdPersonView : MonoBehaviour {

	UnityEngine.UI.Image image;
	UnityEngine.UI.Text txt;
	AppController app;
	// Use this for initialization
	void Start () {
		app = AppController.instance;
		image = gameObject.GetComponent<UnityEngine.UI.Image> ();
		txt = gameObject.GetComponent<UnityEngine.UI.Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(image == null) return;
		if (txt == null)
						return;
		if (app == null)
						return;
		 
		if (gameObject.activeInHierarchy) {
			txt.enabled = image.enabled = !app.thirdPersonCamera.enabled && !app.focusedOnItem;
		}
	}
}
