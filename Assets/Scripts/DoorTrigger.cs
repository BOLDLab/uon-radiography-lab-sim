using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	public DoorInterface door;
	public InventoryItem[] reqItems; 

	//public string message = "You don't have everything you need to enter the Radiography lab.";
	public string successMessage = "Great job! You found all the necessary items. Now open the door an enter.";
	public string secondaryMessage = "Enter the lab?";
	public string returnMessage = "Exit to the corridor?";

	public int scoreForCompletion = 100;
	public bool completed = false;

//	public bool showMessage = false;
	public short messageDisplayTime = 120;

/*	public GameObject entryLocation;
	public string entryButtonText = "Enter the Lab";
	public GameObject returnLocation;
	public float applyXRotation = 90;
	public string returnButtonText = "Exit the Lab";*/

	public GameObject lockBarrier;

	public bool makeClickable = false;
	
<<<<<<< .merge_file_QAxcP2
	private Information information;
=======
	//private Information information;
>>>>>>> .merge_file_SnRp0k

	//private short displayCount = 0;

	public bool requireClickToCloseDoor = false;

	// Use this for initialization
//	private int enterCount = 0;

	private AppController app;

	
	void Start() {
		app = AppController.instance;
<<<<<<< .merge_file_QAxcP2
		information = gameObject.GetComponent<Information> ();
=======
		//information = gameObject.GetComponent<Information> ();
>>>>>>> .merge_file_SnRp0k
	}
	
	void OnMouseDown() {
		if (makeClickable) {
			DebugConsole.Log ("Clicked door");
			if(hasRequiredItems()) {
				DebugConsole.Log ("Activating door");
				activateDoor(true);
			}
		}
	}

	/*void OnMouseEnter() {
		app.mouseOnOpenTrigger = door.isOpen;

		string str = door.isOpen ? "close" : "open";
		if (requireClickToCloseDoor) {
				app.setInfoUIText ("Click to " + str + " " + gameObject.name + ".");
		} else {
				if (!information.firstVisit) {
						if (hasRequiredItems ()) {
								//app.setInfoUIText (successMessage);
								checkLockState();
						} /*else {
								app.setInfoUIText (information.text);
						}
				} 
		}
		app.lastLookedInfo = information;
	}*/


	/*void OnMouseExit() {
		app.mouseOnOpenTrigger = false;
	}*/
	
	public void checkLockState() {
		//DebugConsole.Log ("In Door trigger "+requireClickToCloseDoor);
		if (!door.isOpen && hasRequiredItems()) {
			if (!requireClickToCloseDoor) {
					door.openDoor ();
					if(!completed) {
					if(lockBarrier != null) {
						activateBarrierCollider();
					}
						app.score += scoreForCompletion;
						if(null != audio) audio.Play();
						completed = true;
					}
			}
		} 
	}

	void activateBarrierCollider(bool locked = false) {
		lockBarrier.GetComponent<BoxCollider> ().enabled = locked;
		//enabled = locked;
		//gameObject.GetComponent<BoxCollider> ().enabled = false;
	}
	
	public void activateDoor(bool clicked = false) {
			
		//DebugConsole.Log ("Activate door called: " + clicked);
		if (door.isOpen) {
			if(requireClickToCloseDoor && clicked) {
				door.closeDoor ();
				activateBarrierCollider(true);	
			}
			else if(!requireClickToCloseDoor) {
				door.closeDoor();
			}

		} else if (hasRequiredItems() || reqItems.Length == 0) {
			if(requireClickToCloseDoor && clicked) { 
				door.openDoor ();
				if(lockBarrier != null) {
					activateBarrierCollider();
				}
			} 

			if(!completed) {
				if(lockBarrier != null) {
					activateBarrierCollider();
				}

				app.score += scoreForCompletion;
				if(null != audio) audio.Play();
				completed = true;
			}
		} /*else {
			displayCount = 0;
		}*/
	}

	public bool hasRequiredItems() {

		int matches = 0;
		foreach (InventoryItem item in reqItems) {	
			//Debug.Log ("Item before test: "+item);
			if(item != null && app.inventory != null) {
			//	Debug.Log ("Item inside test: "+item);
				foreach(InventoryItem invItem in app.inventory) {
						if(invItem.displayName == item.displayName) {
							matches++;
						}
				}
			}
		//	} else {Debug.Log ("Item was null"); }
		}

		//DebugConsole.Log ("Inv length: "+app.inventory.Count+"Matches = " + matches+" reqItems length: "+reqItems.Length);

		return (matches >= reqItems.Length);
	}
}
