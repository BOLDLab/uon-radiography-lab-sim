using UnityEngine;
using System.Collections;
/* THIS CLASS IS NO LONGER USED */
public class XRayInterface : MonoBehaviour {

	public static XRayInterface instance;

	//private bool showMenu = false;
	//private GameObject arm;
	private Transform armTransform;
	private Transform unitTransform;
	//private Transform wallHoseTransform;
	private GameObject swivelArm;
	private GameObject xrayHead;
//	private GameObject wallHoseConnector;
//	private GameObject hingeSeat;
	private GameObject standingPlate;
	private GameObject standingPlateTray;
	private GameObject bedTray;

	private bool armUp = false;
	private bool armDown = false;
	//private bool motorChange = false;

	private bool rotateRight = false;
	private bool rotateLeft = false;

	private GameObject greenButton;

	public bool unitLeft = false;
	public bool unitRight = false;

	private bool plateUp = false;
	private bool plateDown = false;

	private bool standTrayIn = false;
	private bool standTrayOut = false;

	private bool bedTrayIn = false;
	private bool bedTrayOut = false;

	private bool pushArmLeft = false;
	private bool pushArmRight = false;

	private bool pushHeadIn = false;
	private bool pullHeadOut = false;

	private JointMotor armMotor;
	private JointMotor headMotor;

	private string device;

	public GameObject armHingePivot;
	public GameObject slidingHead;

	private Vector3 currentMovement;
	private bool moving = false;

	public bool showExitButton = false;

	public static int ROLL_UNIT_LEFT = 100;
	public static int ROLL_UNIT_RIGHT = 101;
	public static int STOP_UPRIGHT = 111;

	public Camera apertureCamera;
	public Camera returnCamera;
	
	void Awake() {
		instance = this;	
	}

	void Start () {

		Debug.Log ("XRayHead.cs Script loaded", this);

		//greenButton = GameObject.Find ("Green Button");

		armTransform = GameObject.Find ("Arm").transform;
		unitTransform = GameObject.Find ("Rolling Unit").transform;
		swivelArm = GameObject.Find ("Swivel Arm");
		xrayHead = GameObject.Find ("X-Ray Head");
	//	wallHoseConnector = GameObject.Find ("Wall Hose Connector");
		//hingeSeat = GameObject.Find ("Hinge Seat");
		standingPlate = GameObject.Find ("Standing Plate");
		standingPlateTray = GameObject.Find ("Standing Plate Slide Tray");
		bedTray = GameObject.Find ("Bed Plate Tray");

		//wallHoseTransform = wallHoseConnector.transform;

		showExitButton = !AppController.instance.firstPerson;
	}



	// Update is called once per frame
	void Update () {

		//Debug.Log ("Attempting to call x-ray interface: " + Main.instance.menuUp);

		if (armUp && armTransform.localPosition.y < 0.6) {
			//wallHoseConnector.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;

						
			armTransform.Translate (Vector3.up * (Time.deltaTime / 10), Space.World);
			//wallHoseTransform.Translate (Vector3.up * (Time.deltaTime / 10), Space.World);

		} else if (armDown && armTransform.localPosition.y > -0.5) {
			//wallHoseConnector.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;


			armTransform.Translate (Vector3.down * (Time.deltaTime / 10), Space.World);
			//wallHoseTransform.Translate (Vector3.down * (Time.deltaTime / 10), Space.World);

		}

		if (plateUp && standingPlate.transform.localPosition.y < 1.74) {
			standingPlate.transform.Translate (Vector3.up * (Time.deltaTime / 10), Space.World);	
		} else if (plateDown && standingPlate.transform.localPosition.y > 0.48) {
			standingPlate.transform.Translate (Vector3.down * (Time.deltaTime / 10), Space.World);	
		}



		if (standTrayIn && standingPlateTray.transform.localPosition.z > 0.064) {
			standingPlateTray.transform.Translate (Vector3.back * (Time.deltaTime / 10), Space.World);
		} else if (standTrayOut && standingPlateTray.transform.localPosition.z < 0.422) {
			standingPlateTray.transform.Translate (Vector3.forward * (Time.deltaTime / 10), Space.World);	
		}

		if (bedTrayIn && bedTray.transform.localPosition.z > 0.84) {
			bedTray.transform.Translate (Vector3.back * (Time.deltaTime / 10), Space.World);
		} else if (bedTrayOut && bedTray.transform.localPosition.z < 1.04) {
			bedTray.transform.Translate (Vector3.forward * (Time.deltaTime / 10), Space.World);	
		}

		if (unitLeft) {
						armTransform.Translate (Vector3.left * (Time.deltaTime / 10), Space.World);
						unitTransform.Translate (Vector3.left * (Time.deltaTime / 10), Space.World);
						currentMovement = Vector3.left * (Time.deltaTime / 10);
				} else if (unitRight) {
						armTransform.Translate (Vector3.right * (Time.deltaTime / 10), Space.World);
						unitTransform.Translate (Vector3.right * (Time.deltaTime / 10), Space.World);
						currentMovement = Vector3.right * (Time.deltaTime / 10);
				}			 

		if (rotateRight) {
			xrayHead.transform.Rotate(Vector3.back * Time.deltaTime * 10);
		} else if(rotateLeft) {
			xrayHead.transform.Rotate(Vector3.forward * Time.deltaTime * 10);
		}

		if (pushArmLeft) {
			swivelArm.transform.RotateAround (armHingePivot.transform.position, Vector3.down, Time.deltaTime * 5);
		} else if (pushArmRight) {
			swivelArm.transform.RotateAround (armHingePivot.transform.position, Vector3.up, Time.deltaTime * 5);
		}

		if (pushHeadIn && slidingHead.transform.localPosition.z > -0.06572837) {
				slidingHead.transform.Translate (Vector3.back * Time.deltaTime / 5, Space.World);		
		} else if (pullHeadOut && slidingHead.transform.localPosition.z < 0.1120272) {
				slidingHead.transform.Translate (Vector3.forward * Time.deltaTime / 5, Space.World);	
		}
	}

