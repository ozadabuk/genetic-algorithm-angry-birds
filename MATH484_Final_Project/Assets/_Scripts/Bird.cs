using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	public Heuristic heuristic;
	public Transform TargetCenter;
	public Rigidbody2D rgb2d;
	public Slingshot slingshot;
	public bool isTossed = false;
	public bool isStopped = false;
	public bool crashed = false;
	public bool powerActivated = false;

	public double Angle = 0f;
	public double InitialForce = 0f;
	public double PowerActivateTime = 10f;

	private double activateTimer = 0.0f;
	private double crashTimer = 0.0f;
	private double InitialDistance = 0.0f;

	void Start () {
		
	}
	
	void Update () {
		if (isTossed) {
			activateTimer += Time.deltaTime;
			if (activateTimer > PowerActivateTime) {
				if (!powerActivated && !crashed) {
					powerActivated = true;
					rgb2d.AddForce ((rgb2d.velocity.normalized * 0.5f + Vector2.right + Vector2.down * 0.1f)* 120f);
				}
			}
			if (IsBirdStopped () && crashed) {
				LoadNextBird ();
			}

			if (crashed)
				crashTimer += Time.deltaTime;
		}
	}

	public bool IsBirdStopped(){
		if (rgb2d.velocity.magnitude < 0.2f || crashTimer > 3.0f)
			return true;
		else
			return false;
	}

	public void LoadNextBird(){
		if (!isStopped) {
			isStopped = true;
			slingshot.BirdCrashed ();
		}
	}

	public void SetInitialDistance(){
		InitialDistance = Vector3.Distance (transform.position, TargetCenter.position);
	}

	void OnCollisionEnter2D(Collision2D col){

		if (!crashed) {
			crashed = true;
			double crashDistance = Vector3.Distance (transform.position, TargetCenter.position);
			double crashScore = InitialDistance - crashDistance;
			double score = crashScore * 5 / InitialDistance;
			score = Mathf.Clamp ((float)score, 0, 5);
			heuristic.AddScore (score);
			//Debug.Log ("Bird crashed");
		}
	}
}
