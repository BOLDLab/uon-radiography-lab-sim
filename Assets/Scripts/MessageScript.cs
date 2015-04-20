using UnityEngine;
using System.Collections;

public class MessageScript : MonoBehaviour {

	public static MessageScript instance;

	public string message = "";

	public string screen_name = "";
	public string title = "";

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		this.message = "";	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DOMmessage(string msg) {
		this.message = msg;

		string[] items = msg.Split (',');

		foreach (string param in items) {
			string[] val = param.Split(':');

			if(val[0] == "screen_name") {
				screen_name = val[1];
			}
			if(val[0] == "title") {
				title = val[1];
			}
		}
	}

}
