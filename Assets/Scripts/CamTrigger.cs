using UnityEngine;
using System.Collections;

public class CamTrigger : MonoBehaviour {

		private int enterCount = 0;
		private bool camOn = false;
		//private static Main.instance Main.instance;

		public string context = "";
		public int camIndex;

		private static FirstPersonActions menuActions;
		// Use this for initialization
		void Start () {
			menuActions = GameObject.FindObjectOfType<FirstPersonActions> ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void mainFunc(int id) {
			//menuActions.SetCamTrigger (this);
			//menuActions.Layout (context); 
		}

		void OnGUI() {
			if (camOn) {
			//	GUI.Window (0, new Rect (20, 50, 300, 100), mainFunc, "What would you like to do, "+MessageScript.instance.screen_name+"?");
			}
		}
		void OnTriggerEnter() {
			if (AppController.instance.cameras[AppController.MAIN_CAMERA] == null)
					return;
			
			if (enterCount++ < 1) {
			/*	CaptainKirk.instance.cameras[CaptainKirk.MAIN_CAMERA].enabled = false;
				CaptainKirk.instance.cameras[camIndex].enabled = true;
				CaptainKirk.instance.fpController.SetActive (false);
				camOn = true;*/
			}
		}
		
		void OnTriggerExit() {
			enterCount = 0;
			camOff ();
		}
		
		public void camOff() {
		/*	CaptainKirk.instance.cameras[camIndex].enabled = false;
			CaptainKirk.instance.fpController.SetActive (true);
			CaptainKirk.instance.cameras[CaptainKirk.MAIN_CAMERA].enabled = true;
			camOn = false;*/
		}
	}

