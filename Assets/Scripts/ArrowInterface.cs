using UnityEngine;
using System.Collections;

public class ArrowInterface : MonoBehaviour {

	public string animState0 = "Base Layer.Arrow Still";
	public string animState1 = "Base Layer.Arrow Up";
	public string animState2 = "Base Layer.Arrow Down";

	public string orientation = "X";

	public Camera rayCamera;
	public Light spotLight;

	public int clickState1;
	public int clickState2;
	public int stopState;

	public float X = 0;
	public float Y = 0;
	public float Z = 0;

	public GameObject inRelationTo;

	public bool setToTransform = false;

	public static string[] animStates = new string[3];
	
	private bool mouseIn = false;

	//private Vector3 camOrigin;
	//private Vector3 myOrigin;
	//private int inCount = 0;

	private Animator anim;

	private int state0; 
	private int state1; //Base Layer.Arrow Up
	private int state2; //Base Layer.Arrow Down

	private bool play1;
	private bool play2;

	//private bool stopped = false;
	private bool rollCameraBack = false;

	private bool activated = false;

	//private Rollover3D relatedRollover;

	private int camDollyBack = 0;

	// Use this for initialization
	void Start () {	
		if (setToTransform) {
			Y = transform.position.y;
			X = transform.position.x;
			Z = transform.position.z;
		}

		rayCamera = rayCamera == null ? Camera.main : rayCamera;
		//camOrigin = rayCamera.transform.position;
		//myOrigin = gameObject.transform.position;
		//relatedRollover = inRelationTo.GetComponent<Rollover3D> ();

		animStates [0] = animState0;
		animStates[1] = animState1;
		animStates[2] = animState2;

		state0 = Animator.StringToHash(animStates[0]);
		state1 = Animator.StringToHash(animStates[1]);
		state2 = Animator.StringToHash(animStates[2]); 

		anim = gameObject.GetComponentInParent<Animator> ();
		anim.StopPlayback ();
	}

	void Update() {
		if (mouseIn) {
						BoxCollider collider = gameObject.GetComponent<BoxCollider> ();

						Vector3 pos = Input.mousePosition;
						//Debug.Log("Mouse pressed " + pos);
						RaycastHit hit;
						Ray ray = rayCamera.ScreenPointToRay (pos);	
						if (collider.Raycast (ray, out hit, 100.0F)) {	
							Debug.Log("Something hit at X:"+hit.point.x+ " Y: "+hit.point.y+" Z: "+hit.point.z);
							//Debug.Log("Transform Position X:"+gameObject.transform.position.x+ " Y: "+gameObject.transform.position.y+" Z: "+gameObject.transform.position.z);
							
							Debug.DrawLine(ray.origin, hit.point);

							 play1 = false;
							 play2 = false;
							
						if(orientation == "Y") {
							
								if (hit.point.y > Y) {
						Debug.Log ("Play 1");
									play1 = true;
									play2 = false;	
								}
			
								if (hit.point.y < Y) {
						Debug.Log ("Play 2");
									play2 = true;
									play1 = false;
								}
							}	
							else if(orientation == "X") {
								

								if (hit.point.x < X) {
						Debug.Log ("Play 1");
									play1 = true;
									play2 = false;	
								}
					
								if (hit.point.x > X) {
						Debug.Log ("Play 2");
									play2 = true;
									play1 = false;
								}
							}
							else if(orientation == "Z") {
								if (hit.point.z > Z) {
									play1 = true;
									play2 = false;
								
								}
					
								if (hit.point.z < Z) {
									play2 = true;
									play1 = false;
									
								}
							}
						
						if(play1) {
							anim.Play (state1);
						}
						if(play2) {
							anim.Play (state2);
						}
						}	
						//inCount++;
				} else {
					anim.Play (state0);
				}

		if (activated) {
			if(camDollyBack < 15) {
				gameObject.transform.Translate(Vector3.forward / (Time.deltaTime * 1000f), Space.World);
				rayCamera.transform.Translate(Vector3.forward / (Time.deltaTime * 1000f), Space.World);
				++camDollyBack;
			} 
		} 

		if(rollCameraBack) {
			if(camDollyBack > 0) {
				gameObject.transform.Translate(Vector3.back / (Time.deltaTime * 1000f), Space.World);
				rayCamera.transform.Translate(Vector3.back / (Time.deltaTime * 1000f), Space.World);
				--camDollyBack;
			} else {
				rollCameraBack = false;
			}
		}

		if (XRayInterface.instance.isMoving()) {
		//Vector3 move = XRayInterface.instance.movement ();
		gameObject.transform.Translate(XRayInterface.instance.movement (), Space.World);
		rayCamera.transform.Translate(XRayInterface.instance.movement (), Space.World);
		spotLight.transform.Translate(XRayInterface.instance.movement (), Space.World);
		X = X + XRayInterface.instance.movement ().x;
		Y = Y + XRayInterface.instance.movement ().y;
		Z = Z + XRayInterface.instance.movement ().z;
		}
	}

	// Update is called once per frame
	void OnMouseEnter () {
		mouseIn = true;
	}

	void OnMouseDown() {

		if (play1 && !activated) {
						XRayInterface.instance.activateState (this.clickState1);
						activated = true;
				} else if (play2 && !activated) {
						XRayInterface.instance.activateState (this.clickState2);
						activated = true;
				} else {
						XRayInterface.instance.activateState (this.stopState);
						activated = false;
						play1 = false;
						play2 = false;
				} 
	}

	void OnMouseExit() {
		mouseIn = false;
		if (!activated) {
						rollCameraBack = true;
						play1 = false;
						play2 = false;
						//activated = false;
		}
	}
}
