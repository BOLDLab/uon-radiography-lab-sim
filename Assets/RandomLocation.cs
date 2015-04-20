using UnityEngine;
using System.Collections;

public class RandomLocation : MonoBehaviour {

	public Vector3 min;
	public Vector3 max;

	public bool freezeX = false;
	public bool freezeY = false;
	public bool freezeZ = false;

	private float x;
	private float y;
	private float z;

	private float ox = 0;
	private float oy = 0;
	private float oz = 0;

<<<<<<< .merge_file_dRKZPP
	private int count = 0;
=======
	//private int count = 0;
>>>>>>> .merge_file_J2myZW
	// Use this for initialization
	void Start () {
		ox = transform.position.x;
		oy = transform.position.y;
		oz = transform.position.z;

		x =  Random.Range(min.x,max.x);		
		y =  Random.Range (min.y, max.y);
		z =  Random.Range (min.z, max.z);

		transform.rigidbody.position = new Vector3 (freezeX ? ox : x, freezeY ? oy : y, freezeZ ? oz : z);

		transform.rigidbody.rotation = Quaternion.Euler(new Vector3(Random.Range (1,360),Random.Range (1,360),Random.Range (1,360)));
	}
	
	// Update is called once per frame
	void Update () {
		/*if (count++ == 100) {
			count = 0;
			x =  Random.Range(min.x,max.x);		
			y =  Random.Range (min.y, max.y);
			z =  Random.Range (min.z, max.z);
			
			transform.rigidbody.position = new Vector3 (freezeX ? ox : x, freezeY ? oy : y, freezeZ ? oz : z);
			
			transform.rigidbody.rotation = Quaternion.Euler(new Vector3(Random.Range (1,360),Random.Range (1,360),Random.Range (1,360)));
		}*/
	}
}
