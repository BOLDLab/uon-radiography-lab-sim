using UnityEngine;
using System.Collections;

public class UIDisplayControl : MonoBehaviour {

	public static UIDisplayControl instance;

	Vector2 orig;
	Vector2 pos;
	
	void Awake() {
		instance = this;	
	}

	public void UIEnabled(bool enable) {
		/*Renderer[] renderas = gameObject.GetComponentsInChildren<Renderer> ();

		foreach (Renderer my_rendera in renderas) {
			my_rendera.enabled = enable;
		}*/
		
		if (!enable) {
			orig = gameObject.transform.position;
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x + 10000.0f, gameObject.transform.position.y);
		} else {
			gameObject.transform.position = orig;		
		}
	}
}
