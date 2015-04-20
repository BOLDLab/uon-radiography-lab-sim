using UnityEngine;
using System.Collections;

		
public class DoorInterface : MonoBehaviour {

	private JointMotor m;

	public bool isOpen = false;
	// Use this for initialization
	void Start () {
		m = new JointMotor ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openDoor() {
		if (!isOpen) { 
			m.force = 15;
			m.targetVelocity = 45;
			m.freeSpin = false;
			hingeJoint.motor = m;

			isOpen = true;
		} 
	}
	public void closeDoor() {

		if (isOpen) {
				m.force = 15;
				m.targetVelocity = -45;
				m.freeSpin = false;
				hingeJoint.motor = m;

				isOpen = false;
		} 
	}

}
