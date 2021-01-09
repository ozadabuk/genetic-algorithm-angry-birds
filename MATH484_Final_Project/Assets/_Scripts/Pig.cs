using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour {

	public Heuristic heuristic;
	public float Durability = 10f;
	public bool activated = false;
	public Rigidbody2D pigRigidbody;

	void Start(){
		pigRigidbody = GetComponent<Rigidbody2D> ();
		Invoke ("Activate", 1.0f);
	}

	void Activate(){
		activated = true;
		pigRigidbody.simulated = true;
	}

	void OnCollisionEnter2D(Collision2D col){

		if ((col.gameObject.tag == "Bird" || col.gameObject.tag == "Block") && activated) {
			
			Rigidbody2D colRigidbody = col.gameObject.GetComponent<Rigidbody2D> ();

			float mass = colRigidbody.mass * 10f;
			float damage = colRigidbody.velocity.magnitude * mass;

			if (damage > 1) {
				ApplyDamage (damage);
			}

		} else if(col.gameObject.tag == "Ground" && activated){
			
			//float mass = pigRigidbody.mass * 100f;
			//float damage = pigRigidbody.velocity.magnitude * mass;
			//Debug.Log ("Velocity: " + pigRigidbody.velocity.magnitude + "  Damage: " + damage);
			//if (damage > 1) {
				ApplyDamage (3);
			//}
		}
	}

	void ApplyDamage(float dmg){
		Durability -= dmg;
		if (Durability < 0.0f) {
			//Debug.Log ("Pig died");
			heuristic.AddScore (10);
			heuristic.PigKilled ();
			gameObject.SetActive (false);
		} else {
			heuristic.AddScore (dmg);
		}
	}
}
