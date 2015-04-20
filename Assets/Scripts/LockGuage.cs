using UnityEngine;
using System.Collections;

public class LockGuage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void onMouseDown() {
		AppController.instance.setGuageLock (true);
		Debug.Log ("Guage locked");
	}

	void onMouseDrag() {
		AppController.instance.setGuageLock (true);
		Debug.Log ("Guage locked");
	}

	void onMouseUp() {
		AppController.instance.setGuageLock (false);
		Debug.Log ("Guage unlocked");
	}
}
