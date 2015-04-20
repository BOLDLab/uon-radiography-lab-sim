using UnityEngine;
using System.Collections;

public class ExperienceServer : MonoBehaviour {
	// singleton
	public static ExperienceServer instance;
	
	void Awake() {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		logExperience ("launched", "launched simulation", "Launched Unity3D X-Ray simulation", "Launched Unity3D X-Ray simulation through a web browser");
		Application.ExternalCall ("startMessageInterface");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void logExperience(string verb, string display_verb, string name, string description){
		Application.ExternalCall ("LogExperience", verb, display_verb, name, description);
	}
	
}
