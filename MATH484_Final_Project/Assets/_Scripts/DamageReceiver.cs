using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour {

	public Heuristic heuristic;
	public float Durability = 10f;
	public List<string> DamagingTags;
	public bool isPig = false;

	void OnCollisionEnter2D(Collision2D col){
		
		if (DamagingTags.Contains(col.gameObject.tag)) {
			Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D> ();
			float mass = rb2d.mass * 10f;
			//Debug.Log (rb2d.velocity.magnitude);
			float damage = rb2d.velocity.magnitude * mass;
			Durability -= damage;
			if (Durability < 0.0f) {
				if (isPig) {
					Debug.Log ("Pig died");
					heuristic.AddScore (10);
				}
				gameObject.SetActive (false);
			} else {
				if (isPig)
					heuristic.AddScore (damage);
			}
		}
	}
}
