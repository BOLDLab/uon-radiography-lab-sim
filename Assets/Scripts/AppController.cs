using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AppController : MonoBehaviour 
{
	public static AppController instance;

	public string baseURL = "http://bold.newcastle.edu.au/xray_data/";
	public string screenShotURI = "index.php";
	public string cassetteSaveURI = "cassname.php";
	public string cassetteListURI = "view_cass.php";
	public string cassetteCountURI = "count_cass.php";
	public string screenShotListURI = "view_shots.php";
	public string shotCountURI = "count_shots.php";
	public string deleteCassURI = "del_cass.php";
	public string deleteScreenShotURI = "del_shots.php";
	public GUISkin guiSkin;

	public GUIStyle generalStyle;
	public GUIStyle buttonStyle;

	public Texture2D background, LOGO;
	public bool DragWindow = false;
	public string levelToLoadWhenClickedPlay = "";
	public string[] AboutTextLines = new string[0];
	public Camera[] cameras;

	public AudioSource audioPickUp;
	public AudioSource audioNoPickUp;
	public AudioSource audioPutDown;

	private int camIndex = 0;
	
	private float startTime;
	
	private string clicked = "mainMenu", MessageDisplayOnAbout = "About \n ";

	private Rect WindowRect;
	//private Rect WindowInv;
	private Rect WindowScore;
	//private float volume = 1.0f;

	public bool menuUp = false;
	public bool firstPerson = false;
	public bool zoomed = false;
	public GameObject fpController;

	//private Camera fpCamera;	
	//private GameObject xRayArm;
	private Transform guageTransform;
	private Quaternion guageOrigin;

	public static short MAIN_CAMERA = 0;
	public static short STANDING_PLATE_HOLDER = 1;
	public static short X_RAY_TABLE = 2;
	public static short X_RAY_ROOM_LEFT_ENTRANCE = 3;
	public static short SINK = 4;
	public static short PLATE_SHELF = 5;
	public static short X_RAY_ROOM_WINDOW = 6;
	
	public Camera startCamera;
	public short startCameraSlot = MAIN_CAMERA;
	public Camera standingCassetteHolder;
	public short standingCassetteSlot = STANDING_PLATE_HOLDER;
	public Camera xRayTable;
	public short xRayTableSlot = X_RAY_TABLE;
	public Camera xRayLeftEntrance;
	public short xRayLeftEntranceSlot = X_RAY_ROOM_LEFT_ENTRANCE;
	
	public InventoryItem[] plates;
	public InventoryItem[] insertedPlates;

	public Texture plateTexture; 

	public GameObject guage;

	public InventoryItem[] overrideInventory;
	public bool toggleOverrideInventory = false;
	
	public Inventory inventory;
	public GameObject inventoryUIPane;
	public GameObject inventoryItemButtonBase;
	public GameObject informationUIPane;
	public GameObject infoButton1;
	public GameObject infoButton2;
	public GameObject infoButton3;
	public GameObject actionButton1;
	public GameObject actionButton2;

	public UnityEngine.UI.Text informationUIText;
	public UnityEngine.UI.Text scoreUIText;

	private bool lockGuage = false;

	private float invXStart = 3;
	private float invYStart = 15;

	public static float startScreenWidth;

	public GameObject currentLocation;
	public int score = 0;
	
	public Information lastLookedInfo = null;

	public Texture upArrow;
	public Texture downArrow;
	public Texture leftArrow;
	public Texture rightArrow;

	public bool NoGUI = false;
	
	//public bool mouseOnUI = false;
	public bool focusedOnItem = false;
	public bool usingTouchRotate = false;
	public bool mouseOnOpenTrigger = false;

	public string screenName;
	public string userTitle;
	public string email;
	public string hash = "LocalTesting123";
	
	public LayerMask LoSBlockLayerMask;
	public bool inMotion = false;

	public int inventoryUIclicks = 0;
	public int focusID = -1;
	public int lastFocusPriority = 1;

	public bool lerpFoVRunning = false;

	public GameObject controlPanel;
	private XRayConsole console;

	public Camera thirdPersonCamera;

	public GameObject pointer;
	public GameObject[] pointerItems;

	public GameObject invPointer;
	public GameObject[] invPointerItems;

	public GameObject lastPointer;  // @todo assign getter/setter

	public MoveToLocation mtl;

	public float lerpFoVSpeed = 0.5f;
	public float lookSpeed = 7.0f;

	private bool clickedMarker = false;
	public bool movingObject = false;

	public GameObject cassetteInputUI;
	public GameObject helpScreenUI;

	public UnityEngine.UI.Text UIProgressText;

	public GameObject statsPanel;
	public GameObject cassettesPanel;
	public GameObject screenShotsPanel;
	public GameObject cassToggle;
	public GameObject shotsToggle;
	public GameObject nameField;

	public GameObject deleteBtn;

	public IDictionary<int,double[]> exposureSettings = new Dictionary<int, double[]>();

	void Awake() {
		instance = this;	
		//this.activateColliders (true);
	}

	public XRayConsole getConsole() {
		return console;
	}

	public void setExposure(int key, double[] vals) {
		exposureSettings [key] = vals;
	}

	public GameObject insertedReaderCassette = null;

	public GameObject getCassetteFromInv() {
		return AppController.instance.inventory.getFirstObjectOfType ("cassette");
	}
	
	private void Start()
	{	
		screenShotURI = baseURL + screenShotURI;
		cassetteSaveURI = baseURL + cassetteSaveURI;
		cassetteListURI = baseURL + cassetteListURI;
		cassetteCountURI = baseURL + cassetteCountURI;
		screenShotListURI = baseURL + screenShotListURI;
		shotCountURI = baseURL + shotCountURI;
		deleteCassURI = baseURL + deleteCassURI;
		deleteScreenShotURI = baseURL + deleteScreenShotURI;

		//DebugConsole.Log ("Starting application...");
		startScreenWidth = Screen.width;
		console = controlPanel.GetComponent<XRayConsole> ();
		inventory = new Inventory ();
				
		invPointer.SetActive (false);
		pointer.SetActive (false);

		cassetteInputUI.SetActive (false);
		statsPanel.SetActive (false);

		cassToggle.SetActive(false);
		shotsToggle.SetActive (false);

		WindowRect = new Rect((Screen.width / 2)-250, Screen.height / 2, 400, 200);
		//WindowInv = new Rect(Screen.width-220, 20, 204, 205);
		WindowScore = new Rect (20, 20, 200, 60);

		Camera[] cams = GameObject.FindObjectsOfType<Camera> ();
		foreach (Camera cam in cams) {
			cam.enabled = false;
		}

		cameras = new Camera[8];
		cameras [0] = startCamera;
		cameras [1] = standingCassetteHolder;
		cameras [2] = xRayTable;
		cameras [3] = xRayLeftEntrance;

		startCamera.enabled = true;

		// X-Ray angle guage
		guageTransform = guage.transform;

		// set origin to starting position
		guageOrigin = new Quaternion(guageTransform.rotation.x, guageTransform.rotation.y, guageTransform.rotation.z, guageTransform.rotation.w);

		for (int x = 0; x < AboutTextLines.Length;x++ )
		{
			MessageDisplayOnAbout += AboutTextLines[x] + " \n ";
		}
		MessageDisplayOnAbout += "Press Esc To Go Back";
		toggleCamTriggers (false);

		insertedPlates = new InventoryItem[2];

		DebugConsole.Log ("Finished init...");

		inventoryItemButtonBase.SetActive (false);

		infoButton1.SetActive (false);
		infoButton2.SetActive (false);
		infoButton3.SetActive (false);
		actionButton1.SetActive (false);
		actionButton2.SetActive (false);

		UnityEngine.UI.Button abtn = deleteBtn.GetComponent<UnityEngine.UI.Button> ();
		abtn.interactable = false;

		foreach (GameObject pt in pointerItems) {
			pt.AddComponent<ArrowPointer>();
		}

		foreach (GameObject ipt in invPointerItems) {
			ipt.AddComponent<InventoryPointer>();
		}

		refreshInventoryPanel ();
	}

	public void setCameraLook(Transform look) {
		mtl.setLookAMe(look);
	}

	public Camera getCameraInstance(int camera) {
		return cameras[camera];
	}

	public void toggleCamTriggers(bool enable) {
//		Debug.Log ("Toggled cam triggers to " + enable);
		CamTrigger[] triggers = GameObject.FindObjectsOfType<CamTrigger> ();

		foreach (CamTrigger trigger in triggers) {
			trigger.enabled = enable;
		}
	}

	public void toggleRollOvers(bool enable) {
	//	Debug.Log ("Toggled rollovers to " + enable);
		Rollover3D[] rollovers = GameObject.FindObjectsOfType<Rollover3D> ();
		
		foreach (Rollover3D rollover in rollovers) {
			rollover.enabled = enable;
		}
	}

	public void activateColliders(string tag, bool toggle) {
		Collider[] colliders = GameObject.FindObjectsOfType<Collider> ();

		foreach (Collider collider in colliders) {
			if(collider.gameObject.tag.Equals(tag)) {
				collider.enabled = toggle;
			}
		}
	}

	public void dropItem(InventoryItem item) {

			inventory.removeItem(item);
			
			item.gameObject.SetActive (true);
			//DebugConsole.Log ("removed item "+item.displayName+", now " + inventory.Count);

			inventoryUIclicks = 0;
			//if(item.putDownPointTransform == null) {
			item.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z+1));
			item.rigidbody.isKinematic = true;	

			MouseMoveMe mme = item.gameObject.GetComponent<MouseMoveMe> ();	
			
			if (mme == null) {
					item.gameObject.AddComponent<MouseMoveMe> ();
					mme = item.gameObject.GetComponent<MouseMoveMe> ();
			}

			mme.setDropAudio (audioPutDown);	
			mme.setDoMove (true);
			
			item.hideObjectFromUser(false);
			
			//item.rigidbody.isKinematic = false;
			
			item.showText = false;

			item.uiButton = null;
			
			movingObject = true;
			invPointer.SetActive (false); // disable app wide inv pointer
			item.toggleMyPointer (false); // disable script on this object

			item.enabled = false;
			item.gameObject.collider.enabled = true;

			focusID = item.GetInstanceID ();
			usingTouchRotate = false;
			refreshInventoryPanel ();
	}

	/* Inventory GUI */
	public void refreshInventoryPanel() {

		InventoryItem[] invList = null;

		if(inventory.Count > 0) {
			invList = inventory.getInventoryList();
		}	

		UnityEngine.UI.Button[] invUIButtons = inventoryUIPane.GetComponentsInChildren<UnityEngine.UI.Button> ();

		//Clearing inventory panel
		foreach (UnityEngine.UI.Button iButton in invUIButtons) {
			if(!Object.Equals(iButton.gameObject, inventoryItemButtonBase)) {
				Destroy(iButton.gameObject);
			}
		}

		GameObject buttonObj = null;
		UnityEngine.UI.Button button = null;

		// rebuilding inventory
		if (null != invList) {
					foreach (InventoryItem item in invList) {
							buttonObj = (GameObject)Instantiate (inventoryItemButtonBase);				
							button = buttonObj.GetComponent<UnityEngine.UI.Button>();
							item.uiButton = button;

							button.transform.SetParent(inventoryUIPane.transform);

							string rolltext = item.rolloverText;
							Sprite sprite = item.sprite;
						
							if(item.isOneOf > 0) {
								rolltext = rolltext + " X " + inventory.smallItemCount(item); 
								
								if(item.groupSprite != null) sprite = item.groupSprite;
							}
							
							if(item.isOneOf == 0 || (item.isOneOf > 0 && item.nthOf == 0)) {
								UnityEngine.UI.Image img = button.GetComponent<UnityEngine.UI.Image>();
								img.sprite = sprite;

								UnityEngine.UI.Text uiText = button.GetComponentInChildren<UnityEngine.UI.Text>();
								if(uiText != null) 	{
									uiText.text = item.displayName;
								}
			
								InventoryItem buttonItem = item;
								button.onClick.AddListener(() => dropItem(buttonItem));
								buttonObj.SetActive(true);
						  }
				}
		}

		MouseOverSlide mos = inventoryUIPane.GetComponentInParent<MouseOverSlide> ();	
		
		if (mos != null && !mos.isDown) {
			mos.doPlay();
		}
	}


	void scoreFunc(int id) {

	}

	private int displayScore = 0;

	private void OnGUI()
	{
		if (NoGUI)
						return;

		if (background != null)
			GUI.DrawTexture(new Rect(0,0,Screen.width , Screen.height),background);
		if (LOGO != null && clicked != "about")
			GUI.DrawTexture(new Rect((Screen.width / 2), 200, 200, 200), LOGO);

		GUI.skin = guiSkin;

		if (score != displayScore) {
			if(displayScore < score)
				displayScore++;
			else {
				displayScore--;
			}
		}

		setScoreUIText ("Score: " + displayScore);
		//GUI.Window (12, WindowScore, scoreFunc, "Score: "+displayScore, generalStyle);

		/*if (null != fpController) {
						//	MouseLook mouseLook = fpController.GetComponent<MouseLook> ();
						//MouseLook camMouseLook = fpCamera.GetComponent<MouseLook> ();
		}*/

		/*if (inventory.Count > 0) {
			WindowInv = GUI.Window (8, WindowInv, invFunc, "Items", AppController.instance.generalStyle);
		}*/

		if (menuUp) {
						
						//mouseLook.enabled = false;
						//camMouseLook.enabled = false;

						if (clicked == "mainMenu") {
							//WindowRect = GUI.Window (0, WindowRect, menuFunc, "Main Menu");
							//if(MessageScript.instance.screen_name != "") {
									//WindowRect = GUI.Window (0, WindowRect, menuFunc, "Main Menu");
							//}
						} else if (clicked == "mode") {
								WindowRect = GUI.Window (1, WindowRect, modeFunc, "Mode");
						} else if (clicked == "resolution") {
								GUILayout.BeginVertical ();
								for (int x = 0; x < Screen.resolutions.Length; x++) {
										if (GUILayout.Button (Screen.resolutions [x].width + "X" + Screen.resolutions [x].height)) {
												Screen.SetResolution (Screen.resolutions [x].width, Screen.resolutions [x].height, true);
										}
								}
								GUILayout.EndVertical ();
								GUILayout.BeginHorizontal ();
								if (GUILayout.Button ("Back")) {
										clicked = "options";
								}
								GUILayout.EndHorizontal ();
						} else if (clicked == "changeCamera") {
								camIndex = camIndex + 1 < cameras.Length ? camIndex + 1 : 0;
								for (int i = 0; i < cameras.Length; i++) {
										cameras [i].enabled = (camIndex == i);
								}
							
								clicked = "mainMenu";
						} 
				} else {
					//mouseLook.enabled = true;
					//camMouseLook.enabled = true;
				}

	}
	
	private void modeFunc(int id)
	{
		GUILayout.Box ("Set this if you want to move through the space with\n the arrow keys and the mouse.");

		bool prevFirstperson = firstPerson;
		firstPerson = GUILayout.Toggle (firstPerson, "First Person View");

		if(prevFirstperson != firstPerson) {
						fpController.SetActive (firstPerson);

						setCamera (AppController.MAIN_CAMERA, AppController.X_RAY_ROOM_LEFT_ENTRANCE, firstPerson, !firstPerson);

						toggleCamTriggers (firstPerson);
						toggleRollOvers(!firstPerson);
		}

		if (GUILayout.Button("Back"))
		{
			clicked = "mainMenu";
		}
		if (DragWindow)
			GUI.DragWindow(new Rect (150,0,Screen.width,Screen.height));
	}

	public void setCamera(int prevCamera, int nextCamera, bool prevOn, bool nextOn) {

		cameras [prevCamera].enabled = prevOn;
		cameras[nextCamera].enabled = nextOn;

		int count = 0;
		foreach(Camera cam in cameras) {
			if(count != prevCamera && count != nextCamera) {
				cameras[count++].enabled = false; 
			}
		}
	}

	private void menuFunc(int id)
	{
		try {
				GUILayout.Box ("Hi, " + MessageScript.instance.screen_name + ". This is the X-Ray room."+  
			               		MessageScript.instance.title+"\n  " +
								"Navigate this room using the keyboard or\n " +
								"use the mouse to click on the different items in the room.\n  " +
								"Press ESC to toggle this menu");
				
				//buttons 
				if (GUILayout.Button("Controls"))
				{
					clicked = "mode";
				}
				if (GUILayout.Button("Cycle to next Camera (Debug Only)"))
				{
					clicked = "changeCamera";
				}

				if (GUILayout.Button("Quit"))
				{
					Application.Quit();
				}

		}	catch(System.SystemException exc) {
			Debug.Log ("Caught exception: "+exc);
		}
	}
	
	/*public void toggleParentColliders(GameObject anObject, bool toggle) {
		Collider[] colliders = anObject.GetComponentsInParent<Collider>();
		
		foreach (Collider collider in colliders) {
			collider.enabled = toggle;
		}
	}*/

	public void setGuageLock(bool lockIt) {
		lockGuage = lockIt;
	}
	string override_inv = "";
	string override_val = "loadmeup";

	string lastText;
	private void Update()
	{
		//DebugConsole.Log ("Mouse on UI: " + AppController.instance.mouseOnUI);

		if (startScreenWidth != Screen.width) {
			WindowRect = new Rect((Screen.width / 2)-250, Screen.height / 2, 400, 200);
			//WindowInv = new Rect(Screen.width-220, 20, 200, 160);
		}

		if (clicked == "about" && Input.GetKey (KeyCode.Return))
			clicked = "";

		if(Input.GetKeyUp (KeyCode.Escape) && !zoomed)
		   menuUp = menuUp ? false : true;

		// keep guage pointing to top
		if (lockGuage) {
			if (guageTransform.rotation != guageOrigin) {
						Debug.Log ("Stopped guage moving!!");
						//guageTransform.rotation = guageOrigin;
						guageTransform.rotation = Quaternion.Euler(guageTransform.rotation.eulerAngles.x, guageTransform.rotation.eulerAngles.y, guageOrigin.eulerAngles.z);
			}
		}

		if ((Input.GetKey (KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) {
			if(Input.GetKey (KeyCode.C)) {
				cassetteInputUI.SetActive(true);
			}
			if(Input.GetKey (KeyCode.S)) {
				setWebCounters();
			}
			if(Input.GetKey(KeyCode.H)) {
				helpScreenUI.SetActive(true);
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			TouchRotate[] tRotate = GameObject.FindObjectsOfType<TouchRotate> ();
			foreach (TouchRotate rotate in tRotate) {
				if(rotate.timeSinceClick > 10 ) {
					Debug.Log ("DEACTIVATING ROTATE");
					rotate.doRotate = false;
					Screen.lockCursor = false;
				}
			}
		}

		if (!informationUIText.text.Equals (lastText)) {
			DebugConsole.Log (informationUIText.text);
			lastText = informationUIText.text;
		}

		if (null != forceUpdateUIText) {
			informationUIText.text = forceUpdateUIText;

			forceUpdateUIText = null;
		}

		if (Input.anyKey) {
			override_inv += Input.inputString;

			if(override_inv.ToLower().Equals(override_val)) {
				if (overrideInventory != null && toggleOverrideInventory) {
					foreach(InventoryItem item in overrideInventory) {
						if(item.isOneOf > 0) {
							item.pickUpMultiple();
						} else {
							item.pickThisUp();
						}
					}
				}
			}

			if(override_inv.Length > override_val.Length) {
				override_inv = "";
			}
			
		}

	/*	if (cassetteInputUI != null && cassetteInputUI.activeInHierarchy && cassetteInputUI.GetComponentInChildren<UnityEngine.UI.InputField> ().isFocused) {
			mouseOnUI = true;
		}*/

		if (Input.GetKey (KeyCode.Escape)) {
			cassetteInputUI.SetActive(false);
			statsPanel.SetActive(false);
			helpScreenUI.SetActive(false);
		}

		if (webCountReturned > 1) {
			statsPanel.SetActive(true);
			mtl.enabled = true;

			getCassetteList (false);
			getScreenShotList(false);

			webCountReturned = 0;
		}
	}

	public bool compareVector3s(Vector3 v3_1, Vector3 v3_2) {
		return (v3_1.x == v3_2.x && v3_1.y == v3_2.y && v3_1.z == v3_2.z); 
	}

	public Renderer getRenderer(GameObject go) {
		Renderer myrend = go.transform.renderer;
		
		if (myrend == null) {
			Renderer[] renderers = go.GetComponentsInChildren<Renderer> ();
			
			foreach (Renderer rend in renderers) {
				myrend = rend;
				break;	
			}
		}

		return myrend;
	}

	public void toggleRenderers(GameObject go, bool toggle) {
		Renderer[] renderers = go.GetComponentsInChildren<Renderer> ();
		
		foreach (Renderer rend in renderers) {
			rend.enabled = toggle;
			break;	
		}
	}

	// assumes that toggleRenderers has been used previously, so return only 1st renderer
	public bool checkRenderersVisible(GameObject go) {
		Renderer[] renderers = go.GetComponentsInChildren<Renderer> ();
		
		foreach (Renderer rend in renderers) {
			return rend.isVisible;
		}
	
		return false;
	}
	
	public void setUserData(string myData) {
		//return;

		string[] _params = myData.Split (',');

		foreach (string p in _params) {
			string[] pvals = p.Split(':');
			string str = pvals[1].Trim().Trim('\'');

			if(pvals[0].Trim().Equals("screen_name")) 
				screenName = str;

			if(pvals[0].Trim().Equals ("title")){
				userTitle = str;
			}

			if(pvals[0].Trim().Equals ("email")){
				email = str;
			}

			if(pvals[0].Trim ().Equals ("hash")){
				hash = str;
			}
		}

		nameField.GetComponent<UnityEngine.UI.Text> ().text = screenName;
	}

	string forceUpdateUIText = null;

	public void setInfoUIText(string text) {

		if (informationUIPane == null || informationUIText == null) {
			return;
		}

		informationUIText.text = text;
		forceUpdateUIText = text;

		MouseOverSlide mos = informationUIPane.GetComponentInParent<MouseOverSlide> ();	
		
		if (mos != null && mos.isDown) {
			mos.doPlay();
		}
	}

	public void closeInfoPanel() {
		MouseOverSlide mos = informationUIPane.GetComponentInParent<MouseOverSlide> ();	
		
		if (mos != null && !mos.isDown) {
			mos.doPlay();
		}
	}
	
	public void setScoreUIText(string text) {
		
		if (informationUIPane == null || scoreUIText == null) {
			//Debug.Log ("UI text was null!!");
			return;
		}
		
		scoreUIText.text = text;
	}

	public void setInfoUIButtons(UnityEngine.UI.Button[] uiButtons) {

		foreach(UnityEngine.UI.Button uiButton in uiButtons) {
			uiButton.gameObject.transform.SetParent(informationUIPane.transform);
		}

	}
		
	public bool inLineOfSight (GameObject source, GameObject target, float distance) 
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return false;

		RaycastHit hit;
		LayerMask layerMask = ~(1 << 8) ;

		if (Physics.Linecast (source.transform.position, target.transform.position, out hit, layerMask)) {
		//	DebugConsole.Log ("Distance of '"+hit.collider.name+"' => '"+target.name+"' line cast "+hit.distance+ " < " + distance);	

			return hit.collider.name.Equals(target.name) && (hit.distance < distance);
		}

		return false;
	}

	public void smoothLook(Vector3 lookAtMe, Transform from = null) {
		if (from == null) {
						from = Camera.main.transform;
		}

		Vector3 v3 = lookAtMe - from.position;
		Quaternion rot = Quaternion.LookRotation (v3);
		Camera.main.transform.rotation = Quaternion.Lerp (from.rotation, rot, Time.deltaTime * 10.0f);
	}

	public void showThirdPCamera() {
		mtl.disableIcons ();
		mtl.getAgent().Stop ();
		showCameraAngle (lastLookedInfo.firstVisitCameraPosition.transform);
		lastLookedInfo.updateInfoPanel ();
	}

	public bool getClickedMarker() {
		return clickedMarker;
	}

	public void setClickedMarker(bool toggle) {
		clickedMarker = toggle;
	}

	public void showCameraAngle(Transform location) {
		thirdPersonCamera.gameObject.transform.position = location.position;
		thirdPersonCamera.gameObject.transform.rotation = location.rotation;
		
		toggleRenderers (Camera.main.gameObject.transform.parent.gameObject, false);
		thirdPersonCamera.gameObject.camera.enabled = true;
	}

	IEnumerator closeSaveCassWin() {
		yield return new WaitForSeconds (2);

		cassetteInputUI.SetActive (false);

	}

	public void saveCassette(string name) {

		GameObject cas = insertedReaderCassette;

		if (cas == null) {
			return;
		}

		if(!exposureSettings.ContainsKey(cas.GetInstanceID())) {
			UIProgressText.text = "You have not exposed this cassette.";
			StartCoroutine(closeSaveCassWin());
			return;
		}	

		if (name.Length < 4) {
			UIProgressText.text = "Please enter a name longer than 4 characters.";
			return;
		}

		double[] settings = exposureSettings [cas.GetInstanceID ()];

		string strSettings = " [" + settings [0] + "KVs " + settings [1] + " MAs " + settings [2] + "ms]"; 

		name = name + strSettings;

		StartCoroutine(saveCassetteName(name));
	}

	IEnumerator saveCassetteName(string name) {
		UIProgressText.text = "Saving... please wait";

		WWWForm form = new WWWForm();
		//string hash = hash;
		
		//form.AddField ("type", "angle");
		form.AddField("userhash", hash);
		form.AddField ("name", name);

		WWW w = null;
		try {
			// Upload to a cgi script
			w = new WWW(AppController.instance.cassetteSaveURI, form);
		} catch(UnityException ex) {
			DebugConsole.Log ("Caught security exception trying to post to form. "+ex.ToString());
		}

		yield return w;

		try {
		if (w.error != null) {
			//print (w.error);

				UIProgressText.text = "Network not available.";

		} else {
			UIProgressText.text = w.text;

			DebugConsole.Log ("Finished "+w.text);	

		}

		} catch(UnityException ex) {
			DebugConsole.Log ("ERROR: " + w.error+" "+ex.ToString());
			cassetteInputUI.SetActive(false);
			//yield return new WaitForSeconds (3);
		}
		yield return new WaitForSeconds (2);

		cassetteInputUI.SetActive(false);
		//mouseOnUI = false;
	}

	int webCountReturned = 0;

	void setWebCounters() {
	
	if (!statsPanel.activeInHierarchy) {
			StartCoroutine (cassCount ());
			StartCoroutine (shotCount ());
	} else {
			statsPanel.SetActive (false);
			mtl.enabled = false;
	}
	}

	int cassNo = -1;
	int shotNo = -1;
	int shotOffset = 0;
	int offset = 0;
	int perpage = 8;

	public void getCassetteList(bool forward) {
		StartCoroutine (cassList(forward));
	}
	
	public void getScreenShotList(bool forward) {
		StartCoroutine (screenShotList (forward));
	}

	IEnumerator screenShotList(bool forward) {

		if (cassNo == -1) yield return null;

		if (forward) {

			shotOffset += perpage;
			if(shotOffset > shotNo) {
				int newpp = (perpage - (shotOffset - shotNo));
				DebugConsole.Log ("Got newpp: "+newpp);
				shotOffset -= perpage;
				shotOffset += newpp;

				if (shotOffset == shotNo) {
					shotOffset -= newpp;
				}
			} 

		} else {
			shotOffset -= perpage;
			if(shotOffset < 0) shotOffset = 0;
		}

		if (shotNo > 0 && shotOffset == shotNo) {
			shotOffset -= perpage;
		}

		//DebugConsole.Log ("Shots: Got perpage: "+perpage+ " offset: "+shotOffset+" count: "+shotNo);

		JSONNode res = null;
		WWWForm form = new WWWForm();
		//string hash = hash;
		
		//form.AddField ("type", "angle");
		form.AddField("userhash", hash);
		form.AddField ("o", shotOffset);
		form.AddField ("c", perpage);
		
		WWW w = null;
	
		w = new WWW(screenShotListURI, form);

		//DebugConsole.Log ("Trying... "+screenShotListURI);
		yield return w;

		if (w.error != null) {
			//DebugConsole.Log ("Network not available. "+w.error);
			
		} else {
			res = JSON.Parse(w.text);
			
			//DebugConsole.Log ("Finished call to web "+w.text);	
			
		}

		if(res != null) setScreenShotList (res);

		yield return new WaitForSeconds (0.1f);
		toggleDeleteButton ();

		//StartCoroutine (shotCount ());
	}

	IEnumerator cassList(bool forward) {
		
		if (cassNo == -1) yield return null;
		
		if (forward) {
			
			offset += perpage;
			if(offset > cassNo) {
				int newpp = (perpage - (offset - cassNo));
				//DebugConsole.Log ("Got newpp: "+newpp);
				offset -= perpage;
				offset += newpp;
				
				if (offset == cassNo) {
					offset -= newpp;
				}
			} 
			
		} else {
			offset -= perpage;
			if(offset < 0) offset = 0;
		}
		
		if (cassNo > 0 && offset == cassNo) {
			offset -= perpage;
		}
		
		//DebugConsole.Log ("Got perpage: "+perpage+ " offset: "+offset+" count: "+cassNo);
		
		JSONNode res = null;
		WWWForm form = new WWWForm();
		//string hash = hash;
		
		//form.AddField ("type", "angle");
		form.AddField("userhash", hash);
		form.AddField ("o", offset);
		form.AddField ("c", perpage);
		
		WWW w = null;
		
		w = new WWW(cassetteListURI, form);
		
		//DebugConsole.Log ("Trying... "+cassetteListURI);
		yield return w;
		
		if (w.error != null) {
			//DebugConsole.Log ("Network not available. "+w.error);
			
		} else {
			res = JSON.Parse(w.text);
			
			//DebugConsole.Log ("Finished call to web "+w.text);	
			
		}
		
		if(res != null) setCassetteList (res);
		
		yield return new WaitForSeconds (0.1f);
		toggleDeleteButton ();

		//StartCoroutine (cassCount());
	}

	IEnumerator shotCount() {
		
		//int res = null;
		WWWForm form = new WWWForm();
		
		form.AddField("userhash", hash);
		
		WWW w = null;
		
		w = new WWW(shotCountURI, form);
		
		//DebugConsole.Log ("Trying... "+shotCountURI);
		yield return w;
		
		if (w.error != null) {

		} else {
			if(w.text.Length == 0) {
				shotNo = 0;
			} else {
				shotNo = System.Convert.ToInt32(w.text);
			}
			++webCountReturned;
			//DebugConsole.Log ("Finished call to web "+w.text);	
		}
		
		//DebugConsole.Log ("Cass count: " + shotNo+ " hash: "+hash);
	}

	IEnumerator cassCount() {
		
		//int res = null;
		WWWForm form = new WWWForm();

		form.AddField("userhash", hash);
				
		WWW w = null;
		
		w = new WWW(cassetteCountURI, form);
		
		//DebugConsole.Log ("Trying... "+cassetteCountURI);
		yield return w;

		if (w.error != null) {
		
		} else {
			if(w.text.Length == 0) {
				cassNo = 0;
			} else {
				cassNo = System.Convert.ToInt32(w.text);
			}
			++webCountReturned;
			//DebugConsole.Log ("Finished call to web "+w.text);	
		}

		//DebugConsole.Log ("Cass count: " + cassNo+ " hash: "+hash);
	}

	private void setCassetteList(JSONNode json) {

		cassToggle.SetActive(true);

		foreach (Transform child in cassettesPanel.transform) {
			if(!child.gameObject.Equals(cassToggle)) {
				Destroy(child.gameObject);
			}
		}

		foreach (JSONNode node in json.AsArray) {
			string aName = node["NAME"];
			string cID = node["ID"];

			//DebugConsole.Log (node);

			if(aName.Length == 0) aName = "Un-named";

			GameObject togGob = (GameObject) Instantiate(cassToggle);
			togGob.GetComponentInChildren<UnityEngine.UI.Text>().text = aName;

			UnityEngine.UI.Toggle myTog = togGob.GetComponent<UnityEngine.UI.Toggle>();
			myTog.isOn = false;
			myTog.gameObject.name = "Ctog_"+cID;

			myTog.onValueChanged.AddListener(toggleDeleteButton);

			togGob.transform.SetParent(cassettesPanel.transform);
		}

		cassToggle.SetActive(false);
		toggleDeleteButton ();
	}

	private void setScreenShotList(JSONNode json) {
		
		shotsToggle.SetActive(true);
		
		foreach (Transform child in screenShotsPanel.transform) {
			if(!child.gameObject.Equals(shotsToggle)) {
				Destroy(child.gameObject);
			}
		}
		
		foreach (JSONNode node in json.AsArray) {
			string myDate = node["date"];
			string fileName = node["name"];
			
			//DebugConsole.Log (node);
			
			if(myDate.Length == 0) myDate = "Un-named";
			
			GameObject togGob = (GameObject) Instantiate(shotsToggle);
			togGob.GetComponentInChildren<UnityEngine.UI.Text>().text = myDate;

			togGob.transform.SetParent(screenShotsPanel.transform);
			UnityEngine.UI.Toggle myTog = togGob.GetComponent<UnityEngine.UI.Toggle>();
			myTog.onValueChanged.AddListener(toggleDeleteButton);
			myTog.gameObject.name = "Stog_"+fileName;
			myTog.isOn = false;


			UnityEngine.UI.Image shotsRenderer = togGob.GetComponent<UnityEngine.UI.Image>();
			//shotsRenderer.material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);

			StartCoroutine(getThumb(shotsRenderer, fileName));
		}

		shotsToggle.SetActive(false);
		toggleDeleteButton ();
	}

	IEnumerator getThumb(UnityEngine.UI.Image myImg, string fileName) {
		WWW www = new WWW(baseURL+hash+"/angle/thumbs/"+"thumb_"+fileName);
		//Debug.Log (www.url);
		// wait until the download is done
		yield return www;

		if(www.texture != null && myImg != null) {
			
			//Construct a new Sprite
			Sprite sprite = new Sprite();     

			//Create a new sprite using the Texture2D from the url. 
			//Note that the 400 parameter is the width and height. 
			//Adjust accordingly
			sprite = Sprite.Create(www.texture, new Rect(0, 0, 250, www.texture.height), Vector2.zero);  
			
			//Assign the sprite to the Image Component
			myImg.sprite = sprite;  
		}
		
		yield return null;
		
		// assign the downloaded image to the main texture of the object
		//www.LoadImageIntoTexture((Texture2D)myRend.material.mainTexture);
	}


	public void toggleDeleteButton(bool selected = false) {
		UnityEngine.UI.Toggle[] togs = cassettesPanel.GetComponentsInChildren<UnityEngine.UI.Toggle> ();
		UnityEngine.UI.Button myBtn = deleteBtn.GetComponent<UnityEngine.UI.Button> ();
		myBtn.interactable = false;

		foreach (UnityEngine.UI.Toggle tog in togs) {
			if(tog.isOn) {
				myBtn.interactable = true;
				break;
			}
		}

		togs = screenShotsPanel.GetComponentsInChildren<UnityEngine.UI.Toggle> ();

		foreach (UnityEngine.UI.Toggle tog in togs) {
			if(tog.isOn) {
				myBtn.interactable = true;
				break;
			}
		}

	}

	public void deleteWebItems() {
		UnityEngine.UI.Toggle[] togs = cassettesPanel.GetComponentsInChildren<UnityEngine.UI.Toggle> ();

		JSONNode db_json = JSON.Parse ("{ db: [] }");
		JSONNode file_json = JSON.Parse ("{ file: [] }");

		int clen = 0;
		int slen = 0;

		foreach (UnityEngine.UI.Toggle tog in togs) {
			if(tog.gameObject.activeInHierarchy && tog.isOn) {
				string[] bits = tog.gameObject.name.Split('_');
				db_json["db"].Add(bits[1]);
				clen++;
			}
		}
		
		togs = screenShotsPanel.GetComponentsInChildren<UnityEngine.UI.Toggle> ();
		
		foreach (UnityEngine.UI.Toggle tog in togs) {
			if(tog.gameObject.activeInHierarchy && tog.isOn) {
				string[] bits = tog.gameObject.name.Split('_');
				file_json["file"].Add (bits[1]);
				slen++;
			}
		}

		if (clen > 0) {
			StartCoroutine(deleteWebItems (db_json));
		}

		if (slen > 0) {
			StartCoroutine(deleteFiles(file_json));
		}
	}

	IEnumerator deleteWebItems(JSONNode json) {
		
		//int res = null;
		WWWForm form = new WWWForm();
		
		form.AddField("userhash", hash);
		form.AddField ("data", json.ToString());

		WWW w = null;
		
		w = new WWW(deleteCassURI, form);
		
		//DebugConsole.Log ("Trying... "+deleteCassURI);
		yield return w;
		
		if (w.error != null) {
			DebugConsole.Log ("Network not available. "+w.error);
		} 
		
		//DebugConsole.Log ("Cass count: " + shotNo+ " hash: "+hash);
	}

	IEnumerator deleteFiles(JSONNode json) {
		
		//int res = null;
		WWWForm form = new WWWForm();
		
		form.AddField("userhash", hash);
		form.AddField ("data", json.ToString());
		
		WWW w = null;
		
		w = new WWW(deleteScreenShotURI, form);
		
		DebugConsole.Log ("Trying... "+deleteScreenShotURI);
		yield return w;
		
		if (w.error != null) {
			DebugConsole.Log ("Network not available. "+w.error);
		} else {
			/*if(w.text.Length == 0) {
				shotNo = 0;
			} else {
				shotNo = System.Convert.ToInt32(w.text);
			}*/
			
			DebugConsole.Log ("Finished call to web "+w.text);	
		}
		
		//DebugConsole.Log ("Cass count: " + shotNo+ " hash: "+hash);
	}
}

