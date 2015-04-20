using UnityEngine;
using System.Collections;

public class InventoryGroup : MonoBehaviour {

	InventoryItem[] items;

	public string displayName = "not set";
	public Sprite sprite;
	public string rolloverText = "";
	
	void Start() {
		// disable collider on all elenents except first
		InventoryItem[] items = GetComponentsInChildren<InventoryItem> ();

		for (int i = 1; i<items.Length; i++) {
			items[i].setIgnoreCollider(true);
		}

	}
}
