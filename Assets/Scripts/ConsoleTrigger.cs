using UnityEngine;
using System.Collections;

public class ConsoleTrigger : MonoBehaviour {

	public GameObject consoleObj;
	private XRayConsole console;
	// Use this for initialization
	void Start () {
		console = consoleObj.GetComponent<XRayConsole> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		console.showPanel (true);
		AppController.instance.currentLocation = console.gameObject;//.transform.position;
	}
}
