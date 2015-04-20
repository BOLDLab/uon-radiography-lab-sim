using UnityEngine;
using System.Collections;

public class ScenarioMenu : MonoBehaviour {

	public float box_X = 600;
	public float box_Y = 300;
	public float width = 500;
	public float height = 180;
	
	//private bool useButton = true;
	public float button_X = 750;
	public float button_Y = 580;
	public float buttonWidth = 213;
	public float buttonHeight = 127;

	public float buttonIndent = 25;
	public float buttonVertSpacing = 50;
	
	public GameObject activateAtLocation;

	public string title;

	private Rect infoWindow;
	private Information[] allInfos;

	// Use this for initialization
	void Start () {
		box_X = (Screen.width / 2) - (width / 2);
		box_Y = (Screen.height / 2) - (height / 2);
		
		button_X = (width / 2) - (buttonWidth / 2);
		//button_Y = box_Y - buttonHeight - 5;
		
		infoWindow = new Rect (box_X, box_Y, width, height);
		
		allInfos = GameObject.FindObjectsOfType<Information>();

		
	}
	
	// Update is called once per frame
	void Update () {
		if (AppController.startScreenWidth != Screen.width) {
			box_X = (Screen.width / 2) - (width / 2);
			box_Y = (Screen.height / 2) - (height / 2);
			
			button_X = (width / 2) - (buttonWidth / 2);
			
			infoWindow = new Rect (box_X, box_Y, width, height);
		}	
	}

	void OnGUI() {
		if (AppController.instance == null)
						return; 
		if (AppController.instance.currentLocation == activateAtLocation) {

			foreach(Information info in allInfos) {
				info.gameObject.collider.enabled = true;
				info.showText = false;
			}
			GUI.Window (4, infoWindow, scenarioFunc, title, AppController.instance.generalStyle);
		}
	}

	void scenarioFunc(int id) {
		GUI.Button (new Rect(buttonIndent,buttonVertSpacing,buttonWidth, buttonHeight), "Foot X-Ray", AppController.instance.buttonStyle);
		GUI.Button (new Rect(buttonIndent,buttonVertSpacing*2,buttonWidth, buttonHeight), "Hand X-Ray", AppController.instance.buttonStyle);
		GUI.Button (new Rect(buttonIndent,buttonVertSpacing*3,buttonWidth, buttonHeight), "Abdomen", AppController.instance.buttonStyle);
		GUI.Button (new Rect(buttonIndent,buttonVertSpacing*4,buttonWidth, buttonHeight), "Sinuses", AppController.instance.buttonStyle);
		GUI.Button (new Rect(buttonIndent,buttonVertSpacing*5,buttonWidth, buttonHeight), "Scapula", AppController.instance.buttonStyle);
	}
}
