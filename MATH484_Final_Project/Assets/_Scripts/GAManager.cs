/* Intellectual property of Oguzcan Adabuk, 2017 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAManager : MonoBehaviour {

	public enum Mode {train, solutions}
	public Mode runMode;

	public Slingshot slingshot;
	public SaveLoadManager saveLoadManager;

	public int PopulationSize = 25;
	public int TournamentSize = 15;
	public int ChildrenSize = 12;

	public List<Solution> solutions = new List<Solution>();
	public static int sIndex = -1;

	public static int chIndex = -1;
	public static int tIndex = -1;

	public static int generation = 0;
	public static int tupleGeneration = 0;

	public Vector2 scrollPosition;
	public Vector2 scrollPosition2;

	public Node currentNode;
	public Tuple currentTuple;

	public GUIStyle gStyle;
	public bool loadNodes = false;
	public bool loadTuples = false;


	public bool playAll = false;

	public bool trainTuples = true;
	private static bool playFast = true;
	private static bool showGenes = true;

	void Start () {
		if (runMode == Mode.train) {
			GeneticAlgorithm.populationSize = PopulationSize;
			GeneticAlgorithm.tournamentSize = TournamentSize;
			GeneticAlgorithm.childrenNum = ChildrenSize;

			if (GeneticAlgorithm.population.Count == 0) {
				if (loadNodes) {
					List<Node> loadedNodes = saveLoadManager.LoadGeneration ();
					GeneticAlgorithm.population = loadedNodes;
				} else {
					GeneticAlgorithm.Init ();	
				}
			}

			if (GeneticAlgorithm.tuplePopulation.Count == 0) {
				if (loadTuples) {
					List<Tuple> loadedTuples = saveLoadManager.LoadTupleGeneration ();
					GeneticAlgorithm.tuplePopulation = loadedTuples;
				} else {
					GeneticAlgorithm.InitTuple ();	
				}
			}
				
			if (trainTuples) {
				slingshot.UseAllBirds ();
				ProcessTupleChromosome ();

			} else {
				slingshot.UseOneBird ();	
				ProcessChromosome ();
			}
		} else {
			if(solutions.Count == 0)
			{
				solutions = saveLoadManager.LoadSolution ();
				PlaySolution ();
			}
		}
	}

	void OnGUI(){

		if (GUI.Button (new Rect (270, 10, 100, 50), "Chromosomes")) {
			showGenes = !showGenes;
		}

		if (GUI.Button (new Rect (880, 10, 120, 50), "Toggle Speed")) {
			playFast = !playFast;
			if (playFast)
				Time.timeScale = 3.0f;
			else
				Time.timeScale = 1.0f;
			Debug.Log ("Play Fast: " + playFast + " " + Time.timeScale);
		}

		/*
		if (GUI.Button (new Rect (900, 10, 150, 50), "Toggle Train Tuples")) {
			trainTuples = !trainTuples;
			if (trainTuples)
				slingshot.UseAllBirds ();
			else
				slingshot.UseOneBird ();
			Debug.Log ("Training tuples: " + trainTuples);

		}*/

		if (trainTuples) {
			if (GUI.Button (new Rect (1050, 10, 150, 50), "Save Tuple Generation")) {
				Debug.Log ("Saving current tuple generation:");
				saveLoadManager.SaveTupleGeneration (generation, ChildrenSize, GeneticAlgorithm.tuplePopulation);
			}
		} else {
			
			if (GUI.Button (new Rect (1050, 10, 150, 50), "Save Node Generation")) {
				Debug.Log ("Saving current node generation:");
				saveLoadManager.SaveGeneration (generation, ChildrenSize, GeneticAlgorithm.population);
			}
		}

		if (GUI.Button (new Rect (450, 10, 120, 50), "Save Solutions")) {
			Debug.Log ("Saving solutions:");
			saveLoadManager.SaveSolution (generation, ChildrenSize, GeneticAlgorithm.tuplePopulation);
		}

		if (showGenes) {
			
			GUI.Label (new Rect (15, 0, 200, 20), "Tuple Generation: " + generation + " Average fitness: " + GeneticAlgorithm.GetAverageFitness ().ToString("n3") + " Current: " + tIndex, gStyle);
			for (int i = 0; i < GeneticAlgorithm.tuplePopulation.Count; i++) {
				GUI.Label (new Rect (15, (i+1) * 20, 200, 20), GeneticAlgorithm.tuplePopulation [i].GetChromosome (), gStyle);
			}

			GUI.Label (new Rect (800, 0, 200, 20), "Node Generation: " + generation + " Average fitness: " + GeneticAlgorithm.GetAverageFitness () + " Current: " + chIndex, gStyle);
			for (int i = 0; i < GeneticAlgorithm.population.Count; i++) {
				GUI.Label (new Rect (800, (i+1) * 20, 200, 20), GeneticAlgorithm.population [i].GetChromosome (), gStyle);
			}
		}
	}

	public void PlaySolution(){
		sIndex++;
		if (sIndex < solutions.Count) {
			slingshot.SetSolutionChromosome (solutions [sIndex].solutionChromosome);
		} else {
			
		}
	}

	public void ProcessChromosome(){

		chIndex++;

		if (chIndex == GeneticAlgorithm.population.Count) {
			chIndex = -1;

			GeneticAlgorithm.TournamentSelection ();
			generation++;
			Debug.Log ("New Generation");

			ProcessChromosome ();
		} else {
			currentNode = GeneticAlgorithm.population [chIndex];
			if (currentNode.GetFitness () == 0.0f || playAll)
				slingshot.SetChromosome (currentNode.chromosome);
			else
				EvaluateChromosome (currentNode.GetFitness (), currentNode.solution);
		}
	}

	public void ProcessTupleChromosome(){

			tIndex++;

		if (tIndex == GeneticAlgorithm.tuplePopulation.Count) {
			tIndex = -1;

			GeneticAlgorithm.TupleTournamentSelection ();
			generation++;
			Debug.Log ("New Generation");

			ProcessChromosome ();
		} else {
			//Debug.Log ("tIndex: " + tIndex + " tuples count: " + GeneticAlgorithm.tuplePopulation.Count);
			currentTuple = GeneticAlgorithm.tuplePopulation [tIndex];
			if (currentTuple.GetFitness () == 0.0f || playAll) {
				slingshot.SetTuplesChromosome (currentTuple.chromosome);
			} else {
				EvaluateTupleChromosome (currentTuple.GetFitness (), currentTuple.solution);
			}
		}
	}

	public void LoadNextSolution(){
		SceneManager.LoadScene (0);
	}

	public void Evaluate(double score, bool solutionFound){
		if (runMode == Mode.train) {
			if (trainTuples) {
				EvaluateTupleChromosome (score, solutionFound);
			} else {
				EvaluateChromosome (score, solutionFound);
			}
		} else {
			LoadNextSolution ();
		}
	}
	public void EvaluateChromosome(double score, bool solutionFound){
		currentNode.SetFitness (score, solutionFound);

		SceneManager.LoadScene (0);
	}
		
	public void EvaluateTupleChromosome(double score, bool solutionFound){
		currentTuple.SetFitness (score, solutionFound);

		SceneManager.LoadScene (0);
	}
}
