using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {

	StreamWriter sw;
	StreamReader sr;

	public string file = "Assets/Data/Generations.txt";
	public string tupleFile = "Assets/Data/Tuples.txt";
	public string solutionFile = "Assets/Data/Solutions.txt";

	void Start () {
		
	}
	
	public void SaveGeneration(int generationNumber, int childrenSize, List<Node> population){

		if (!Directory.Exists ("Assets/Data")) {
			Directory.CreateDirectory ("Assets/Data");
		}
		if (!File.Exists (file)) {
			File.Create (file);
		}

		sw = new StreamWriter (file);

		for (int i = 0; i < population.Count; i++) {
			string str = "";

			for(int j = 0; j < population[i].chromosome.Length; j++){
				str += population[i].chromosome[j].ToString();
				str += "|";
			}

				// Save fitness
			str += population[i].GetFitness() + "|";
				// Save solution
			str += population[i].solution + "|";
				// Save nodeIndex
			str += population[i].nodeIndex + "|";
	
			sw.WriteLine (str);

		}
		sw.Close ();
		Debug.Log ("Generation successfully saved.");
	}

	public void SaveTupleGeneration(int generationNumber, int childrenSize, List<Tuple> tuplePopulation){

		if (!Directory.Exists ("Assets/Data")) {
			Directory.CreateDirectory ("Assets/Data");
		}
		if (!File.Exists (tupleFile)) {
			File.Create (tupleFile);
		}

		sw = new StreamWriter (tupleFile);

		for (int i = 0; i < tuplePopulation.Count; i++) {
			string str = "";

			for(int j = 0; j < tuplePopulation[i].chromosome.Length; j++){
				str += tuplePopulation[i].chromosome[j].ToString();
				str += "|";
			}

			// Save fitness
			str += tuplePopulation[i].GetFitness() + "|";
			// Save solution
			str += tuplePopulation[i].solution + "|";
			// Save nodeIndex
			str += tuplePopulation[i].nodeIndex + "|";

			sw.WriteLine (str);

		}
		sw.Close ();
		Debug.Log ("Tuple generation successfully saved.");
	}

	public void SaveSolution(int generationNumber, int childrenSize, List<Tuple> tuplePopulation){

		if (!Directory.Exists ("Assets/Data")) {
			Directory.CreateDirectory ("Assets/Data");
		}
		if (!File.Exists (solutionFile)) {
			File.Create (solutionFile).Dispose();
		}
			
		string wholeFile = "";
		Debug.Log ("Make sure str doesn't exist");
		using (StreamReader sr = new StreamReader (solutionFile)) {
			while (sr.Peek () >= 0) {
				wholeFile += sr.ReadLine ();
			}
			sr.Close ();
		}

		sw = new StreamWriter (solutionFile);
		Debug.Log ("Save solution to file");

		for (int i = 0; i < tuplePopulation.Count; i++) {
			string str = "";
			int m = 0;
			if (tuplePopulation [i].solution) {
				for (int j = 0; j < 3; j++) {
					for (int k = 0; k < 3; k++) {
						str += GeneticAlgorithm.population [tuplePopulation [i].chromosome [j]].chromosome [k].ToString ();
						if(m < 8) 
							str += "|";
						m++;
					}
				}

				Debug.Log (str);

				if(!wholeFile.Contains(str))
				{
					sw.WriteLine (str);
				}
			}
		}


		sw.Close ();
		Debug.Log ("Solutions successfully saved.");
	}

	public List<Node> LoadGeneration(){
		List<Node> generation = new List<Node> ();

		using (StreamReader sr = new StreamReader (file)) {
			while (sr.Peek () >= 0) {
				string line = sr.ReadLine ();
				string[] ch = line.Split ('|');

				double[] chromosome = new double[3];
				for (int i = 0; i < chromosome.Length; i++) {
					chromosome [i] = System.Math.Round(double.Parse(ch [i]), 2);
				}

				Node newNode = new Node (chromosome);
				newNode.fitness = double.Parse (ch [3]);
				newNode.solution = bool.Parse (ch [4]);
				newNode.nodeIndex = int.Parse (ch [5]);
				generation.Add (newNode);

			}
			sr.Close ();

		}

		Debug.Log ("Generation successfully loaded.");

		return generation;
	}

	public List<Tuple> LoadTupleGeneration(){
		List<Tuple> tupleGeneration = new List<Tuple> ();

		using (StreamReader sr = new StreamReader (tupleFile)) {
			while (sr.Peek () >= 0) {
				string line = sr.ReadLine ();
				string[] ch = line.Split ('|');

				int[] tupleChromosome = new int[3];
				for (int i = 0; i < tupleChromosome.Length; i++) {
					tupleChromosome [i] = int.Parse (ch [i]);
				}

				Tuple newNode = new Tuple (tupleChromosome);
				newNode.fitness = double.Parse (ch [3]);
				newNode.solution = bool.Parse (ch [4]);
				newNode.nodeIndex = int.Parse (ch [5]);
				tupleGeneration.Add (newNode);

			}
			sr.Close ();
		}

		Debug.Log ("Tuple generation successfully loaded.");

		return tupleGeneration;
	}

	public List<Solution> LoadSolution(){

		Debug.Log ("Loading solutions...");
		List<Solution> solutions = new List<Solution> ();

		using (StreamReader sr = new StreamReader (solutionFile)) {
			while (sr.Peek () >= 0) {
				string line = sr.ReadLine ();
				string[] ch = line.Split ('|');

				double[] solutionChromosome = new double[9];
				for (int i = 0; i < solutionChromosome.Length; i++) {
					solutionChromosome [i] = double.Parse (ch [i]);
				}

				Solution sol = new Solution (solutionChromosome);
				solutions.Add (sol);

			}
			sr.Close ();
		}

		Debug.Log ("Solutions successfully loaded.");

		return solutions;
	}
}
