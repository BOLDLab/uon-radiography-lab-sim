using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {
	//Vector3 v3player;
	Vector3 v3origin;
	MoveToLocation mv;
	//Collision col;	
	// Use this for initialization
	void Start () {
		v3origin = transform.position;
		mv = AppController.instance.mtl;
	}

	bool colliding = false;
	float _f = 0.3f;

	// Update is called once per frame
	void Update () {
		if(mv == null) mv = AppController.instance.mtl;

		if (v3origin != null) {
			if(mv.bouncingBack) {
				mv.getAgent().transform.Translate(Vector3.back * _f);
				if(Vector3.Distance (mv.getAgent ().transform.position, mv.bounceBackPosition) < _f) {
					mv.bouncingBack = false;
				}
			}	
		}
		

	}

	//float str = 2.0f;

	void OnCollisionEnter(Collision col) {
		if(!col.gameObject.name.Equals (mv.gameObject.name)) return;

		//this.col = col;
		//Debug.Log ("Barrier: " + gameObject.name+ "go: "+col.gameObject.name);

		if (!colliding && !mv.bouncingBack) { 
			v3origin = mv.getAgent().transform.position;
			//mv.getAgent().destination = mv.bounceBackPosition;
			mv.getAgent().Stop ();
			mv.hitBarrier = true;
			colliding = true;
			mv.bouncingBack = true;
		}
	}

	void OnCollisionExit(Collision col) {
		if(!col.gameObject.name.Equals (mv.gameObject.name)) return;

		mv.hitBarrier = false;

		colliding = false;
	}
}
