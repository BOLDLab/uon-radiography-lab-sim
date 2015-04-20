using UnityEngine;
using System.Collections;

public class CollumationController : MonoBehaviour {
	
	private Rect hSliderRect = new Rect (10, 135, 280, 20);
	private Rect vSliderRect = new Rect (10, 145, 20, 280);

	public GameObject collumation;
	public GameObject uprightCollumation;
	public static bool collOn = false;

	public static float xs;
	public static float ys;
	public static float prexs;
	public static float preys;

	private static float lastXPos = -10000f;
	
	private static short offsetX = 0;
	private static float offsetPos = 0;
	private Quaternion origin;
	private Vector3 originV;
	private Quaternion uprightOrigin;

	private bool rotatedVert = false;
	private static bool takeHiResShot = false;

	public static bool apertureActive = false;
	// Use this for initialization
	void Start () {
		xs = collumation.transform.localScale.x;
		ys = collumation.transform.localScale.y;

		origin = new Quaternion (collumation.transform.rotation.x, collumation.transform.rotation.y, collumation.transform.rotation.z, collumation.transform.rotation.w);
		originV = new Vector3 (collumation.transform.position.x, collumation.transform.position.y, collumation.transform.position.z);
		
		uprightOrigin = new Quaternion (uprightCollumation.transform.rotation.x, uprightCollumation.transform.rotation.y, 
		                                uprightCollumation.transform.rotation.z, uprightCollumation.transform.rotation.w);

		showCollumation (uprightCollumation, false);
	}
	
	void Update() {
		showCollumation (collumation, collOn);
	}
	// Update is called once per frame
	void OnGUI () {
		if (collOn && collumation.renderer.isVisible && apertureActive) {

						/*if (Event.current != null) {
								if (Event.current.type == EventType.MouseDown) {
										if (hSliderRect.Contains (Event.current.mousePosition) || vSliderRect.Contains (Event.current.mousePosition)) {
												Camera.main.GetComponent<MoveCamera> ().enabled = false;
										} 
								}
						}*/
						Vector3 v3;
						
						if(Camera.main == null) {
							v3 = AppController.instance.thirdPersonCamera.WorldToScreenPoint(collumation.transform.position);
						} else {
							v3	= Camera.main.WorldToScreenPoint (collumation.transform.position);
						}			
						hSliderRect = new Rect (Mathf.Abs (v3.x) - 140, Mathf.Abs (v3.y) - 140, 280, 20);
						vSliderRect = new Rect (Mathf.Abs (v3.x) - 140, Mathf.Abs (v3.y) - 120, 20, 280);
			
						prexs = xs;
						preys = ys;
			
						xs = GUI.HorizontalSlider (hSliderRect, xs, 1.0f, 50.0f);
						ys = GUI.VerticalSlider (vSliderRect, ys, 1.0f, 50.0f);
		
						//if (collOn) {
						//showCollumation (collumation, true);
				
						rotatedVert = collumation.transform.position.x < -60;
				
						if (rotatedVert) {
					
								if (offsetX > 2)
										offsetX = 0;
					
								if (rotatedVert) {
										offsetX += 1;
								}
					
								if (collumation.transform.position.y < 15.94) {
										rotatedVert = false;
										collumation.transform.position = new Vector3 (1.79f, originV.y, collumation.transform.position.z);
										collumation.transform.rotation = new Quaternion (origin.x, origin.y, origin.z, origin.w);
										offsetX = 0;
										lastXPos = -10000;
								}
					
						}
				
						//} /*else {
						//	showCollumation (collumation, false);
						//	showCollumation (uprightCollumation, false);		
						//	}
			
						if (offsetX == 1 && collumation.transform.position.y > offsetPos) { 
								offsetPos = collumation.transform.position.y - 0.005f;
						} else if (offsetX == 1 && collumation.transform.position.y < offsetPos) {
								offsetPos = collumation.transform.position.y + 0.005f;
						} else {
								offsetPos = collumation.transform.position.y;
						}
			
						if (XRayMachineMenu.buttonStr == "Return to normal view") {
								if (collOn) {
										if (prexs != xs) {
												xs += 0.2f;
										}
										if (preys != ys) {
												ys += 0.2f;
										}
					
										if (rotatedVert) {
												collumation.transform.rotation = new Quaternion (uprightOrigin.x, uprightOrigin.y, uprightOrigin.z, uprightOrigin.w);
												collumation.transform.position = new Vector3 (1.06f, offsetPos, collumation.transform.position.z);
										} else {
												collumation.transform.rotation = new Quaternion (origin.x, origin.y, origin.z, origin.w);
												collumation.transform.position = new Vector3 (collumation.transform.position.x, originV.y, collumation.transform.position.z);
										}
								} 
						} else {
								if (rotatedVert) {
										collumation.transform.rotation = new Quaternion (uprightOrigin.x, uprightOrigin.y, uprightOrigin.z, uprightOrigin.w);
										collumation.transform.position = new Vector3 (1.06f, offsetPos, collumation.transform.position.z);
										lastXPos = collumation.transform.position.x;
										//collumation.transform.position = new Vector3 (collumation.transform.position.x, collumation.transform.position.y, collumation.transform.position.z);
								} else {
									//DebugConsole.Log(collumation.transform);
										collumation.transform.rotation = new Quaternion (origin.x, origin.y, origin.z, origin.w);
										if (lastXPos == -10000f) {
												collumation.transform.position = new Vector3 (collumation.transform.position.x, originV.y, collumation.transform.position.z);
										} else {
												collumation.transform.position = new Vector3 (lastXPos, originV.y, collumation.transform.position.z);
												lastXPos = -10000f;
										}
								}
						}
	
						collumation.transform.localScale = new Vector3 (xs, ys, collumation.transform.localScale.z);
				}
	}

