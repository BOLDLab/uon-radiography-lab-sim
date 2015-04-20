using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MoveToLocation : MonoBehaviour {

	NavMeshAgent agent;
	public GameObject mousePointer;

	Transform mousePointerBase;
	Transform LookaMe;

	//List<Component> interesting = new List<Component>();

	public float defaultFoV = 60;
	private Vector3 previousPosition;
	
	public float curSpeed;

//	private int frames = 0;
	public Vector3 bounceBackPosition;
	public bool hitBarrier = false;
	public bool bouncingBack = false;

	public GameObject eyeHeightPointer;

	private Transform LookAMe;

	private AppController app;

	public NavMeshAgent getAgent() {
		return agent;
	}

	public void setLookAMe(Transform look) {
		LookaMe = look;
	}

	// Use this for initialization
	void Start () {
		app = AppController.instance;
		agent = GetComponent<NavMeshAgent>();
		LookaMe = eyeHeightPointer.transform;

		mousePointerBase = mousePointer.transform;
		mousePointer.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		//if(app.mouseOnUI) DebugConsole.Log ("MouseOnUI "+app.mouseOnUI);

		if (EventSystem.current.IsPointerOverGameObject()) {

			agent.Stop();
			//agent.destination = transform.position;
			return;
		}
				if (/*app.focusedOnItem || */
						app.lerpFoVRunning /*|| app.invPointer.activeInHierarchy*/) {
						agent.Stop ();	
						return;
				}

				if (!hitBarrier) {
						bounceBackPosition = agent.transform.position;
				}

				if (Input.GetKey (KeyCode.LeftControl)) {
						return;
				}


		if (!app.getClickedMarker ()) {
						if (Input.GetMouseButtonDown (0) && /*!exitedThirdP && */
								!app.usingTouchRotate && 
								!app.mouseOnOpenTrigger) {
								
								app.closeInfoPanel ();
								if (app.lastLookedInfo != null && app.thirdPersonCamera.enabled) {			
			
										app.toggleRenderers (Camera.main.gameObject.transform.parent.gameObject, true);
										app.thirdPersonCamera.enabled = false;
										
								} else {
									
										disableIcons ();
										
										LookaMe = eyeHeightPointer.transform;

										RaycastHit hit;

										if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 5.0f)) {
												agent.destination = hit.point;
										}
					
										if (!isRunning) { 
												app.pointer.SetActive (false);
												displayMousePointer (hit.point);
												isRunning = true;
										}
								}				
						}
		}
			
		if (Vector3.Distance (mousePointer.transform.position, transform.position) < 2.0f) {
			disableIcons();
		}

		if (LookaMe != null) {
						if (Vector3.Distance (transform.position, LookaMe.position) > 2.5f || app.focusedOnItem) {
								app.smoothLook (LookaMe.position);
						}
				
						Information info = LookaMe.gameObject.GetComponent<Information> ();
						
						if (info != null) {
							app.lastLookedInfo = info;
							app.lastLookedInfo.checkLockState ();
						}
		}
		

	}

	public void disableIcons() {
		mousePointer.SetActive (false);
		isRunning = false;
	}

	bool isRunning = false;
	//Camera tempCam = null;
	
	void displayMousePointer(Vector3 v3) {
		mousePointer.SetActive (true);
		mousePointer.transform.position = new Vector3(v3.x, mousePointerBase.position.y, v3.z);
	}
}
