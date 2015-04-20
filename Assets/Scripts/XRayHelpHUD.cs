using UnityEngine;
using System.Collections;

public class XRayHelpHUD : MonoBehaviour {

	public Texture bgTexture;
	public bool show = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void showPanel(bool show) {
		this.show = show;
	}
	
	public bool isVisible() {
		return this.show;
	}
	
	void OnGUI() {
		
				if (!this.show)
						return;

				GUI.DrawTexture (new Rect(0, 0, Screen.width-5, Screen.height-5), bgTexture);
	}


	// Update is called once per frame
	void Update () {
	
	}
}
