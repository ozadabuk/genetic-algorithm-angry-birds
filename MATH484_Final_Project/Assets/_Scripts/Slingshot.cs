using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

	public GAManager gaManager;
	public Heuristic heuristic;
	public Rigidbody2D[] Birds;
	public Rigidbody2D[] Pigs;
	public Rigidbody2D currentBird;

	public int birdIndex = 0;
	public static int birdLimit = 0;
	public double PigMovementMag = 0.0f;
	public double[] currentChromosome = new double[9];

	public Transform Ramp;
	public CameraFollow cam;
	public LineRenderer forceLine;

	public bool isPlayerPlaying = false;
	public bool gameOver = false;
	public bool birdCrashed = false;

	void Update () {

		if (!PigsMoving ()) {
			if (birdCrashed) {
				if (birdIndex == Birds.Length - birdLimit)
					gameOver = true;
				if (gameOver) {
					gaManager.Evaluate (heuristic.Score, heuristic.solutionFound);
				} else {
					birdCrashed = false;
					LoadBird ();
				}
			}
		}
	}

	public void SetSolutionChromosome(double[] sChr){
		currentChromosome = new double[sChr.Length];

		for (int i = 0; i < sChr.Length; i++) {
			currentChromosome [i] = sChr [i];
		}

		Birds [0].GetComponent<Bird> ().Angle = currentChromosome [0];
		Birds [0].GetComponent<Bird> ().InitialForce = currentChromosome [1];
		Birds [0].GetComponent<Bird> ().PowerActivateTime = currentChromosome [2];

		Birds [1].GetComponent<Bird> ().Angle = currentChromosome [3];
		Birds [1].GetComponent<Bird> ().InitialForce = currentChromosome [4];
		Birds [1].GetComponent<Bird> ().PowerActivateTime = currentChromosome [5];

		Birds [2].GetComponent<Bird> ().Angle = currentChromosome [6];
		Birds [2].GetComponent<Bird> ().InitialForce = currentChromosome [7];
		Birds [2].GetComponent<Bird> ().PowerActivateTime = currentChromosome [8];

		if(birdIndex == 0)
			LoadBird ();
	}

	public void SetChromosome(double[] chsm){
		
		currentChromosome = new double[chsm.Length];

		for (int i = 0; i < chsm.Length; i++) {
			currentChromosome [i] = chsm [i];
		}
			
		Birds [0].GetComponent<Bird> ().Angle = currentChromosome [0];
		Birds [0].GetComponent<Bird> ().InitialForce = currentChromosome [1];
		Birds [0].GetComponent<Bird> ().PowerActivateTime = currentChromosome [2];

		if(birdIndex == 0)
			LoadBird ();
	}

	public void SetTuplesChromosome(int[] chsm){

		//currentChromosome = new double[chsm.Length * 3];
		/*
		for (int i = 0; i < currentChromosome.Length; i++) {
			for (int j = 0; j < 9; j++) {
				currentChromosome [i] = GeneticAlgorithm.population[ GeneticAlgorithm.tuplePopulation [chsm[i]]].chromosome [j % 3];
			}
		}*/
		Debug.Log ("population count: " + GeneticAlgorithm.population.Count);
		Debug.Log ("3 Bird chromosomes:");
		Debug.Log(
			GeneticAlgorithm.population [chsm [0]].chromosome [0] + "|" + 
			GeneticAlgorithm.population [chsm [0]].chromosome [1] + "|" + 
			GeneticAlgorithm.population [chsm [0]].chromosome [2] + "|" + 
			GeneticAlgorithm.population [chsm [1]].chromosome [0] + "|" + 
			GeneticAlgorithm.population [chsm [1]].chromosome [1] + "|" + 
			GeneticAlgorithm.population [chsm [1]].chromosome [2] + "|" + 
			GeneticAlgorithm.population [chsm [2]].chromosome [0] + "|" + 
			GeneticAlgorithm.population [chsm [2]].chromosome [1] + "|" + 
			GeneticAlgorithm.population [chsm [2]].chromosome [2] + "|"
		);

		Birds [0].GetComponent<Bird> ().Angle = GeneticAlgorithm.population [chsm [0]].chromosome [0];
		Birds [0].GetComponent<Bird> ().InitialForce = GeneticAlgorithm.population [chsm [0]].chromosome [1];
		Birds [0].GetComponent<Bird> ().PowerActivateTime = GeneticAlgorithm.population [chsm [0]].chromosome [2];

		Birds [1].GetComponent<Bird> ().Angle = GeneticAlgorithm.population [chsm [1]].chromosome [0];
		Birds [1].GetComponent<Bird> ().InitialForce = GeneticAlgorithm.population [chsm [1]].chromosome [1];
		Birds [1].GetComponent<Bird> ().PowerActivateTime = GeneticAlgorithm.population [chsm [1]].chromosome [2];

		Birds [2].GetComponent<Bird> ().Angle = GeneticAlgorithm.population [chsm [2]].chromosome [0];
		Birds [2].GetComponent<Bird> ().InitialForce = GeneticAlgorithm.population [chsm [2]].chromosome [1];
		Birds [2].GetComponent<Bird> ().PowerActivateTime = GeneticAlgorithm.population [chsm [2]].chromosome [2];

		if(birdIndex == 0)
			LoadBird ();
	}

	public void LoadBird(){
		if (birdIndex < Birds.Length - birdLimit) {
			
			currentBird = Birds [birdIndex];
			currentBird.simulated = false;
			currentBird.transform.position = Ramp.transform.position;
			currentBird.transform.rotation = Ramp.rotation;
			currentBird.transform.parent = Ramp;

			Vector3 linePos = new Vector3 ((float) currentBird.GetComponent<Bird>().InitialForce * 15f / 400f, 0, 0);
			forceLine.SetPosition(1, linePos);

			if (!gameOver && !isPlayerPlaying)
				SetSlingshot ();
			birdIndex++;

		} else {
			gameOver = true;
		}
	}

	public void SetSlingshot(){

		currentBird.GetComponent<Bird> ().SetInitialDistance ();

		Ramp.eulerAngles = Vector3.zero;
		Ramp.Rotate (0, 0, (float) currentBird.GetComponent<Bird> ().Angle);

		Invoke ("Shoot", 0.1f);
	}

	public void Shoot(){
		//Debug.Log ("Shooting...");
		//cam.FollowTarget (currentBird.transform);
		currentBird.GetComponent<Bird>().isTossed = true;
		currentBird.transform.parent = null;
		currentBird.simulated = true;
		currentBird.AddForce (new Vector2 (Ramp.right.x, Ramp.right.y) * (float) currentBird.GetComponent<Bird> ().InitialForce);
	}

	public void BirdCrashed(){
		birdCrashed = true;
	}

	public bool PigsMoving(){
		double movement = 0;
		for (int i = 0; i < Pigs.Length; i++) {
			if (Pigs [i].gameObject.activeSelf) {
				movement += Pigs [i].velocity.magnitude;
			}
		}
		PigMovementMag = movement;
		if (movement > 0.1f)
			return true;
		else
			return false;
	}

	public void UseAllBirds(){
		//Debug.Log ("Using all birds");
		birdLimit = 0;
	} 

	public void UseOneBird(){
		//Debug.Log ("Using one bird");
		birdLimit = 2;
	}
	/*
	public void PlayerMoves(){
		
		if (isPlayerPlaying) {
			rampRotation = Input.GetAxis ("Horizontal") * -20f * Time.deltaTime;
			currentBird.GetComponent<Bird> ().Angle += rampRotation;
			currentBird.GetComponent<Bird> ().Angle = Mathf.Clamp (currentBird.GetComponent<Bird> ().Angle, 0, 90);
		}

		if (Input.GetKey (KeyCode.Space)) {
			currentBird.GetComponent<Bird> ().InitialForce += 2f;
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			//cam.FollowTarget (Birds[birdIndex].transform);
			currentBird.GetComponent<Bird> ().isTossed = true;
			currentBird.transform.parent = null;
			currentBird.simulated = true;
			currentBird.AddForce (new Vector2 (Ramp.right.x, Ramp.right.y) * currentBird.GetComponent<Bird> ().InitialForce);
		}

		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			currentBird.AddForce ((Vector3.right + Vector3.down * 0.15f) * 300f);
		}
	}*/
}
