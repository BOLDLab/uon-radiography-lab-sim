using UnityEngine;
using System.Collections;

public class CassetteInsertX : MonoBehaviour {
	
	//private bool showText = false;
	private bool optionIsOpen = false;
	private Rect optionRect;
	
	public GameObject cassetteInsertedEmpty;
	public GameObject cassetteParent;
	private GameObject cassette;
	private AppController app;
	
	public float inXLimit = 0.008f;
	public float outXLimit = -1;
	public bool useOutDiff = false;
	public string description = "This is the Bed cassette tray.  Insert cassettes here or place them on the bed.";
	public string removeMessage = "Remove the cassette from the bed tray.";
	public string buttonText = "Remove Cassette";

	public GameObject scanner;

	Animator anim;

	bool slide = false;
	bool isIn = false;

	//bool finished = false;
	
	UnityEngine.UI.Button button1;
	UnityEngine.UI.Button button2;
	UnityEngine.UI.Text butText;
	
	Vector3 trayStartPos;
	public GameObject trayEndPos;

	bool b1init = false;
	bool b2init = false;
	// Use this for initialization
	void Start () {
		//gameObject.AddComponent<Rollover3D> ();
		app = AppController.instance;

		anim = scanner.GetComponent<Animator> ();
		b1init = app.actionButton1.activeInHierarchy;
		b2init = app.actionButton2.activeInHierarchy;

		if(!b1init) 
			app.actionButton1.SetActive (true);
		
		if(!b2init) 
			app.actionButton2.SetActive (true);

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
				transform.position = new Vector3(Mathf.Lerp (transform.position.x, trayStartPos.x, Time.deltaTime * 2.0f), transform.position.y, transform.position.z);
				
				slide = transform.position.x > outXLimit;
				
				if(!slide) { 
					finishRemove();
					isIn = false;
				}
			} else {
				transform.position = new Vector3(Mathf.Lerp (transform.position.x, trayEndPos.transform.position.x, Time.deltaTime * 2.0f), transform.position.y, transform.position.z);

				slide = transform.position.x < inXLimit;
				
				if(!slide) isIn = true;
			}
			//slide = !slide;
		}
		//if(name.Equals ("Cassette Start Pos")) 
			//DebugConsole.Log (transform.position.x);
	}
	
	void OnMouseEnter() {
		if (cassette == null) {
			//app.actionButton2.SetActive (true);
			cassette = app.getCassetteFromInv (); 
		} 
		if (cassette != null) { 
			button1.gameObject.SetActive (true);
		}

		app.invPointer.SetActive (false);
		button1.onClick.RemoveAllListeners ();

		if (isIn) {
			app.setInfoUIText (removeMessage);
			
			button1.gameObject.SetActive (true);

			butText.enabled = true;
			butText.text = buttonText;
			
			button1.onClick.AddListener (() => removeCassette ());
			app.insertedReaderCassette = null;
			optionIsOpen = true;
		} else if (cassette == null) {
			
			app.setInfoUIText (description);
			
		} else {
			
			optionIsOpen = true;
			app.setInfoUIText ("Insert the " + cassette.GetComponent<InventoryItem> ().displayName);
			button1.gameObject.SetActive (true);
			//button1.onClick.RemoveAllListeners ();
			butText.enabled = true;
			butText.text = "Insert Cassette";
			
			
			button1.onClick.AddListener (() => insertCassette ());
			app.insertedReaderCassette = cassette;
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

		//app.invPointer.SetActive (true);
		//cassette.GetComponent<InventoryItem> ().toggleMyPointer (true);
	}

	bool inserted = false; 

	void insertCassette() {

		if (inserted) {
			anim.Play ("move_scanner");
			inserted = false;
		} else {
			anim.Play ("move_scanner_1");
			inserted = true;	
		}

		app.closeInfoPanel ();

		StartCoroutine (afterAnimComplete());
	}
	
	void removeCassette() {
		if (cassette == null)
			return;

		app.closeInfoPanel ();

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


	IEnumerator afterAnimComplete() {

		optionIsOpen = false;
		//showText = false;
		InventoryItem item = cassette.GetComponent<InventoryItem> ();
		app.inventory.removeItem(item);
		item.gameObject.SetActive (true);
		
		cassette.transform.position = cassetteInsertedEmpty.transform.position;
		cassette.transform.rotation = cassetteInsertedEmpty.transform.rotation;
		cassette.rigidbody.isKinematic = true;
		cassette.rigidbody.Sleep();
		cassette.SetActive(true);
		
		app.refreshInventoryPanel ();
		item.hideObjectFromUser(false);
		
		cassette.transform.parent = gameObject.transform;

		yield return new WaitForSeconds(1.5f);

		slide = true;
		
		app.closeInfoPanel ();

		app.cassetteInputUI.SetActive (true);
		//anim.SetTrigger();
	}
	
}
