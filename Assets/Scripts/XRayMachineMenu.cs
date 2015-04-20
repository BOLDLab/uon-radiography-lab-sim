	using UnityEngine;
using System.Collections;

public class XRayMachineMenu : MonoBehaviour {
	
	public float box_X = 20;
	public float box_Y = 60;
	private float width = 350;
	//private float height = 180;
	
	//private bool useButton = true;
	public float button_X = 5;
	public float button_Y = 5;
	public float buttonWidth = 295;
	public float buttonHeight = 20;

	public Camera activeCamera;
	public GameObject aperturePosition;
	public GameObject originPosition;
	public GameObject[] alternatePositions;

	public GameObject xRayHead;
	
	public string title;
	private bool showMenu = false;

	public GameObject collumation;
	public GameObject collEmpty;
	public GameObject uprightCollumation;

	//private Rect infoWindow;
	//private Information[] allInfos;
	public static string buttonStr = "View from X-Ray Aperture";
	//private string buttonStr2 = "Show alternate view";

	public GameObject uiPanel;
	public UnityEngine.UI.Text angleText;
	public UnityEngine.UI.Button[] buttons;
	public UnityEngine.UI.Toggle collumationToggle;
	
	public float testX = 90;
	public float testY = 0;
		
//	Animator anim;
	//private string inAnim = "xray-slidein";
	//private string outAnim = "xray-slideout";

	public bool activate = false;

	private AppController app;

	// Use this for initialization
	void Start () {
		app = AppController.instance;
		//box_X = 10;
		//box_Y = 40;
		//anim = uiPanel.GetComponent<Animator> ();

		button_X = (width / 2) - (buttonWidth / 2);
	
		//infoWindow = new Rect (box_X, box_Y, width, height);
		
		//allInfos = GameObject.FindObjectsOfType<Information>();

		foreach(UnityEngine.UI.Button but in buttons) {
			but.onClick.RemoveAllListeners ();
		}
		
		collumationToggle.onValueChanged.RemoveAllListeners ();
		
		buttons [0].onClick.AddListener (() => activateAperture());
		
		collumationToggle.onValueChanged.AddListener(delegate{toggleCollumation(!CollumationController.collOn);});
		
		buttons [1].onClick.AddListener (() => cycleCameras (1));
		buttons [2].onClick.AddListener (() => cycleCameras (-1));

		buttons[3].onClick.AddListener(()=>CollumationController.takeScreenshot());

		uiPanel.SetActive (false);
	}
	
	public void displayMenu() {
		showMenu = true;
		showXRayMenu();
	}

	public void hideMenu() {
		showMenu = false;
		uiPanel.SetActive (false);
		app.thirdPersonCamera.enabled = false;
		app.mtl.gameObject.SetActive (true);
	
	}

	//private bool takeHiResShot = false;
	
	/*void OnGUI() {

				if (showMenu) {
								foreach (Information info in allInfos) {
								if(info.name != "X-Ray Room Door") {
										info.gameObject.collider.enabled = true;
										info.showText = false;
								}

					
				}

			float deg = xRayHead.transform.rotation.eulerAngles.z;
			
			if(deg > 180) {
				deg = deg - 360;
			}
			
			float truncated = Mathf.Abs (deg);

			angleText.text = "Angle: " + truncated + " degrees";

		}

	}*/

	void Update() {
		if (showMenu) {
			/*foreach (Information info in allInfos) {
				if(info.name != "X-Ray Room Door") {
					info.gameObject.collider.enabled = true;
					info.showText = false;
				}
				
				
			}*/
			
			float deg = xRayHead.transform.rotation.eulerAngles.z;
			
			if(deg > 180) {
				deg = deg - 360;
			}
			
			float truncated = Mathf.Abs (deg);
			
			angleText.text = "Angle: " + truncated + " degrees";
			
		}
	}

	void activateAperture() {
		UnityEngine.UI.Text text = buttons[0].GetComponentInChildren<UnityEngine.UI.Text>();	

			if (activeCamera.transform.position != aperturePosition.transform.position) {
				activeCamera.transform.position = aperturePosition.transform.position;
				activeCamera.transform.rotation = aperturePosition.transform.rotation;
				buttonStr = "Normal view";
				CollumationController.apertureActive = true;
			} else {
				activeCamera.transform.position = originPosition.transform.position;
				activeCamera.transform.rotation = originPosition.transform.rotation;
				buttonStr = "X-Ray aperture";
				
				activateApertureButton(buttonStr, text);
			}

		text.text = buttonStr;	
		DebugConsole.Log ("Activating: " + text.text);
	}

	void activateApertureButton(string str, UnityEngine.UI.Text text) {
		buttonStr = str;
		text.text = str;	
		CollumationController.apertureActive = false;
	}

	int camIndex = 0;

	void cycleCameras(int dir) {
		UnityEngine.UI.Text text = buttons[0].GetComponentInChildren<UnityEngine.UI.Text>();	
		string buttonStr = "X-Ray aperture";
		activateApertureButton(buttonStr, text);

		camIndex = camIndex + dir;

		if (camIndex < 0) {
			camIndex = alternatePositions.Length - 1;
		}

		if (camIndex > alternatePositions.Length - 1) {
						camIndex = 0;
		}

		activeCamera.transform.position = alternatePositions [camIndex].transform.position;
		activeCamera.transform.rotation = alternatePositions [camIndex].transform.rotation;
	}

	void toggleCollumation(bool coll) {
		DebugConsole.Log ("Collumation clicked" + coll);
		CollumationController	.collOn = coll;
		collumation.SetActive (coll);
	}

	void showXRayMenu() {

		app.mtl.gameObject.SetActive (false);
		uiPanel.SetActive (true);
		activate = !activate;

		AppController.instance.thirdPersonCamera.transform.position = originPosition.transform.position;
		AppController.instance.thirdPersonCamera.transform.rotation = originPosition.transform.rotation;

		app.thirdPersonCamera.enabled = true;
	}

	/*IEnumerator activatePanel(bool active) {
		MouseOverSlide	slide = uiPanel.GetComponent<MouseOverSlide>();
		slide.doPlay();
		yield return new WaitForSeconds(1);
		gameObject.SetActive (active);
	}*/
}

