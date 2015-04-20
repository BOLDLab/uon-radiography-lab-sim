using UnityEngine;
using System.Collections;

public class TouchRotate : MonoBehaviour {
	Vector3 moveDirection;
	//public bool activated = false;
	public bool doRotate = false;
	public bool doMove = false;

//	public Camera myCam;

	public int timeSinceClick = 0;

	public int clickCount = 0;

	private InventoryItem invItemScript;
	private MoveCamera moveCamScript;

	private bool mouseOver = false;

	private Rect messageRect;

	private Vector3 lastV3;
	private Vector3 screenV3;
	
	private Vector3 targetFlyRotation;
	
	private bool rotating = false;
	public bool rotateViaUI = false;

	public float fieldOfView = 65.0f;

	public float rayCastDistance = 1.0f;

	private AppController app;

	private GameObject myItemGameObject;
	// Use this for initialization
	void Start () {
		app = AppController.instance;
		invItemScript = gameObject.GetComponent<InventoryItem> ();
		moveCamScript = Camera.main.gameObject.GetComponent<MoveCamera> ();
		//moveDirection = rigidbody.transform.rotation.eulerAngles;
		messageRect = new Rect (Screen.width / 2 + 300, Screen.height / 2 - 20, 200, 80);
		targetFlyRotation = rigidbody.transform.rotation.eulerAngles;
	}

	void initComponent() {
		if (GetComponent<InventoryItem> () != null) {
				if (GetComponent<InventoryItem> ().isOneOf > 0) {
						myItemGameObject = transform.parent.gameObject;
				} else {
						myItemGameObject = gameObject;		
				}
		} else {
				myItemGameObject = gameObject;
		}
	}

	void OnMouseEnter() {
		if (!enabled || app == null)
						return;

		if (myItemGameObject == null) {
			initComponent();	
		}

		if (!app.inLineOfSight (Camera.main.gameObject, myItemGameObject, rayCastDistance)) 
						return;

		if (enabled) {
						mouseOver = true;
		}
	}

	void OnMouseExit() {
	

		if (enabled && clickCount == 0) {
			mouseOver = false;
			doMove = false;
			moveCamScript.enabled = true;
		}
		//rigidbody.isKinematic = false;
	}

	void OnMouseDown() {

		if (enabled) {
			if(invItemScript != null) invItemScript.enabled = false;
			moveCamScript.enabled = false;
			app.mtl.enabled = false;
		}
	}

	void OnMouseUp() {
	
		if (enabled) {
			doRotate = true;
			doMove = false;
			timeSinceClick = 0;
			clickCount = 0;
			//rigidbody.isKinematic = false;		
		}
	}

	void OnMouseDrag() {

		if (enabled && !doRotate) {
						doMove = true;
						
		}
	}

	void OnGUI() {
		if (mouseOver && clickCount == 0 && !rotateViaUI) {
						AppController.instance.setInfoUIText("Click/Touch to Rotate "+gameObject.name);
		}

				
		if(doRotate || rotateViaUI) {
			rotating = false;
			AppController.instance.usingTouchRotate = true;			
		
			//GUI.SetNextControlName("Up");
			if(GUI.RepeatButton (new Rect (messageRect.x, messageRect.y - 80, 80, 80), AppController.instance.upArrow)) {
				targetFlyRotation = transform.up;
				rotating = true;
			}

			//GUI.SetNextControlName("Down");
			if(GUI.RepeatButton (new Rect (messageRect.x, messageRect.y + 80, 80, 80), AppController.instance.downArrow)) {
				targetFlyRotation = transform.up * -1;
				rotating = true;
			}

			//GUI.SetNextControlName("Left");
			if(GUI.RepeatButton (new Rect (messageRect.x - 80, messageRect.y, 80, 80), AppController.instance.leftArrow)) {
				targetFlyRotation = transform.right * -1;
				rotating = true;
			}

			//GUI.SetNextControlName("Right");
			if(GUI.RepeatButton (new Rect (messageRect.x + 80, messageRect.y, 80, 80), AppController.instance.rightArrow)) {
				targetFlyRotation = transform.right;
				rotating = true;
			}

			if(GUI.Button (new Rect (messageRect.x, messageRect.y, 80, 80), "Place")) {
				rotating = false;
				doRotate = false;
				rotateViaUI = false;
				gameObject.rigidbody.isKinematic = false;

				if(rigidbody.IsSleeping()){
					rigidbody.WakeUp();
				}

				if(invItemScript != null)
					invItemScript.enabled = true;

				if(gameObject.transform.parent != null) {
					InventoryItem invItem = gameObject.transform.parent.gameObject.GetComponent<InventoryItem>();
					if(invItem != null) {
						this.enabled = false;
					}
				}
				
				AppController.instance.usingTouchRotate = false;
				app.mtl.enabled = true;
			}
		}


	/*	if (doMove) {
			GUI.Label (messageRect, "Move mode - Release to rotate." , AppController.instance.generalStyle);
		}*/	

		/*if (doRotate) {
			GUI.Label (messageRect, "Rotation mode - Click/Touch anywhere to Place.", AppController.instance.generalStyle);
		}
		Debug.Log ("Click count: " + clickCount);*/
	}


	// Update is called once per frame
	void Update () {

		/*if (doMove) {
			rigidbody.isKinematic = true;
						Vector3 v3 = Input.mousePosition;
						v3.z = 1.0f;
						screenV3 = v3;

						v3 = Vector3.Lerp (gameObject.transform.position, Camera.main.ScreenToWorldPoint (v3), Time.deltaTime * 2.0f);
		
			AppController.instance.smoothLook(v3);
		
						transform.position = v3;
						lastV3 = v3;
			AppController.instance.usingTouchRotate = true;
		}*/

		if(rotating) {
			targetFlyRotation.Normalize ();
			moveDirection += targetFlyRotation;

			gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime);

			AppController.instance.usingTouchRotate = true;
		}

		if (doRotate || rotateViaUI) {
			Camera.main.transform.LookAt(gameObject.transform);
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fieldOfView, Time.deltaTime*5.0f);
		}
	}
}
