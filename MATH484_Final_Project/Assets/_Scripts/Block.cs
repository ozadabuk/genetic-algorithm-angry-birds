using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public Heuristic heuristic;
	public float Durability = 10f;
	public List<string> DamagingTags;
	public bool activated = false;

	void Start(){
		Invoke ("Activate", 1.0f);
	}

	void Activate(){
		activated = true;
	}

	void OnCollisionEnter2D(Collision2D col){

		if (DamagingTags.Contains(col.gameObject.tag) && activated) {
			Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D> ();
			float mass = rb2d.mass * 10f;
			float damage = rb2d.velocity.magnitude * mass;
			Durability -= damage;
			if (Durability < 0.0f) {
				heuristic.AddScore (2);
				gameObject.SetActive (false);
			}
		}
	}
}
