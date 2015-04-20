using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Rollover3D : MonoBehaviour {

	Color[] colors; 
	Renderer[] renderers;
	public string action = "";
	public int camIndex;
	//public bool enabled = true;

	//public static ThirdPersonActions thirdPersonActions;
	
	// Use this for initialization
	void Start () {
		renderers = GetComponentsInChildren<Renderer>();
		colors = new Color[renderers.Length];
		//thirdPersonActions = GameObject.FindObjectOfType<ThirdPersonActions> ();

		int count = 0;
		foreach (Renderer r in renderers)
		{
			colors[count++] = r.material.color;
		}

		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseOver() {
		if (!enabled)
						return;

		if (EventSystem.current.IsPointerOverGameObject())
			return;

		foreach (Renderer r in renderers)
		{
			r.material.color = Color.white;

		}
	}
	
	void OnMouseExit() {

		int count = 0;
		foreach (Renderer r in renderers)
		{
			r.material.color = colors[count++];
		}
		
		count = 0;
	}

	void OnMouseDown() {
		if (!enabled)
			return;
		//Debug.Log ("Mouse down on " + gameObject.transform.name);
		//thirdPersonActions.perform (action, camIndex);
	}
}

