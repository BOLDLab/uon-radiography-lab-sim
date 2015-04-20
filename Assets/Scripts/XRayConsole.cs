using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class XRayConsole : MonoBehaviour {

	public Texture bgTexture; 
	public GUIStyle xRaySquareBGUIStyle;
	public GUIStyle xRayCircleBGUIStyle;
	public GUIStyle xRayPanelGUIStyle;
	public GUIStyle XRayBottomPanelGUIStyle;
	public GUIStyle XRayExposureButtonStyle;

	public GUIStyle powerButton;

	public GUIContent iconTable;
	public GUIContent iconStanding;
	public GUIContent iconHead;
	public GUIContent iconPower;

	public GUIContent circleLight;
	public GUIContent dashLight;

	public GUIContent topDisplayBackground;
	public Font smallBottomFont;	

	public int circleXOffset = 46;
	public int circleYoffset = 54;

	public int sqXOffset = 12;
	public int sqYOffset = 47;

	public XRayMachineMenu menu; 

	public bool show = false;
	public bool stateChange = false;

	public GameObject locationOnClose;

	public Camera collumationCamera;
	public Transform aperturePosition;
	
	private string topDisplayText = "\n50kV 2.00mAs 7.14ms";

	// not used
	private string headAndShouldersTxt = "shoulder ap=\t         clavicle=\n\nscapula ap=\t         humerus ap=\t\n\nelbow=   *\n\n\t\t\n\t\t";//menu";
	 
	// not used
	private string erectBukyDisplayText = "skull ap/pa\t          skull lat\n\nskull /Towne   skull axial\n\n" +
				"petr.b.Stenv.   skull spec.\n\n\t\t\n\t\tmenu";

	private string lowerExt = "femur            patella\n\n\n\nknee             Knee/Frick\t\n\n\n"
							+ "lower leg        ankle\n\n\nfoot\t           menu";


	private string upperExt = "humerus          elbow\n\n\n\nforearm          wrist ap\t\n\n\n"
							+ "hand             wrist lat\n\n\nscaphoid\t\tmenu";

	private string mainMenuDisplayText = 	"SKULL\t            SPINE\n\n\n" +
											"THORAX/SHOULD\t    ABDOMEN/PELVIS\n\n\n" +
											"UPPER EXTR.\t       LOWER EXT.";

	public int KVsIndex = 0;
	public int[] KVs = new int[] {40,41,42,44,46,48,50,52,55,57,60,63,66,70,73,77,81,85,90,96,102,109,117,125};

	public int mAsIndex = 0;
	public double[] mAs = new double[] {0.50, 0.63,0.80,1.0,1.25,1.6,2.0,2.5,3.2,4.0,5.0,6.3,8.0,10.0,12.5,16,20,25,32,40,50,63,80,100,125,160,200,250,320,400,500,630,800,850};

	public int msIndex = 0;
	public double[] ms = new double[] {1.72,2.03,2.58,3.23,4.04,5.17,6.47,8.08,10.3,12.9,16.1,20.3,25.8,32.3,40.4,51.7,64.7,80.8,104,133,172,224,293,383,503,685,909,1220,1700,2350,3300,4710,7020,7790};
	
	public IDictionary<string, double[]> presets = new Dictionary<string, double[]> ();
	public IDictionary<string, bool> lowerExtToggles = new Dictionary<string, bool> ();
	public IDictionary<string, bool> upperExtToggles = new Dictionary<string, bool> ();
	
	private string bottomDisplayText = "";

	private short exposureReady = 0;
	private bool circleShown = true;
	private int flashInterval = 0;
	private bool powerOn = false;

	private bool toggleTable = false;
	private bool toggleUnassigned = false;
	private bool toggleUprightBucky = false;
	private bool toggleAboveTable = false;

	// bottom panel button toggles
	private bool toggleMainMenu = false;
	private bool toggleLowerExt = false;
	//private bool toggleAbdPelvis = false;
	//private bool toggleSpine = false;

	private bool toggleUpperExt = false;
	//private bool[] lowerExtToggles;

	public int exposureMessageDisplayTime = 1000;
	//private int expDispCount = 0;

	public GameObject door;
	private DoorInterface doorInterface;

	//private bool exposed = false;

	// for turning off items in XRayView
	private InventoryItem[] invItems;

	public Material xrayMaterial;
	public Material xrayBacking;
	public Material xrayFlesh;
	
	private Material origMaterial;
	private Material origFlesh;

	private bool consoleTopDisplayChanged = false;

	bool expError = false;

	void Awake() {
		// upper extr presets
		presets["humerus"] = new double[] {66, 6.4, 24.8};
		presets["forearm"] = new double[] {55,4.0,12.9};
		presets["hand"] = new double[] {50,2.0,7.14};
		presets["scaphoid"] = new double[] {52,5.0,17.1}; 
		presets["elbow"] = new double[] {52,5.0,17.1}; 
		presets["wrist ap"] = new double[] {50,3.2,11.4};
		presets["wrist lat"] = new double[] {52,5.0,17.1}; 

		// upper extr toggle settings
		upperExtToggles ["humerus"] = false;
		upperExtToggles ["forearm"] = false;
		upperExtToggles ["hand"] = false;
		upperExtToggles ["scaphoid"] = false;
		upperExtToggles ["elbow"] = false;
		upperExtToggles ["wrist ap"] = false;
		upperExtToggles ["wrist lat"] = false;

		// lower extr toggle settings
		lowerExtToggles ["femur"] = false;
		lowerExtToggles ["knee"] = false;
		lowerExtToggles ["lower leg"] = false;
		lowerExtToggles ["foot"] = false;
		lowerExtToggles ["patella"] = false;
		lowerExtToggles ["Knee/Frick"] = false;
		lowerExtToggles ["ankle"] = false;

		// lower extr presets
		presets["femur"] = new double[] {70,16.0, 65.8}; 
		presets["knee"] = new double[] {55,8.0, 25.8}; 
		presets["lower leg"] = new double[] {55,5.0, 16.1};
		presets["foot"] = new double[] {50,4.0, 14.2}; 
		presets["patella"] = new double[] {63, 6.3, 23.3}; 
		presets["Knee/Frick"] = new double[] {60, 5.0, 17.6}; 
		presets["ankle"] = new double[] {55, 4.0, 12.9}; 
	}
	// Use this for initialization
	void Start () {
		doorInterface = door.GetComponent<DoorInterface> ();
		invItems = GameObject.FindObjectsOfType<InventoryItem>();

		//Debug.Log ("Path: "+Application.dataPath);
	}
	
	public void showPanel(bool show) {
		this.show = show;
		expError = false;

		//AppController.instance.mtl.getAgent ().Stop ();
		AppController.instance.mtl.enabled = !show;
	}
	
	public bool isVisible() {
		return this.show;
	}


	bool resetTopLevelToggles() {
		toggleTable = false;
		toggleUnassigned = false;
		toggleUprightBucky = false;	
		toggleAboveTable = false;

		return powerOn;
	}

	void resetToTopMenu() {
		toggleTable = false;
		toggleUnassigned = false;
		toggleUprightBucky = false;
		toggleAboveTable = true;
		
		// bottom panel button toggles
		toggleMainMenu = false;
		toggleLowerExt = false;
		//toggleAbdPelvis = false;
		//toggleSpine = false;
		
		toggleUpperExt = false;
	}

	bool powerOnConsole() {
		powerOn = false;
		return true;
	}

	void expFunc(int id) {
		GUI.Box (new Rect (0, 0, 300, 300), "Press the exposure button again to expose the casette.");
	}

	Rect cont;
	
	string menuLevel = "top";
	string lastKey = "";
	//string currentKey = "";

	void OnGUI() {

		if (!this.show)
				return;

		if (AppController.instance == null)
						return;

		cont = new Rect(0, 0, Screen.width-5, Screen.height-5);

		GUI.DrawTexture (cont, bgTexture);

		//DebugConsole.Log ("Menulevel: "+menuLevel+ " main menu "+toggleMainMenu);
		// left side grid square buttons
		if (GUI.Toggle (ResizeGUI (new Rect (148 - sqXOffset, 359 - sqYOffset, 75, 68)), toggleTable, iconTable, xRaySquareBGUIStyle)) {
			//toggleTable = resetTopLevelToggles();

			//Debug.Log ("Toggle: "+toggleTable);
		};  // table

		if (GUI.Toggle (ResizeGUI (new Rect (232 - sqXOffset, 359 - sqYOffset, 75, 68)), toggleUnassigned, GUIContent.none, xRaySquareBGUIStyle)) {
			//toggleUnassigned = resetTopLevelToggles();
		}; // unassigned
		if (GUI.Toggle (ResizeGUI (new Rect (148 - sqXOffset, 439 - sqYOffset, 75, 68)), toggleUprightBucky, iconStanding, xRaySquareBGUIStyle)) {
			//toggleUprightBucky = resetTopLevelToggles();
		}; // standing plate

		if (GUI.Toggle (ResizeGUI (new Rect (232 - sqXOffset, 439 - sqYOffset, 75, 68)), toggleAboveTable, iconHead, xRaySquareBGUIStyle)) {
			toggleAboveTable = resetTopLevelToggles();	
		}; // upper body
		
		GUI.Button (ResizeGUI(new Rect(148-sqXOffset, 520-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(232-sqXOffset, 520-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(148-sqXOffset, 598-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(232-sqXOffset, 598-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned

		GUI.Button (ResizeGUI(new Rect(148-sqXOffset, 724-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(232-sqXOffset, 724-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(148-sqXOffset, 800-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(232-sqXOffset, 800-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(148-sqXOffset, 884-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned
		GUI.Button (ResizeGUI(new Rect(232-sqXOffset, 884-sqYOffset, 75, 68)), GUIContent.none, xRaySquareBGUIStyle); // unassigned

		// left top panel square buttons
		GUI.Button (ResizeGUI(new Rect(382-sqXOffset, 157, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned
		GUI.Button (ResizeGUI(new Rect(382-sqXOffset, 249, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned

		GUI.Button (ResizeGUI(new Rect(1152-sqXOffset, 157, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned
		GUI.Button (ResizeGUI(new Rect(1152-sqXOffset, 249, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned

		// bottom panel horizontal square buttons
		GUI.Button (ResizeGUI(new Rect(505-sqXOffset, 825, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned
		GUI.Button (ResizeGUI(new Rect(589-sqXOffset, 825, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned

		GUI.Button (ResizeGUI(new Rect(1032-sqXOffset, 825, 75, 68)), GUIContent.none, xRaySquareBGUIStyle);  // unassigned

		// bottom left vertical circle button rects
		Rect bltog1 = new Rect (430 - circleXOffset, 535 - circleYoffset, 48, 54);
		Rect bltog2 = new Rect (430 - circleXOffset, 627 - circleYoffset, 48, 54);
		Rect bltog3 = new Rect (430 - circleXOffset, 718 - circleYoffset, 48, 54);
		Rect bltog4 = new Rect(430-circleXOffset, 810-circleYoffset, 48, 54);

		// bottom right vertical circle button rects
		Rect brtog1 = new Rect (1200 - circleXOffset, 535 - circleYoffset, 48, 54);
		Rect brtog2 = new Rect (1200 - circleXOffset, 627 - circleYoffset, 48, 54);
		Rect brtog3 = new Rect (1200 - circleXOffset, 718 - circleYoffset, 48, 54);
		Rect brtog4 = new Rect (1200 - circleXOffset, 810 - circleYoffset, 48, 54);

		// left vertical bottom panel circle buttons

		// right vertical bottom panel circle buttons
		if (toggleAboveTable && menuLevel == "top") {
			//toggleSpine = GUI.Button (ResizeGUI(brtog1), GUIContent.none, xRayCircleBGUIStyle); // spine
			//toggleAbdPelvis = GUI.Toggle (ResizeGUI (brtog2), toggleAbdPelvis, GUIContent.none, xRayCircleBGUIStyle); // abd & pelvis
			toggleLowerExt = GUI.Toggle (ResizeGUI (brtog3), toggleLowerExt, GUIContent.none, xRayCircleBGUIStyle); // lower ext
			toggleUpperExt = GUI.Toggle (ResizeGUI (bltog3), toggleLowerExt, GUIContent.none, xRayCircleBGUIStyle); // lower ext
		} 

		if (GUI.Toggle (ResizeGUI (brtog4), toggleMainMenu, GUIContent.none, xRayCircleBGUIStyle)) {
			if(toggleAboveTable) { 
				toggleUpperExt = false;
				toggleLowerExt = false;
			}		
		};// main menu

		// top left two circles
		GUI.Button (ResizeGUI(new Rect(153, 163, 48, 54)), GUIContent.none, xRayCircleBGUIStyle);
	
		powerOn = GUI.Toggle (ResizeGUI (new Rect (200, 139, 96, 106)), powerOn, iconPower, powerButton);

		if (toggleTable) {
				bottomDisplayText = headAndShouldersTxt;
			//toggleMainMenu = false;	
				//menuLevel = "head & shoulders";
			menuLevel = "top";
		} else if (toggleUprightBucky) {
				bottomDisplayText = erectBukyDisplayText;
			//toggleMainMenu = false;	
				//menuLevel = "erect buky";
			menuLevel = "top";
		} else if (toggleAboveTable) {
				bottomDisplayText = mainMenuDisplayText;
			//toggleMainMenu = false;	
				//menuLevel = "above table";
			menuLevel = "top";
		} else {
			//if(!powerOn) {
				bottomDisplayText = "";
		
			menuLevel = "top";
		}

		bool allowMenu = (toggleTable || toggleUprightBucky || toggleAboveTable);

		if (toggleAboveTable) {

						if (toggleLowerExt) {
								bottomDisplayText = lowerExt;
								menuLevel = "lower ext";
								toggleUpperExt = false;
						} else if(toggleUpperExt) {
							bottomDisplayText = upperExt;
							menuLevel = "upper ext";
							toggleLowerExt = false;
			} 
		}

		// check if a user selected a new key e.g. "knee", indicating a state change
		if (consoleTopDisplayChanged) {
			addAsterix ();
		}

		//int[] powerInd = new int[] {};
		//string topTextChanged = "";
		if (menuLevel.Equals ("lower ext") || menuLevel.Equals("upper ext")) {
			if(toggleAboveTable) {

				if (toggleLowerExt) {
					if(GUI.Toggle (ResizeGUI (brtog1), lowerExtToggles["patella"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "patella");
					} 
					if(GUI.Toggle (ResizeGUI (brtog2), lowerExtToggles["Knee/Frick"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "Knee/Frick");
					}
					if(GUI.Toggle (ResizeGUI (brtog3), lowerExtToggles["ankle"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "ankle");
					}
					if(GUI.Toggle (ResizeGUI (bltog1), lowerExtToggles["femur"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "femur");
					}
					if(GUI.Toggle (ResizeGUI (bltog2), lowerExtToggles["knee"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "knee");
					}
					if(GUI.Toggle (ResizeGUI (bltog3), lowerExtToggles["lower leg"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "lower leg");
					}
					if(GUI.Toggle (ResizeGUI (bltog4), lowerExtToggles["foot"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(lowerExtToggles, "foot");
					}

					if (lowerExtToggles ["ankle"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"ankle");
					}
					
					if (lowerExtToggles ["Knee/Frick"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"Knee/Frick");
					}
					
					if (lowerExtToggles ["patella"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"patella");
					}
					
					if (lowerExtToggles ["femur"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"femur");
					}
					
					if (lowerExtToggles ["knee"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"knee");
					}
					
					if (lowerExtToggles ["lower leg"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"lower leg");
					}
					
					if (lowerExtToggles ["foot"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"foot");
					}
				} else if (toggleUpperExt) {
					if(GUI.Toggle (ResizeGUI (brtog1), upperExtToggles["elbow"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "elbow");

					} 
					if(GUI.Toggle (ResizeGUI (brtog2), upperExtToggles["wrist ap"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "wrist ap");
					}
					if(GUI.Toggle (ResizeGUI (brtog3), upperExtToggles["wrist lat"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "wrist lat");
					}
					if(GUI.Toggle (ResizeGUI (bltog1), upperExtToggles["humerus"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "humerus");
					}
					if(GUI.Toggle (ResizeGUI (bltog2), upperExtToggles["forearm"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "forearm");
					}
					if(GUI.Toggle (ResizeGUI (bltog3), upperExtToggles["hand"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "hand");
					}
					if(GUI.Toggle (ResizeGUI (bltog4), upperExtToggles["scaphoid"], GUIContent.none, xRayCircleBGUIStyle)) {
						setExclusiveToggle(upperExtToggles, "scaphoid");
					}

					if (upperExtToggles ["humerus"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"humerus");
					}
					
					if (upperExtToggles ["forearm"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"forearm");
					}
					
					if (upperExtToggles ["hand"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"hand");
					}
					
					
					if (upperExtToggles ["scaphoid"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"scaphoid");
					}
					
					if (upperExtToggles ["elbow"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"elbow");
					}
					
					if (upperExtToggles ["wrist ap"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"wrist ap");
					}
					
					if (upperExtToggles ["wrist lat"]) {
						bottomDisplayText = setPanelBrackets(bottomDisplayText,"wrist lat");
					}
				} else if (toggleMainMenu && allowMenu) {
					bottomDisplayText = mainMenuDisplayText;	
					menuLevel = "top";
				}  
			}
		} else {
			GUI.Button (ResizeGUI (brtog1), GUIContent.none, xRayCircleBGUIStyle); // unassigned
			GUI.Button (ResizeGUI (brtog3), GUIContent.none, xRayCircleBGUIStyle); // unassigned
			GUI.Button (ResizeGUI (brtog2), GUIContent.none, xRayCircleBGUIStyle); // unassigned
			GUI.Button (ResizeGUI(bltog1), GUIContent.none, xRayCircleBGUIStyle); // unassigned
			GUI.Button (ResizeGUI(bltog2), GUIContent.none, xRayCircleBGUIStyle); // unassigned
			GUI.Button (ResizeGUI(bltog3), GUIContent.none, xRayCircleBGUIStyle); // unassigned
			GUI.Button (ResizeGUI(bltog4), GUIContent.none, xRayCircleBGUIStyle); // unassigned
		}



		// top row under display panel circle buttons
		if (GUI.Button (ResizeGUI (new Rect (558 - circleXOffset, 416 - circleYoffset, 48, 54)), GUIContent.none, xRayCircleBGUIStyle)) {
			KVsIndex = KVsIndex > 0 ? --KVsIndex : KVsIndex;
			consoleTopDisplayChanged = true;
		}; 
		
		// decrement KV
		if (GUI.Button (ResizeGUI (new Rect (639 - circleXOffset, 416 - circleYoffset, 48, 54)), GUIContent.none, xRayCircleBGUIStyle)) {
			KVsIndex = KVsIndex < KVs.Length-1 ? ++KVsIndex : KVsIndex;
			consoleTopDisplayChanged = true;
		}; // increment KV
		
		if (GUI.Button (ResizeGUI (new Rect (784 - circleXOffset, 416 - circleYoffset, 48, 54)), GUIContent.none, xRayCircleBGUIStyle)) {
			mAsIndex = mAsIndex > 0 ? --mAsIndex : mAsIndex;
			consoleTopDisplayChanged = true;
		}; // decrement mAs
		
		if (GUI.Button (ResizeGUI (new Rect (865 - circleXOffset, 416 - circleYoffset, 48, 54)), GUIContent.none, xRayCircleBGUIStyle)) {
			mAsIndex = mAsIndex < mAs.Length-1 ? ++mAsIndex : mAsIndex;	
			consoleTopDisplayChanged = true;
		}; // increment mAs
		
		if (GUI.Button (ResizeGUI (new Rect (1003 - circleXOffset, 416 - circleYoffset, 48, 54)), GUIContent.none, xRayCircleBGUIStyle)) {
			msIndex = msIndex > 0 ? --msIndex : msIndex;	
			consoleTopDisplayChanged = true;
		}; // decrement ms
		
		if (GUI.Button (ResizeGUI (new Rect (1080 - circleXOffset, 416 - circleYoffset, 48, 54)), GUIContent.none, xRayCircleBGUIStyle)) {
			msIndex = msIndex < ms.Length-1 ? ++msIndex : msIndex;
			consoleTopDisplayChanged = true;
		}; // increment ms

		// top display panel
		if (powerOn) {
			//if(topTextChanged.Length == 0) {

			topDisplayText = topDisplayFormat(KVs[KVsIndex], mAs[mAsIndex], ms[msIndex]);
		
				GUI.Box (ResizeGUI (new Rect (488, 142, 628, 189)), topDisplayText, xRayPanelGUIStyle);
				Rect bottomPanel = new Rect(488,470,628, 335);
				
				if(expError) {
					GUI.Box (ResizeGUI(bottomPanel), "\n  Please place a cassette on\n  the table or in the tray.", XRayBottomPanelGUIStyle);
				} else {	
					GUI.Box (ResizeGUI(bottomPanel), bottomDisplayText, XRayBottomPanelGUIStyle);
				}
				if(exposureReady == 1) {
					if(flashInterval++ > 100) {
						Debug.Log ("interval : "+flashInterval+ " shown: "+circleShown);
						
						flashInterval = 0;
						circleShown = !circleShown;
					}
				} 

					if(circleShown) {
						GUI.Box (ResizeGUI (new Rect(595,168, 41,45)), circleLight); 
					}

				GUI.Box (ResizeGUI (new Rect(825,168, 60,39)), dashLight);

			//Debug.Log ("Eclicks: "+exposureReady);

			if (exposureReady > 0) {
				if(exposureReady == 1) {
					GUI.Box (ResizeGUI (new Rect (1256, 353, 143, 171)), "Press Again to Expose Cassette", XRayExposureButtonStyle);
				}

				if(exposureReady >= 2) {
						if(doorInterface.isOpen) {
							AppController.instance.score = AppController.instance.score - 100;
							if(null != audio) audio.Play();
						} 
						saveExposureSettings = true;
						exposureReady = 0;
				} 
			}

			if (GUI.Button (ResizeGUI (new Rect (1456, 353, 143, 171)), GUIContent.none, XRayExposureButtonStyle)) {
				exposureReady += 1;
				circleShown = true;
			};
		} else {
				GUI.Box (ResizeGUI (new Rect (488, 142, 628, 189)), GUIContent.none, xRayPanelGUIStyle); // top display panel
				GUI.Box (ResizeGUI(new Rect(488,470,628, 335)), GUIContent.none, XRayBottomPanelGUIStyle); // bottom display panel
			//resetToTopMenu ();
		}

		if (GUI.Button (new Rect(250, 25, 180, 25), "Close X-Ray Console", AppController.instance.buttonStyle)) {
			showPanel (false);
		}

	}

	string lastToggleKey = "";

	void setExclusiveToggle(IDictionary<string, bool> toggles, string myKey) {

		List<string> list = new List<string>(toggles.Keys);

		foreach (string key in list) {
			if(key.Equals (myKey)) {
				toggles[key] = true;
				if(! lastToggleKey.Equals (key)) {
					consoleTopDisplayChanged = false;
				}

				lastToggleKey = key;
			} else {
				toggles[key] = false;
			}
		}
	
		setPowerIndexes(myKey); // indexes for KVs, mAs, ms settings on top panel

	return;
	}

	string topDisplayFormat(double KVs, double mAs, double ms) {
		return "\n\n"+KVs + " KVs "+mAs + " mAs " + (ms > 999 ? ms/1000 : ms) + (ms > 999 ? " s" : " ms");
	}
	
	// add asterix
	void addAsterix() {
		if (lastKey.Length == 0)
						return;

		bottomDisplayText = bottomDisplayText.Replace("*", "");
		bottomDisplayText = bottomDisplayText.Replace(lastKey, lastKey+"*");
	}

	// reverse index lookup
	void setPowerIndexes(string key) {

		if (lastKey.Length > 0) {
			if(key.Equals (lastKey)) 
				return;
		}

		lastKey = key;

		int kvi = 0, mas = 0, _ms = 0;
		for(int i=0; i < KVs.Length; i++) {
			if(KVs[i] == presets[key][0]) {
				kvi = i; 
				break;
			}
		}	

		for(int i=0; i < mAs.Length; i++) {
			if(mAs[i] == presets[key][1]) {
				mas = i; 
				break;
			}
		}	

		for(int i=0; i < ms.Length; i++) {
			if(ms[i] == presets[key][2]) {
				_ms = i;
				break;
			}
		}	

		KVsIndex = kvi;
		mAsIndex = mas;
		msIndex = _ms;
	
		//addAsterix();
	}

	string setPanelBrackets(string panelText, string label) {
		panelText = panelText.Replace("[", "");
		panelText = panelText.Replace("]", "");
		panelText = panelText.Replace(label, "["+label+"]");

		return panelText;
	}

	Rect ResizeGUI(Rect _rect)
	{
		float FilScreenWidth = _rect.width / 1636;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = _rect.height / 1047;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (_rect.x / 1636) * Screen.width;
		float rectY = (_rect.y / 1047) * Screen.height;

		int topFontSize = (int) ((double)(Screen.height + Screen.width) * 0.015);
		int bottomFontSize = (int) ((double)(Screen.height + Screen.width) * 0.0105);

		if (bottomFontSize < 35) {
			XRayBottomPanelGUIStyle.font = smallBottomFont;
		}

		XRayBottomPanelGUIStyle.fontSize = bottomFontSize;
		xRayPanelGUIStyle.fontSize = topFontSize;

		return new Rect(rectX,rectY,rectWidth,rectHeight);
	}

	private bool saveExposureSettings = false;
	
	public static string ScreenShotName(int width, int height) {
		return string.Format("{0}/screenshots/xray_{1}x{2}_{3}.png", 
		                     Application.dataPath, 
		                     width, height, 
		                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
	
	//float planeScale = 10.0f;
	GameObject cas;

	void LateUpdate() {
	
		//takeHiResShot |= Input.GetKeyDown("k");

		if (saveExposureSettings) {
			cas = findClosestObject (menu.collumation, "cassette");

			float dist = Vector3.Distance(menu.collumation.transform.position, cas.transform.position);
			if(dist > 1.5f) {

				expError = true;
			} else {
				AppController.instance.setExposure(cas.GetInstanceID(), new double[] {KVs[KVsIndex], mAs[mAsIndex], ms[msIndex]});
				saveExposureSettings = false;
				expError = false;
			}

		}
	}

	// not implemented
	/* this is if we want to save an image of the x-ray */
	private IEnumerator grabCollumation() {

		AppController.instance.NoGUI = true;
		Camera[] cams = cas.GetComponentsInChildren<Camera>();
		Camera invertedCamera = null;

		if (cams.Length == 1) {
						invertedCamera = cams [0];
						invertedCamera.enabled = true;
		} else {
			AppController.instance.NoGUI = false;
			Debug.Log ("No camera found!!");
		}

		yield return new WaitForEndOfFrame();

		if(invertedCamera==null) yield return null;

		Transform pos = collumationCamera.transform;
		collumationCamera.transform.position = aperturePosition.position;
		collumationCamera.transform.rotation = aperturePosition.rotation;
		
		float camWidth = invertedCamera.pixelWidth;
		float camHeight = invertedCamera.pixelHeight;	

		RenderTexture rt = new RenderTexture((int)camWidth,(int)camHeight, 24);
		invertedCamera.targetTexture = rt;

		Texture2D screenShot = new Texture2D((int)invertedCamera.pixelWidth, (int)invertedCamera.pixelHeight, TextureFormat.RGB24, false);
		
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, invertedCamera.pixelWidth, invertedCamera.pixelHeight),0,0);
		
		invertedCamera.Render();
		invertedCamera.targetTexture = null;
		RenderTexture.active = null; // JC: added to avoid errors
		Destroy(rt);
		byte[] bytes = screenShot.EncodeToPNG();

		WWWForm form = new WWWForm();
		string hash = AppController.instance.hash;

		form.AddField("type", "xray");
		form.AddField("userhash", hash);
		form.AddBinaryData("fileUpload", bytes, "xrayShot.png", "image/png");

		// Upload to a cgi script
		WWW w = new WWW(AppController.instance.screenShotURI, form);

		yield return w;

		if (w.error != null) {
						print (w.error);
						DebugConsole.Log ("SCREENSHOT ERROR: " + w.error);
		} else {
						DebugConsole.Log ("Finished Uploading Screenshot "+w.text);	
		}

		saveExposureSettings = false;
		
		collumationCamera.transform.position = pos.position;
		collumationCamera.transform.rotation = pos.rotation;
			
		invertedCamera.enabled = false;
		AppController.instance.NoGUI = false;
		collumationCamera.enabled = true;

		cleanupAfterXRayShot ();
	}

	private GameObject findClosestObject(GameObject closestTo, string tag) {

			GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
			GameObject closestObject = null;

			foreach (GameObject obj in objectsWithTag)
			{
				if(closestObject == null)
				{
					closestObject = obj;
				}
				//compares distances
				if(Vector3.Distance(closestTo.transform.position, obj.transform.position) <= Vector3.Distance(closestTo.transform.position, closestObject.transform.position))
				{
					closestObject = obj;
				}
			}
			
		return closestObject;
	}

	private void setupXRayShot() {
		foreach(InventoryItem item in invItems) {
			Renderer[] rends = item.gameObject.GetComponentsInChildren<Renderer>();
			item.enabled = false;
			
			if(rends.Length > 0) {
				foreach(Renderer rendara in rends) {
					
					if(!item.gameObject.tag.Equals("phantom")) {
						rendara.enabled = false;
					} 
					
					if(rendara.gameObject.tag.Equals ("skin")) {
						origFlesh = rendara.material;
						rendara.material = xrayFlesh;
					}
					
					if(rendara.gameObject.name.Equals ("bones")) {
						origMaterial = rendara.material;
						rendara.material = xrayMaterial;
					}
					
				}
				
			}
		}
	}

	private void cleanupAfterXRayShot() {
		Debug.Log ("CLEANING UP");
		foreach(InventoryItem item in invItems) {
			item.enabled = true;
			
			Renderer[] rends = item.gameObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer rendara in rends) {
				if(origMaterial != null) {
					if(rendara.gameObject.name.Equals ("bones")) {
						Debug.Log ("applying bone texture");
						rendara.material = origMaterial;
					}
					
					if(rendara.gameObject.tag.Equals ("skin")) {
						Debug.Log ("applying skin texture");
						rendara.material = origFlesh;
					}
				} else {Debug.Log ("WAS NULL!!!");}
				rendara.enabled = true;
			}
		}
	}
}
