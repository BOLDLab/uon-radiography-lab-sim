using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MouseOverSlide : MonoBehaviour {

	Animator anim;
	public bool isDown = false;
	public GameObject srcButton;
	private Image img;
	public Sprite slideUp, slideDown;

	public string upAnim;
	public string downAnim;
	
	void Start() {
		anim = GetComponent<Animator> ();
		img =  srcButton.GetComponentInChildren<Image>();
		if (isDown) {
			img.sprite = slideUp;
		} else {
			img.sprite = slideDown;		
		}

	}

	public void doPlay() {
		if (img == null)
						return;

		Debug.Log ("Playing anim");

		if (isDown) {
			img.sprite = slideDown;
			anim.Play (upAnim);
		} else {
			img.sprite = slideUp;
			anim.Play (downAnim);
		}

		isDown = !isDown;
	}
}
