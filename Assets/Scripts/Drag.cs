using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Drag: MonoBehaviour {
	private float dist;
	private Vector3 v3Offset;
	private Plane plane;

	public string id = "";
	public float sensitivity = 10;
	private Camera dragCamera;

	public GameObject startEmpty;
	public GameObject returnEmpty;
	public GameObject alternateCameraEmpty;

	public bool disableCameraOnMouseRelease = false;
	//public Camera returnCamera;
	public bool dragOnXAxis = true;
	public float XbottomLimit = 0;
	public float XtopLimit = 0;
	public bool dragOnYAxis = false;
	public float YbottomLimit = 0;
	public float YtopLimit = 0;
	public bool dragOnZAxis = false;
	public float ZbottomLimit = 0;
	public float ZtopLimit = 0;
	public bool invertMouse = true;
	private MoveCamera moveCamera;

	// public RenderTexture renderTexture; pay $75 a month, wtf???
	public XRayMachineMenu menu;
	public string mouseOverMessage = "";
	public bool applyAsRotation = false;
	public float rotateCenter = 0;
	public bool applyRotationToX = false;
	public bool applyRotationToY = false;
	public bool applyRotationToZ = false;

	public GameObject hingePivot;

	public ButtonHover button1;
	
	public bool lockGuageOnRotate = false; 

	private Vector3 camOrigin;
	private Quaternion camOriginRotation;

	bool dragging = false;

	void Start() {
		enabled = false;
		dragCamera = AppController.instance.thirdPersonCamera;
		moveCamera = dragCamera.GetComponent<MoveCamera> ();


		/*if (gameObject.GetComponent<Rollover3D> () == null) {
							collider.enabled = false;
		}*/

		if (Application.platform == RuntimePlatform.IPhonePlayer) {
		//	dragCamera.fieldOfView = 28;
		} else {
			dragCamera.fieldOfView = 60;		
		}
	}

	void Update() {
		//Debug.Log ("Mouse pos: " + Input.mousePosition);
	}

	/*void OnGUI() {
				if (alternateCameraEmpty != null) {
						if (GUILayout.Button ("Change Perspective")) {
								changePerspective();
						}		 
				}
	}*/

	void changePerspective() {
			if(dragCamera.transform.position != alternateCameraEmpty.transform.position) {
				dragCamera.transform.position = alternateCameraEmpty.transform.position;
				dragCamera.transform.rotation = alternateCameraEmpty.transform.rotation;
			} else {
				dragCamera.transform.position = startEmpty.transform.position;
				dragCamera.transform.rotation = startEmpty.transform.rotation;
			}
		AppController.instance.currentLocation = dragCamera.gameObject;
	}

	void OnMouseEnter() {
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (null != menu && mouseOverMessage != null && mouseOverMessage.Length > 0) {
				AppController.instance.setInfoUIText(mouseOverMessage);
		}
	}

	void OnMouseDown() {
		if (EventSystem.current.IsPointerOverGameObject())
						return;

		AppController.instance.setGuageLock (lockGuageOnRotate);

		//if(null != moveCamera)
			//moveCamera.enabled = false;
		DebugConsole.Log ("Show menu script is on:" + name);
		if (null != menu && !menu.uiPanel.activeInHierarchy) {
				menu.displayMenu();
		}

		if (!dragging) {
						Vector3 mousePos = invertMouse ? Input.mousePosition * -1 : Input.mousePosition;
						//Vector3 mousePos = transform.position;
						//Main.instance.setGuageLock(false);
						mousePos = mousePos * sensitivity;//Time.smoothDeltaTime;
						plane.SetNormalAndPosition (dragCamera.transform.forward, transform.position);
						Ray ray = dragCamera.ScreenPointToRay (mousePos);
						float dist;
						plane.Raycast (ray, out dist);
						v3Offset = transform.position - ray.GetPoint (dist);  
						
		}
	}

	void OnMouseUp() {
		button1.resetCamPostionOnDragRelease ();

		dragging = false;
		if (disableCameraOnMouseRelease) {
						dragCamera.enabled = false;
		}
		this.enabled = false;

		this.collider.enabled = false;
		
		AppController.instance.setGuageLock (false);
		/*if (alternateCameraEmpty != null && dragCamera.transform.position == alternateCameraEmpty.transform.position) {
			changePerspective();
		}*/

		if(null != moveCamera)
			moveCamera.enabled = true;
	}

	void OnMouseDrag() {
		if (!enabled)
						return;

		dragging = true; 

		Vector3 mousePos = invertMouse ? Input.mousePosition * -1 : Input.mousePosition;
	
		//Debug.Log ("New y: "+newY+" New x: "+newX+" New z:"+ne
		if (applyAsRotation) {
			//float center = XtopLimit - XbottomLimit;
			float myDir = 0;


			Vector3 direction;

			if (dragOnYAxis) {
				if (mousePos.y > rotateCenter) {
					myDir = -1;
				} else {
					myDir = 1;
				}
			}
			if (dragOnXAxis) {

				if (mousePos.x > rotateCenter) {
					myDir = -1;
				} else {
					myDir = 1;
				}
			}
			if (dragOnZAxis) {
				if (mousePos.z > rotateCenter) {
					myDir = -1;
				} else {
					myDir = 1;
				}
			}



		//	DebugConsole.Log("Pos: "+mousePos);
			direction = new Vector3(applyRotationToX?myDir:0, applyRotationToY?myDir:0, applyRotationToZ?myDir:0);
			//Debug.Log ("Direction: "+direction);
			transform.RotateAround (hingePivot.transform.position, direction, Time.deltaTime * sensitivity);

		} else {
			Ray ray = dragCamera.ScreenPointToRay (mousePos * sensitivity);
			float dist;
			plane.Raycast (ray, out dist);
			Vector3 v3Pos = ray.GetPoint (dist);
			
			float newY = v3Pos.y + v3Offset.y;
			float newX = v3Pos.x + v3Offset.x;
			float newZ = v3Pos.z + v3Offset.z;
			
			newX = v3Pos.x + v3Offset.x;
			newY = v3Pos.y + v3Offset.y;
			newZ = v3Pos.z + v3Offset.z;
			
			Vector3 newV = transform.position;
			//Vector3 newRCV = dragCamera.transform.position;

				if (dragOnXAxis) {
						if (newX > XbottomLimit && newX < XtopLimit) {
								newV.x = newX;
						}
				} 
				if (dragOnYAxis) {
						if (newY > YbottomLimit && newY < YtopLimit) {
								newV.y = newY;
						}
				} 
				if (dragOnZAxis) {
						if (newZ > ZbottomLimit && newZ < ZtopLimit) {
								newV.z = newZ;
						}
				}  
			DebugConsole.Log ("New V: " + newV);
			transform.position = newV; 
		}
	}

	/*public void activateButton(int reference) {
		if (reference > 0) {
			activateButton1 = false;
			activateButton2 = false;

			if(reference == 1) {
				activateButton1 = true;
			}

			if(reference == 2) {
				activateButton2 = true;
			}

		}

	}*/
}
