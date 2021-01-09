using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heuristic : MonoBehaviour {

	public double Score = 0f;
	public Text TextScore;
	public int pigsKilled = 0;
	public bool solutionFound = false;

	void Start () {
		//Invoke ("Clean", 1.5f);
	}

	public void PigKilled(){
		pigsKilled++;
		if (pigsKilled == 4) {
			solutionFound = true;
		}
	}

	public void AddScore(double scr){
		Score += scr;
		TextScore.text = "Score: " + Score.ToString ("n3");
	}

	void Clean(){
		Score = 0.0f;
		TextScore.text = "Score: " + Score.ToString ("n3");
	}
}
