using UnityEngine;
using System.Collections;

public class MouseMoveMe : MonoBehaviour {

	//private float mouseYaw;
	//private float mousePitch;
	//private Vector3 moveDirection;
	public float speed = 3.0f;
	public Camera myCamera;
	public MoveCamera moveCamera;

	private AudioSource audioPutDown;

//	private bool activated = false;

	//Vector3 targetFlyRotation = Vector3.zero;
	public float minAngle = 0;
	public float maxAngle = 359;

//	private float baseAngle = 0.0f;

	bool doMove = false;
	TouchRotate touchRotate;
	InventoryItem item;
	
	private AppController app;

	// Use this for initialization
	void Start () {
		app = AppController.instance;
		touchRotate = GetComponent<TouchRotate> ();
		item = GetComponent<InventoryItem> ();
	}

	public void setDropAudio(AudioSource audio) {
		audioPutDown = audio;
	}

	public void setDoMove(bool move) {
		doMove = move;
	}
	// Update is called once per frame
	void Update () {
		if (doMove) {

			//if(rigidbody != null) 
				//rigidbody.isKinematic = true;

			Vector3 v3 = Input.mousePosition;
			v3.z = 1.0f;
			//screenV3 = v3;
			
			v3 = Vector3.Lerp (gameObject.transform.position, Camera.main.ScreenToWorldPoint (v3), Time.deltaTime * 2.0f);
			
			app.smoothLook(v3);
			
			transform.position = v3;
			//lastV3 = v3;
			app.usingTouchRotate = true;
		
			if(Input.GetMouseButton(0)) {
				doMove = false;
				audioPutDown.Play ();
				app.mtl.enabled = true;
				
				if(touchRotate != null)
					touchRotate.enabled = true;
				
				if(rigidbody != null) 
					rigidbody.isKinematic = false;

				item.enabled = true;
				item.toggleMyPointer (true);
				app.movingObject = false;

				Destroy (this);
				app.usingTouchRotate = false;
			}
		}
	}

}
