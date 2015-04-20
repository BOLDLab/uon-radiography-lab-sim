using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour 
{
	//
	// VARIABLES
	//
	
	public float turnSpeed = 1.0f;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 1.0f;		// Speed of the camera when being panned
	public float zoomSpeed = 1.0f;		// Speed of the camera going back and forth

	public float maxYRotation;
	public float minYRotation;

	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?
	private bool isZooming;		// Is the camera zooming?
	
	private float lastMouseX = 0;

	//
	// UPDATE
	//
	
	void Update () 
	{
		if (!this.enabled)
						return;

		if (!Input.GetKey (KeyCode.LeftControl))
						return;

		// Get the left mouse button
		if(Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}
		
		// Get the right mouse button
	/*	if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}
		
		// Get the middle mouse button
		if(Input.GetMouseButtonDown(2))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isZooming = true;
		}*/
		
		// Disable movements on button release
		if (!Input.GetMouseButton(0)) isRotating=false;
		if (!Input.GetMouseButton(1)) isPanning=false;
		if (!Input.GetMouseButton(2)) isZooming=false;



	// Rotate camera along X and Y axis

		if (isRotating)
		{
			AppController.instance.setCameraLook(null);

			Vector3 pos = this.camera.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			/*if(AppController.instance.lastLookedInfo != null && camera.fieldOfView != AppController.instance.lastLookedInfo.fieldOfView) {
				this.camera.fieldOfView = Mathf.Lerp (this.camera.fieldOfView, AppController.instance.lastLookedInfo.fieldOfView, Time.deltaTime * AppController.instance.lerpFoVSpeed);
			}*/

			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}
		
		// Move the camera on it's XY plane
		if (isPanning)
		{
			Vector3 pos = this.camera.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			
			Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
			transform.Translate(move, Space.Self);
		}
		
		// Move the camera linearly along Z axis
		if (isZooming)
		{
			Vector3 pos = this.camera.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			
			Vector3 move = pos.y * zoomSpeed * transform.forward; 
			transform.Translate(move, Space.World);
		}
	}


	private int getDirection() {

		if(Camera.main.WorldToScreenPoint(Input.mousePosition).x > lastMouseX) {
			return 1;
		}
		
		if(Camera.main.WorldToScreenPoint(Input.mousePosition).x < lastMouseX) {
			return 2;
		}
		return 0;
	}
}
