using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
		public string displayName = "not set";
		public UnityEngine.UI.Button uiButton;
		public Sprite sprite;
		public string rolloverText = "";
		public int pickUpPointId = 0;
		public int putDownPointId = 0;
		public Transform pickUpPointLocation;
		public Transform putDownPointTransform;
		public bool showText = false;
		public TouchRotate touchRotate;
		public bool lockPositionOnPutDown = false;
		public int isOneOf = 0;
		public int nthOf = -1;
		public Sprite groupSprite;
		private Rect optionRect;
		//private bool optionIsOpen = false;
		public float rayCastDistance = 2.0f;
		public int maxCarried = -1;
<<<<<<< .merge_file_zoeKsJ
		float fovTarget = 15.0f;
		float fov = 60.0f;
=======
		//float fovTarget = 15.0f;
		//float fov = 60.0f;
>>>>>>> .merge_file_3V5hAC
		
		private bool ignoreCollider = false;

		int focusPriority = 1;
		
		private IEnumerator coroutine;
		InventoryItem[] items;

		private AppController app;
		
		InventoryPointer invPointer;
		
		public void setIgnoreCollider(bool toggle) {
			ignoreCollider = toggle;
		}
		
		public bool getIgnoreCollider() {
			return ignoreCollider;
		}

		void Start ()
		{
				app = AppController.instance;
				
				rolloverText = rolloverText == "" ? displayName : rolloverText;

				touchRotate = gameObject.GetComponent<TouchRotate> ();

				if (touchRotate != null) {
						touchRotate.enabled = false;
				}
				
				if (isOneOf > 0) {
					items = gameObject.transform.parent.GetComponentsInChildren<InventoryItem> ();	
				}
		}
		
		void OnMouseEnter ()
		{
		if (ignoreCollider || !enabled || app == null)
						return;

		if (app.thirdPersonCamera != null && app.thirdPersonCamera.enabled)
			return;

		if (EventSystem.current.IsPointerOverGameObject() || CollumationController.apertureActive)
						return;
	
		if (!app.inLineOfSight (Camera.main.gameObject, gameObject, rayCastDistance)) 
						return;

		Renderer myrend = app.getRenderer (gameObject);

		if (enabled && myrend.enabled && myrend.isVisible) {
			string str = "";

			UnityEngine.UI.Text uiText = app.informationUIPane.GetComponentInChildren<UnityEngine.UI.Text> ();
		
				string goName = gameObject.name;
				if(isOneOf > 0) {
					goName = GetComponentInParent<InventoryGroup>().gameObject.name;
				}	

				if (touchRotate == null) {
					//	if (app.inventoryUIclicks == 0) {
								str = "\n\n Click to see mor information about "+goName+".";
					//	}
				}  else {
					if(isOneOf == 0) {
						str = "\n\n Click "+goName+" for further options.";
					}
				}

			string rt = rolloverText;

			if(isOneOf > 0) {
				rt = GetComponentInParent<InventoryGroup>().rolloverText;
			}

			uiText.text = rt + str;
			}
		}
		
		/*void Update() {
				
		if (!app.lerpFoVRunning)
						return;

			app.setCameraLook (transform);

			float dif = Mathf.Abs (Camera.main.fieldOfView - fov);
			MoveCamera script = Camera.main.GetComponent<MoveCamera> ();
			script.enabled = false;

		//	while (dif > 0.5f) {
					Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, fov, Time.deltaTime * app.lerpFoVSpeed);

					dif = Mathf.Abs (Camera.main.fieldOfView - fov);

					//app.lastLookedInfo.fieldOfViewyield return new WaitForEndOfFrame ();
		//	}
		
		if(dif > 0.5f) {
			script.enabled = true;
			app.lerpFoVRunning = false;
		}

		}*/

		/*IEnumerator lerpFoV(float fov) {
		app.lerpFoVRunning = true;

		app.setCameraLook (transform);

		float dif = Mathf.Abs(Camera.main.fieldOfView - fov);
		MoveCamera script = Camera.main.GetComponent<MoveCamera>();
		script.enabled = false;
			
			while(dif > 0.5f) {
				Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, Time.deltaTime * app.lerpFoVSpeed);
				
				dif = Mathf.Abs(Camera.main.fieldOfView - fov);
				
				yield return new WaitForEndOfFrame();
			}

			script.enabled = true;
			app.lerpFoVRunning = false;
		}*/
		
		void OnMouseExit() {
					
		if (ignoreCollider || !enabled || app == null)
						return;
			app.mtl.enabled = true;
			app.focusedOnItem = false;
			app.inventoryUIclicks = 0;
		}

		void OnMouseDown ()
		{
		if (ignoreCollider || !enabled || app == null)
			return;

		if (app.thirdPersonCamera.enabled)
						return;

		Renderer myrend = app.getRenderer (gameObject);
		if (!myrend.enabled || !myrend.isVisible)
						return;

		if (/*EventSystem.current.IsPointerOverGameObject() ||*/ CollumationController.apertureActive)
				return;

		int focusID = GetInstanceID ();
<<<<<<< .merge_file_zoeKsJ
		if (isOneOf > 0) {
=======
		/*if (isOneOf > 0) {
>>>>>>> .merge_file_3V5hAC
				focusID = gameObject.transform.parent.GetComponent<InventoryGroup> ().GetInstanceID ();
				app.setCameraLook (transform.parent);
		} else {
				app.setCameraLook (transform);
<<<<<<< .merge_file_zoeKsJ
		}

=======
		}*/
>>>>>>> .merge_file_3V5hAC

				// guarantee focus on this object
				if (app.inventoryUIclicks == 0) {
						app.focusID = focusID;
						app.lastFocusPriority = focusPriority;
				} else if (!app.focusID.Equals (focusID)) {
						return;	
				}

				//UnityEngine.UI.Text uiText = app.informationUIPane.GetComponentInChildren<UnityEngine.UI.Text> ();

				app.inventoryUIclicks++;

				if (app.inventoryUIclicks == 1) {
						//	camOrig = Camera.main.transform;

						string str;	

						if (null != touchRotate && isOneOf == 0) {
								str = "\n\n For further options use the buttons below or click again to choose another item.";
								displayActionButtons (true);
						} else {
								str = "\n\n Click again to pick this up.";
						}
						
						app.setInfoUIText (rolloverText + str);
						
						//coroutine = lerpFoV(fovTarget);
					
						//StartCoroutine(coroutine); 
<<<<<<< .merge_file_zoeKsJ
						fov = fovTarget;
=======
						//fov = fovTarget;
>>>>>>> .merge_file_3V5hAC
						app.mtl.enabled = false;
						app.focusedOnItem = true;
				} else if (app.inventoryUIclicks == 2) {
				
						app.inventoryUIclicks = 0;

						if (touchRotate == null) {
								if (isOneOf > 0) {
										pickUpMultiple ();
								} else {
										pickThisUp ();
								}
								//StartCoroutine(lerpFoV(fovOrig));
						} else {
								if (isOneOf > 0) {
										pickUpMultiple ();
								}
								displayActionButtons (false);
								
						}
<<<<<<< .merge_file_zoeKsJ
						if (app.lastLookedInfo) {
								//coroutine = lerpFoV(app.lastLookedInfo.fieldOfView);
								fov = app.lastLookedInfo.fieldOfView;
						} else {
								//coroutine = lerpFoV(60.0f);
								fov = 60.0f;
						}
=======
						
>>>>>>> .merge_file_3V5hAC
						app.mtl.enabled = false;
				} else {
			app.mtl.enabled = true;
				}
			//DebugConsole.Log ("After click fovTarget: " + fovTarget);
		}
		
		bool invIsFull() {
		if (app == null)
						return false;

		//UnityEngine.UI.Text uiText = app.informationUIPane.GetComponentInChildren<UnityEngine.UI.Text> ();
		if (app.inventory == null)
						return false;

		if (app.inventory.isFull ()) { 
			string str = "You don't have room for this item in your bag.";
			app.setInfoUIText(str);

			return true;
		} 
		
		
		if (maxCarried == 1 && app.inventory.containsByTag (gameObject.tag)) {
			string str = "You can only carry one " + tag + " at a time";
			app.setInfoUIText(str);

			return true;
		}		

		return false;
		}

		public void pickUpMultiple() {
		if (invIsFull())
						return;

			int index = 0;
			foreach(InventoryItem item in items) {
				item.isOneOf = items.Length;
				item.nthOf = index++;
				
				if (app.inventory.isFull () && app.inventory.smallItemCount(item) == 0) {
					app.audioNoPickUp.Play ();	
				} else {
					app.inventory.addItem (item);
					//item.gameObject.SetActive (false);
					hideObjectFromUser (true, item.gameObject);
					item.groupSprite = item.GetComponentInParent<InventoryGroup>().sprite;
				}
			}

		app.refreshInventoryPanel ();

		app.actionButton1.SetActive (false);	
		app.actionButton2.SetActive (false);	

		string str = "some";
		string nm =  GetComponentInParent<InventoryGroup>().gameObject.name;
		app.setInfoUIText ("You picked up " + str + " " + nm);
		app.inventoryUIclicks = 0;
		app.usingTouchRotate = false;
		}

		public void pickThisUp ()
		{
		if (app == null)
						return;

		if (invIsFull())
			return;

				app.inventory.addItem (this);
				app.audioPickUp.Play ();
				app.refreshInventoryPanel ();
				//optionIsOpen = false;

				app.actionButton1.SetActive (false);	
				app.actionButton2.SetActive (false);	
				
				hideObjectFromUser (true);

				app.focusedOnItem = false;

		string str = "a";
		string nm = gameObject.name;
	
		app.setInfoUIText ("You got " + str + " " + nm);
		app.inventoryUIclicks = 0;
		app.invPointer.SetActive (false);
		app.usingTouchRotate = false;

		gameObject.SetActive (false);
		}
	
		void rotateEvent ()
		{
				touchRotate.enabled = true;
				touchRotate.rotateViaUI = true;
				//optionIsOpen = false;
				enabled = false;
		}
		
		Vector3 orig;

		public void hideObjectFromUser (bool enable, GameObject _obj = null)
		{
		if (_obj == null)
						_obj = gameObject;

				Renderer[] rs = _obj.GetComponentsInChildren<Renderer> ();

				foreach (Renderer r in rs) {
					r.enabled = !enable;
				}
		}

		void displayActionButtons (bool but_enabled)
		{
				app.actionButton1.SetActive (but_enabled);		
				
				if (isOneOf == 0) {
						app.actionButton2.SetActive (but_enabled);	
				}

				if (but_enabled) {
						UnityEngine.UI.Button button1 = app.actionButton1.GetComponentInChildren<UnityEngine.UI.Button> ();	
						UnityEngine.UI.Text text1 = button1.GetComponentInChildren<UnityEngine.UI.Text> ();		
							
						string dname = this.displayName;
						
						if(isOneOf > 0) {
							dname = GetComponentInParent<InventoryGroup>().displayName;
						}

						text1.text = "Pick up " + dname;
			
						button1.onClick.RemoveAllListeners ();

						if(isOneOf > 0) {
							button1.onClick.AddListener (() => pickUpMultiple ());
						} else {
							button1.onClick.AddListener (() => pickThisUp ());
						}
						
					if(isOneOf == 0) {
						UnityEngine.UI.Button button2 = app.actionButton2.GetComponentInChildren<UnityEngine.UI.Button> ();	
						UnityEngine.UI.Text text2 = button2.GetComponentInChildren<UnityEngine.UI.Text> ();		
	
						text2.text = "Move/Rotate " + dname;		
						button2.onClick.RemoveAllListeners ();
						button2.onClick.AddListener (() => rotateEvent ());	
					}
				}
		}
		
		public void toggleMyPointer(bool toggle) {
			if (invPointer == null) {
				if(isOneOf > 0) {
					invPointer = transform.parent.gameObject.GetComponent<InventoryPointer> ();
				} else {
					invPointer = GetComponent<InventoryPointer> ();
				}
			}
			if (invPointer == null) {
				return;
			}

			invPointer.enabled = toggle;
		}

		public void makeCopy (InventoryItem item)
		{

				item.displayName = this.displayName;
				item.sprite = this.sprite;
				item.rolloverText = this.rolloverText;
				item.pickUpPointId = this.pickUpPointId;
				item.putDownPointId = this.putDownPointId;
				item.pickUpPointLocation = this.pickUpPointLocation;
				item.showText = this.showText;
				item.touchRotate = this.touchRotate;
				item.lockPositionOnPutDown = this.lockPositionOnPutDown;
				item.isOneOf = this.isOneOf;
				item.groupSprite = this.groupSprite;
		}

}
