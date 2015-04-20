using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : ArrayList
{	
	public short maxItems = 6;

	public IDictionary<string, int> smalls = new Dictionary<string, int>(); 

	private ArrayList purse = new ArrayList();

	public Inventory() {
		//smalls = new int[maxItems]; 
	}

	public void addItem(InventoryItem _obj) {
		//Debug.Log ("Add item: "+_obj.displayName);
		if (_obj.isOneOf > 0 && smallItemCount(_obj) == -1) {
			Add (_obj);
			purse.Add(_obj);
			smalls[_obj.displayName] = 1;
		} else if (_obj.isOneOf > 0) {
			if(smalls[_obj.displayName] < _obj.isOneOf) {
				smalls[_obj.displayName] = smalls[_obj.displayName] + 1;
				purse.Add(_obj);
			}
		} else {
				Add (_obj);
				smalls[_obj.displayName] = 0;
		}
	}

	public void removeItem(InventoryItem _obj) {
		if(!Contains(_obj)) return;

		if (_obj.isOneOf > 0) {
			if(!purse.Contains(_obj)) return;

			if(--smalls [_obj.displayName] == 0) {
				Remove (_obj);
				purse.Remove(_obj);
			}
		} else {
			Remove (_obj);
		}
	}

	public bool containsByTag(string tag) {
		object[] items = this.ToArray ();

		foreach(InventoryItem item  in items) {
			if(tag.Equals(item.gameObject.tag)) {
				return true;
			}
		}

		return false;
	}

	public GameObject getFirstObjectOfType(string tag) {
		object[] items = this.ToArray ();
		
		foreach(InventoryItem item  in items) {
			if(tag.Equals(item.gameObject.tag)) {
				return item.gameObject;
			}
		}
		
		return null;
	}

	public int smallItemCount(InventoryItem _obj) {
		if(!smalls.ContainsKey (_obj.displayName) || _obj.isOneOf == 0) return -1;
	
		return smalls [_obj.displayName];
	}

	/*public void setSmallItemCount(InventoryItem _obj, int _count) {
		if (_obj.isOneOf == 0)
			return;

		smalls [_obj.displayName] = _count;
	}*/

	public int count() {
		return this.Count;
	}

	public bool isFull() {
		return this.Count == maxItems;
	}

	public InventoryItem[] getInventoryList() {
		InventoryItem[] items = new InventoryItem[this.Count];

		int i = 0;
		foreach(InventoryItem item in this) {
			items[i++] = item;
		}

		return items;
	}
}