	void showCollumation(GameObject _obj, bool show) {
		_obj.SetActive(show);
		_obj.renderer.enabled = show;
	}

	public static void takeScreenshot() {
		DebugConsole.Log ("taking shot...");
		takeHiResShot = true;
	}
	
	void LateUpdate() {
		if (takeHiResShot) {
			AppController.instance.NoGUI = true;
			UIDisplayControl.instance.UIEnabled (false);

			StartCoroutine(takeCameraAngleShot());
			takeHiResShot = false;
		}
	}
	
	private IEnumerator takeCameraAngleShot() {
	
		yield return new WaitForEndOfFrame();

		Camera myCam = Camera.main;
		if (myCam == null || !myCam.enabled) {
			myCam = AppController.instance.thirdPersonCamera;
		} 

		if(myCam == null) yield return null;
		
		float camWidth = myCam.pixelWidth;
		float camHeight = myCam.pixelHeight;	
		
		RenderTexture rt = new RenderTexture((int)camWidth,(int)camHeight, 24);
		myCam.targetTexture = rt;
		
		Texture2D screenShot = new Texture2D((int)myCam.pixelWidth, (int)myCam.pixelHeight, TextureFormat.RGB24, false);
		
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, myCam.pixelWidth, myCam.pixelHeight),0,0);

		myCam.Render();
		myCam.targetTexture = null;
		RenderTexture.active = null; // JC: added to avoid errors
		Destroy(rt);
		byte[] bytes = screenShot.EncodeToPNG();
		
		WWWForm form = new WWWForm();
		string hash = AppController.instance.hash;

		form.AddField ("type", "angle");
		form.AddField("userhash", hash);
		form.AddBinaryData("fileUpload", bytes, "angle.png", "image/png");
		
		// Upload to a cgi script
		WWW w = new WWW(AppController.instance.screenShotURI, form);
		
		yield return w;
		
		if (w.error != null) {
			print (w.error);
			DebugConsole.Log ("SCREENSHOT ERROR: " + w.error);
		} else {
			DebugConsole.Log ("Finished Uploading Screenshot "+w.text);	
		}

		AppController.instance.NoGUI = false;
		UIDisplayControl.instance.UIEnabled (true);
	}
	

}

