using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Information : MonoBehaviour {

	//public int _id = 0;
	public Texture texture;	
	private GUIStyle style;

	public bool showText = false;
	public string text;
	public string buttonText = "Go Here";
	public string button2Text = "Move Closer";
	public int displayTextTime = 800;

	public float box_X = 600;
	public float box_Y = 300;
	public float width = 500;
	public float height = 180;

	//private bool useButton = true;
	public float button_X = 750;
	public float button_Y = 580;
	public float buttonWidth = 213;
	public float buttonHeight = 127;

	public float boxHeightOffset = -200;
	public float buttonHeightOffset = 135;

	public bool moveCam = true;
	public Camera cameraToMove;
	
	public GameObject firstVisitCameraPosition;
	
	public int fieldOfView = 60;
	private int returnFieldOfView;

	public bool jumpToCam = false;
	public Camera cameraToJumpTo;
	public Camera cameraToReturnTo;
	
	//public bool overrideButtonActivate = false;

	private int timeDisplayed = 0;

	public bool disableColliderAtLocation = true;

	private DoorTrigger doorTrigger;

	private Rect infoWindow;
	Information[] allInfos;

	public GameObject XRayMenuObject;
	//private XRayConsole console;

	private XRayMachineMenu xRayMenu;

	public bool useConsole = false;

	public float rayCastDistance = 1.5f;
	public bool noRaycast = false;
	//public string alternateCollider = null;

	public bool doorTriggerOverride = false;

	public bool firstVisit = true; 
	//public ActivateAtLocation panelActivator;

	private AppController app;

	public bool rightClick = false;

	public Rect getInfoRect() {
		return infoWindow;
	}

	void Start() {

		app = AppController.instance;

		if (moveCam == jumpToCam) {
			DebugConsole.LogError("You can't jump to and move a cam at the same time.");
		}

		box_X = (Screen.width / 2) - (width / 2);
		box_Y = (Screen.height / 2) - (height / 2);//+ boxHeightOffset;

		button_X = (width / 2) - (buttonWidth / 2);
		//button_Y = box_Y - buttonHeight - 5;

		infoWindow = new Rect (box_X, box_Y, width, height);

		allInfos = GameObject.FindObjectsOfType<Information>();

		doorTrigger = gameObject.GetComponent<DoorTrigger> ();

		if(null != XRayMenuObject)
			xRayMenu = XRayMenuObject.GetComponent<XRayMachineMenu> ();
	}

	// not implemented
	public void showInfo() {

		bool inLoS = (!noRaycast && app.inLineOfSight (Camera.main.gameObject, gameObject, rayCastDistance));

		if (app.thirdPersonCamera.enabled || EventSystem.current.IsPointerOverGameObject() || firstVisit || !inLoS)
						return;

		if (app.getConsole () != null && app.getConsole().show) {
			return;
		}

		if (doorTriggerOverride)
				return;

		if (doorTrigger == null) {
				updateInfoPanel ();
		}

		showConsoleButtonUI (useConsole);
	}

	void OnMouseEnter() {
		if (useConsole) {
						showConsoleButtonUI (useConsole);
				}

		if (null == doorTrigger)
						return;

		if(!doorTrigger.door.isOpen && !app.movingObject && !EventSystem.current.IsPointerOverGameObject()) {
					triggerMouseOver ();
		} 
	}

	void OnMouseDown() {
		OnMouseEnter ();
		app.setCameraLook (transform);
	}

	public void triggerMouseOver() {
		updateInfoPanel ();

		showConsoleButtonUI (useConsole);

		if(doorTrigger != null)
			doorTrigger.checkLockState ();
	}

	void Update() {
		if (app.thirdPersonCamera.enabled)
						return;

		if (Input.GetMouseButton (1)) {
			rightClick = true;
		}
	}

	void showConsole() {
		if (app.getConsole () != null) {
			app.getConsole ().showPanel (true);
		}
	}

	void showConsoleButtonUI(bool show = true) {
		if (app == null)
			return;

		app.infoButton1.SetActive (true);

		UnityEngine.UI.Button button1 = app.infoButton1.GetComponentInChildren<UnityEngine.UI.Button> ();	
		UnityEngine.UI.Text text1 = button1.GetComponentInChildren<UnityEngine.UI.Text> ();		
		
		UnityEngine.UI.Text infoText = app.informationUIPane.GetComponentInChildren<UnityEngine.UI.Text> ();

		if (show) {				
						text1.text = buttonText;
						infoText.text = text;

						button1.onClick.AddListener (() => showConsole ());
		} else {
						text1.text = "";
						infoText.text = "";
						button1.onClick.RemoveAllListeners();
		}

		app.infoButton1.SetActive (show);
	}

	public void updateInfoPanel() {
		if (app.getConsole () != null && app.getConsole ().show) {
			return;
		}
		collider.enabled = !disableColliderAtLocation;

		app.setInfoUIText(text);
	}

	public void checkLockState() {
		if (doorTrigger != null) {
						doorTrigger.checkLockState ();
		}
	}
}
