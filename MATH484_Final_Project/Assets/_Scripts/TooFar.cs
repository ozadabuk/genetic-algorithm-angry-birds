using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooFar : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Bird") {
			col.gameObject.SendMessage ("LoadNextBird");
			col.gameObject.GetComponent<Rigidbody2D> ().simulated = false;
		}
	}
}
