using UnityEngine;
using System.Collections;

public class MouseOverPulse : MonoBehaviour {
	Vector3 atRest;
	Vector3 targetScale;
	bool mouseOver = false;
	bool pulseUp = true;
	AppController app;
	Information info;
	UnityEngine.UI.Image image;
	UnityEngine.UI.Image[] childImage;
	UnityEngine.UI.Text[] text;

	// Use this for initialization
	void Start () {
		app = AppController.instance;
		atRest = transform.localScale;
		image = gameObject.GetComponent<UnityEngine.UI.Image> ();
		text = gameObject.GetComponentsInChildren<UnityEngine.UI.Text> ();
		childImage = gameObject.GetComponentsInChildren<UnityEngine.UI.Image> ();
		targetScale = new Vector3 (transform.localScale.x + 0.2F, transform.localScale.y + 0.2F, transform.localScale.z + 0.2F);
	}

	public void onMouseDown() {
		info = app.lastPointer.gameObject.GetComponent<Information> ();
		app.lastLookedInfo = info;
		
		app.setClickedMarker (true);
		app.showThirdPCamera();	

		app.actionButton1.SetActive (false);
		app.actionButton2.SetActive (false);
		app.infoButton1.SetActive (false);
		app.infoButton2.SetActive (false);
		app.infoButton3.SetActive (false);
	}

	public void onMouseUp() {
		app.setClickedMarker (false);
	}

	public void pulse() {

		mouseOver = !mouseOver;
	}

	void Update() {
		if(image == null) return;
		if (app == null)
			return;

		if (gameObject.activeInHierarchy) {
						//image.enabled = childImage.enabled = !app.thirdPersonCamera.enabled;

						foreach (UnityEngine.UI.Image img in childImage) {
							img.enabled = !app.thirdPersonCamera.enabled;
						}

						foreach (UnityEngine.UI.Text t in text) {
							t.enabled = !app.thirdPersonCamera.enabled;
						}
				}

		if (mouseOver) {
						if (pulseUp) {
								transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, Time.deltaTime * 2.0f);
								if (transform.localScale == targetScale) {
										pulseUp = false;
								}
						} else {
								transform.localScale = Vector3.MoveTowards (transform.localScale, atRest, Time.deltaTime * 2.0f);
								if (transform.localScale == atRest) {
										pulseUp = true;
								}
						}
				}
	}

}