	public void showGUIInterface(string device) {
		this.device = device;
	}

	public void activateState(int state) {
		moving = true;
		if (state == ROLL_UNIT_LEFT) {
			this.unitLeft = true;
			this.unitRight = false;
		} 
		if (state == ROLL_UNIT_RIGHT) {
			this.unitLeft = false;
			this.unitRight = true;
		}
		if (state == STOP_UPRIGHT) {
			this.unitLeft = false;
			this.unitRight = false;
			moving = false;
		}
	}

	public Vector3 movement() {
		return currentMovement;
	}

	public bool isMoving() {
		return moving;
	}

	private void xrayHeadFunc(int id) {
	try {

			if (device == "table") {
				//ExperienceServer.instance.logExperience("Accessed x-ray table", "Accessed x-ray table", "X-Ray Table", "Clicked on the x-ray arm");
				AppController.instance.toggleRollOvers(false);
						GUILayout.TextArea ("X-Ray Table");
						if (GUILayout.Button ("Slide Arm Up")) {
							//ExperienceServer.instance.logExperience("Moved X-Ray Arm Up", "Moved X-Ray Arm Up", "Moved X-Ray Arm Up", "Moved the x-ray arm up");
								armUp = true;
								armDown = false;
								//hingeSeat.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
						}
						if (GUILayout.Button ("Slide Arm Down")) {
							//ExperienceServer.instance.logExperience("Moved X-Ray Arm Down", "Moved X-Ray Arm Down", "Moved X-Ray Arm Down", "Moved the x-ray arm down");
								armDown = true;
								armUp = false;
								//	hingeSeat.rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
						}
						if (GUILayout.Button ("Push Arm Left")) {
							//ExperienceServer.instance.logExperience("Moved X-Ray Arm Left", "Moved X-Ray Arm Left", "Moved X-Ray Arm Left", "Moved the x-ray arm Left");	
								//armMotor.targetVelocity = 5;
								//motorChange = true;
								pushArmLeft = true;
								pushArmRight = false;
						}
						if (GUILayout.Button ("Push Arm Right")) {
							//ExperienceServer.instance.logExperience("Moved X-Ray Arm Right", "Moved X-Ray Arm Right", "Moved X-Ray Arm Right", "Moved the x-ray arm Right");	
								pushArmLeft = false;
								pushArmRight = true;
								//armMotor.targetVelocity = -5;
								//motorChange = true;
								//hingeSeat.rigidbody.constraints = RigidbodyConstraints.None;
						}
						if (GUILayout.Button ("Rotate Head Left")) {
							//ExperienceServer.instance.logExperience("Rotated X-Ray Head Left", "Rotated X-Ray Head Left", "Rotated X-Ray Head Left", "Rotated X-Ray Head Left");	

								//headMotor.targetVelocity = 4;
								//motorChange = true;
								//hingeSeat.rigidbody.constraints = RigidbodyConstraints.FreezeAll;

								rotateLeft = true;
								rotateRight = false;
						}
						if (GUILayout.Button ("Push Head In")) {
							//ExperienceServer.instance.logExperience("Moved X-Ray Arm Right", "Moved X-Ray Arm Right", "Moved X-Ray Arm Right", "Moved the x-ray arm Right");	
							pushHeadIn = true;
							pullHeadOut = false;

						}
						if (GUILayout.Button ("Pull Head Out")) {
				
							pushHeadIn = false;
							pullHeadOut = true;
						}
						if (GUILayout.Button ("Rotate Head Right")) {
							//ExperienceServer.instance.logExperience("Rotated X-Ray Head Right", "Rotated X-Ray Head Right", "Rotated X-Ray Head Right", "Rotated X-Ray Head Right");	

								//headMotor.targetVelocity = -4;
								//motorChange = true;
								//hingeSeat.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
								rotateLeft = false;
								rotateRight = true;
								
						}
						if (GUILayout.Button ("Roll X-Ray Unit Left")) {
							//ExperienceServer.instance.logExperience("Roll X-Ray Unit Left", "Roll X-Ray Unit Left", "Roll X-Ray Unit Left", "Rolled X-Ray Unit Left");	

								unitLeft = true;
								unitRight = false;
						}
						if (GUILayout.Button ("Roll X-Ray Unit Right")) {
							//ExperienceServer.instance.logExperience("Roll X-Ray Unit Right", "Roll X-Ray Unit Right", "Roll X-Ray Unit Right", "Rolled X-Ray Unit Right");

								unitLeft = false;
								unitRight = true;
						}
						if (GUILayout.Button ("Slide Bed Tray In")) {
							//ExperienceServer.instance.logExperience("Slide Bed Tray In", "Slide Bed Tray In", "Slide Bed Tray In", "Slid Bed Tray In");

							bedTrayIn = true;
							bedTrayOut = false;
						}
						if (GUILayout.Button ("Slide Bed Tray Out")) {
							//ExperienceServer.instance.logExperience("Slide Bed Tray Out", "Slide Bed Tray Out", "Slide Bed Tray Out", "Slid Bed Tray Out");

							bedTrayIn = false;
							bedTrayOut = true;
						}
						
						if(AppController.instance.inventory != null  && AppController.instance.inventory.count() > 0) {
							if (GUILayout.Button ("Insert the plate into the bed tray.")) {
							//ExperienceServer.instance.logExperience("Attempted to Insert the plate into the bed tray.", "Attempted to Insert the plate into the bed tray", "Attempted to Insert the plate into the bed tray", "Attempted to Insert the plate into the bed tray");

								if(AppController.instance.insertedPlates[0] != null) {
										if(AppController.instance.insertedPlates[0].gameObject.activeInHierarchy) {
											//ExperienceServer.instance.logExperience("FAIL - the bed tray already contained a plate", "FAIL - the bed tray already contained a plate", "FAIL - the bed tray already contained a plate", "FAIL - the bed tray already contained a plate");
											GUILayout.Label("There's already a plate in the bed tray!");
										} else {
											//ExperienceServer.instance.logExperience("SUCCESS - plate inserted", "SUCCESS - plate inserted", "SUCCESS - plate inserted", "SUCCESS - plate inserted");
											
											AppController.instance.insertedPlates[0].gameObject.SetActive(true);
											AppController.instance.inventory.removeItem(AppController.instance.insertedPlates[0]);
										}
								}
							}
							
						}
				}

		if (device == "stand") {
				AppController.instance.toggleRollOvers(false);
						if (GUILayout.Button ("Standing Plate Up")) {
								plateUp = true;
								plateDown = false;
						}
						if (GUILayout.Button ("Standing Plate Down")) {
								plateUp = false;
								plateDown = true;
						}
						if (GUILayout.Button ("Slide Standing Tray In")) {
								standTrayIn = true;
								standTrayOut = false;
						}
						if (GUILayout.Button ("Slide Standing Tray Out")) {
								standTrayIn = false;
								standTrayOut = true;
						}
						if(AppController.instance.insertedPlates[1].gameObject.activeInHierarchy) {
							if (GUILayout.Button ("Remove the plate from the standing tray.")) {
								AppController.instance.insertedPlates[1].gameObject.SetActive(false);
							}
						} else if(AppController.instance.inventory != null  && AppController.instance.inventory.count() > 0) {
							if (GUILayout.Button ("Insert the plate into the standing tray.")) {
								if(AppController.instance.insertedPlates[1] != null) {
									AppController.instance.insertedPlates[1].gameObject.SetActive(true);
									AppController.instance.inventory.removeItem(AppController.instance.insertedPlates[1]);
								}
							}
						}
			}
			
			if (GUILayout.Button ("Stop")) {
				AppController.instance.toggleRollOvers(false);
				armUp = false;
				armDown = false;
				unitLeft = false;
				unitRight = false;
				plateUp = false;
				plateDown = false;
				standTrayIn = false;
				standTrayOut = false;
				bedTrayIn = false;
				bedTrayOut = false;
				headMotor.targetVelocity = 0;
				armMotor.targetVelocity = 0;
				rotateLeft = false;
				rotateRight = false;
				pushArmLeft = false;
				pushArmRight = false;
				pushHeadIn = false;
				pullHeadOut = false;
		}
		
		if(device != "") {
				Debug.Log ("Checking camera settings");
			if (GUILayout.Button ("Exit to Room")) {
					Debug.Log ("Exit clicked firsperson = "+AppController.instance.firstPerson);
				if(AppController.instance.firstPerson) {
					AppController.instance.setCamera(AppController.MAIN_CAMERA, AppController.X_RAY_ROOM_WINDOW, true, false);
						showExitButton = false;
				} else {
					AppController.instance.setCamera(AppController.X_RAY_ROOM_WINDOW, AppController.MAIN_CAMERA, true, false);
						showExitButton = true;
						AppController.instance.toggleRollOvers(true);
				}
				device = "";
			}
		}

		}	catch(System.SystemException exc) {
				Debug.Log ("Caught exception: "+exc);
		}
	}

}
