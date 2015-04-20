using UnityEngine;
using System.Collections;

public class CassetteInsert : MonoBehaviour {

<<<<<<< .merge_file_lmJ3Rs
	private bool showText = false;
=======
	//private bool showText = false;
>>>>>>> .merge_file_I4sOxW
	private bool optionIsOpen = false;
	private Rect optionRect;

	public GameObject cassetteInsertedEmpty;
	public GameObject cassetteParent;
	private GameObject cassette;
	private AppController app;

	public float inZLimit = 0.008f;
	public float outZLimit = -1;
	public bool useOutDiff = false;
	public string description = "This is the Bed cassette tray.  Insert cassettes here or place them on the bed.";
	public string removeMessage = "Remove the cassette from the bed tray.";
	public string buttonText = "Remove Cassette";

	bool slide = false;
	bool isIn = false;

	UnityEngine.UI.Button button1;
	UnityEngine.UI.Button button2;
	UnityEngine.UI.Text butText;
	
	Vector3 trayStartPos;
	public GameObject trayEndPos;
	// Use this for initialization

	bool b1init = false;
	bool b2init = false;

	void Start () {
		//gameObject.AddComponent<Rollover3D> ();
		app = AppController.instance;

<<<<<<< .merge_file_lmJ3Rs
		//app.actionButton1.SetActive (true);
		//app.actionButton2.SetActive (true);
=======
		b1init = app.actionButton1.activeInHierarchy;
		b2init = app.actionButton2.activeInHierarchy;
		
		if(!b1init) 
			app.actionButton1.SetActive (true);
		
		if(!b2init) 
			app.actionButton2.SetActive (true);
>>>>>>> .merge_file_I4sOxW

	 	button1 = app.actionButton1.GetComponent<UnityEngine.UI.Button> ();	
		button2 = app.actionButton2.GetComponent<UnityEngine.UI.Button> ();
		butText = button1.GetComponentInChildren<UnityEngine.UI.Text> ();

		app.actionButton1.SetActive (b1init);
		app.actionButton1.SetActive (b2init);

		trayStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (slide) {
			if(isIn) {
				gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp (transform.position.z, trayStartPos.z, Time.deltaTime * 2.0f));

				slide = transform.position.z < outZLimit;

				if(!slide) { 
					finishRemove();
					isIn = false;
				}
			} else {
				gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp (transform.position.z, trayEndPos.transform.position.z, Time.deltaTime * 2.0f));

				slide = transform.position.z > inZLimit;

				if(!slide) isIn = true;
			}
			//slide = !slide;
		}
		//if(name.Equals ("Cassette Start Pos")) 
		//DebugConsole.Log (transform.position.x);
	}

	void OnMouseEnter() {
		if (cassette == null) {
			cassette = app.getCassetteFromInv();
		} else {
			button1.gameObject.SetActive (true);
		}
						
		button1.onClick.RemoveAllListeners ();

		if (isIn) {
			app.setInfoUIText (removeMessage);
			
			button1.gameObject.SetActive (true);
<<<<<<< .merge_file_lmJ3Rs
			button1.onClick.RemoveAllListeners ();
=======

>>>>>>> .merge_file_I4sOxW
			butText.enabled = true;
			butText.text = buttonText;
			
			button1.onClick.AddListener (() => removeCassette ());
			optionIsOpen = true;
		} else if (cassette == null) {

						app.setInfoUIText (description);

				} else {
						
								optionIsOpen = true;
								app.setInfoUIText ("Insert the " + cassette.GetComponent<InventoryItem> ().displayName);
								button1.gameObject.SetActive (true);
<<<<<<< .merge_file_lmJ3Rs
								button1.onClick.RemoveAllListeners ();
=======
								//button1.onClick.RemoveAllListeners ();
>>>>>>> .merge_file_I4sOxW
								butText.enabled = true;
								butText.text = "Insert Cassette";
						

								button1.onClick.AddListener (() => insertCassette ());
								button2.gameObject.SetActive (false);
				}
	}

	void OnMouseExit() {
		if (cassette == null)
						return;
		if (!optionIsOpen) {
						button1.gameObject.SetActive (false);
						button2.gameObject.SetActive (false);
				}
	}
	
	void insertCassette() {
		if (cassette == null)
						return;

		optionIsOpen = false;
<<<<<<< .merge_file_lmJ3Rs
		showText = false;
=======
		//showText = false;
>>>>>>> .merge_file_I4sOxW
		InventoryItem item = cassette.GetComponent<InventoryItem> ();
		app.inventory.removeItem(item);
		
		cassette.transform.position = cassetteInsertedEmpty.transform.position;
		cassette.transform.rotation = cassetteInsertedEmpty.transform.rotation;
		cassette.rigidbody.isKinematic = true;
		cassette.rigidbody.Sleep();
		cassette.SetActive(true);

		app.refreshInventoryPanel ();
		item.hideObjectFromUser(false);

		cassette.transform.parent = gameObject.transform;

		slide = true;

		app.closeInfoPanel ();


		//isIn = false;
	}

	void removeCassette() {
		if (cassette == null)
			return;

		slide = true;
	}

	void finishRemove() {
		InventoryItem item = cassette.GetComponent<InventoryItem> ();
		//app.inventory.addItem (item);
		item.pickThisUp ();
		
		cassette.rigidbody.isKinematic = false;
		
		//item.enabled = true;
		
		//app.refreshInventoryPanel ();
		//item.hideObjectFromUser(true);
		//item.toggleMyPointer (false);
		cassette.transform.parent = cassetteParent.transform;
		
		cassette = null;
		app.closeInfoPanel ();
	}

}
