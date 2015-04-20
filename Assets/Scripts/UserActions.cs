using UnityEngine;
using System.Collections;

public class UserActions  {

	public UserActions() {
	}
	
	public bool perform(string action) {
		bool acted = false;
		if (action == "plate shelf") {
			//AppController.instance.plates = new InventoryItem(GameObject.FindGameObjectsWithTag ("plate");
				if (AppController.instance.plates.Length > 0) {
						if (acted = GUILayout.Button ("Get a plate from the plate shelf.")) {
								AppController.instance.inventory.addItem (AppController.instance.plates [0]);
								AppController.instance.plates [0].gameObject.SetActive (false);
						}
				}
		}

		if (action == "sink") {
			GUILayout.Button ("Wash your hands");
		}

		if (action == "standing plate") {
			GUILayout.Button ("Use the standing plate holder.");
		}
		
		/*if (action == "x-ray table") {

		}*/

		return acted;
	}
}
