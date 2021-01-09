/* Intellectual property of Oguzcan Adabuk, 2017 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour {

	public static List<Node> population = new List<Node>();
	public static List<Tuple> tuplePopulation = new List<Tuple>();

	public static Tuple TupleParent1;
	public static Tuple TupleParent2;
	public static Tuple TupleChild;

	public static Node Parent1;
	public static Node Parent2;
	public static Node Child;

	public static int populationSize = 6;
	public static int tournamentSize = 10;
	public static int childrenNum = 5;
	public static int childCount = 0;

	public static void Init () {

		for (int i = 0; i < populationSize; i++) {
			Node individual = new Node ();
			individual.nodeIndex = i;
			population.Add (individual);
		}
	}

	public static void InitTuple () {

		for (int i = 0; i < populationSize; i++) {
			Tuple individual = new Tuple ();
			individual.nodeIndex = i;
			tuplePopulation.Add (individual);
		}
	}

	public static void TournamentSelection(){

		// Choose parent 1
		List<Node> participants = new List<Node> ();
		int[] pIndices = new int[tournamentSize];
		Random.InitState (System.DateTime.Now.Millisecond);

		for (int i = 0; i < pIndices.Length; i++)
			pIndices[i] = Random.Range (0, population.Count);

		for (int i = 0; i < pIndices.Length; i++)
			participants.Add (population [pIndices [i]]);
	
		double maxScore = double.MinValue;

		for (int i = 0; i < participants.Count; i++) {
			if (participants [i].GetFitness () > maxScore) {
				maxScore = participants [i].GetFitness ();
				Parent1 = new Node(participants [i].chromosome);
			}
		}

		// Choose parent 2
		pIndices = new int[tournamentSize];
		participants.Clear ();

		for (int i = 0; i < pIndices.Length; i++)
			pIndices[i] = Random.Range (0, population.Count);

		for (int i = 0; i < pIndices.Length; i++)
			participants.Add (population [pIndices [i]]);

		maxScore = double.MinValue;

		for (int i = 0; i < participants.Count; i++) {
			if (participants [i].GetFitness () > maxScore) {
				maxScore = participants [i].GetFitness ();
				Parent2 = new Node(participants [i].chromosome);
			}
		}
		Debug.Log ("Parent 1: " + Parent1.GetChromosome() + " Parent 2: " + Parent2.GetChromosome());
		CrossOver ();
	}

	public static void TupleTournamentSelection(){

		// Choose parent 1
		List<Tuple> participants = new List<Tuple> ();
		int[] pIndices = new int[tournamentSize];
		Random.InitState (System.DateTime.Now.Millisecond);

		for (int i = 0; i < pIndices.Length; i++)
			pIndices[i] = Random.Range (0, tuplePopulation.Count);

		for (int i = 0; i < pIndices.Length; i++)
			participants.Add (tuplePopulation [pIndices [i]]);

		double maxScore = double.MinValue;

		for (int i = 0; i < participants.Count; i++) {
			if (participants [i].GetFitness () > maxScore) {
				maxScore = participants [i].GetFitness ();
				TupleParent1 = new Tuple(participants [i].chromosome);
			}
		}

		// Choose parent 2
		pIndices = new int[tournamentSize];
		participants.Clear ();

		for (int i = 0; i < pIndices.Length; i++)
			pIndices[i] = Random.Range (0, tuplePopulation.Count);

		for (int i = 0; i < pIndices.Length; i++)
			participants.Add (tuplePopulation [pIndices [i]]);

		maxScore = double.MinValue;

		for (int i = 0; i < participants.Count; i++) {
			if (participants [i].GetFitness () > maxScore) {
				maxScore = participants [i].GetFitness ();
				TupleParent2 = new Tuple(participants [i].chromosome);
			}
		}
		Debug.Log ("Tuple Parent 1: " + TupleParent1.GetChromosome() + " Tuple Parent 2: " + TupleParent2.GetChromosome());
		TupleCrossOver ();
	}

	public static void CrossOver(){

		double[] childChromosome = new double[3];
		string str = "";

		for (int i = 0; i < childChromosome.Length; i++) {
			Random.InitState (System.DateTime.Now.Millisecond * i);
			int p1score = (int) (Parent1.GetFitness () + 1) * 10;
			int p2score = (int) (Parent2.GetFitness () + 1) * 10;
			int totalScore = p1score + p2score;

			int randNum = Random.Range (0, totalScore);
			if (randNum <= p1score) {
				childChromosome [i] = Parent1.chromosome [i];
			} else {
				childChromosome [i] = Parent2.chromosome [i];
			}
			str += childChromosome [i] + "|";
		}

		Debug.Log ("Child        : " + PrintChromosome (childChromosome));

		Random.InitState (System.DateTime.Now.Millisecond);
		double randNum2 = Random.Range (0, 100);

		if (randNum2 > 85) {
			
			// Mutate
			for (int i = 0; i < childChromosome.Length; i++) {
				Random.InitState (System.DateTime.Now.Millisecond * i);
				randNum2 = Random.Range (0, 100);

				if((Parent1.GetFitness() + Parent2.GetFitness()) / 2 < GetAverageFitness())
					randNum2 *= 1.25f;
				
				if (randNum2 > 60) {
					if (i % 3 == 0)
						childChromosome [i] = System.Math.Round((double)Random.Range (0f, 90f), 2);
					if (i % 3 == 1)
						childChromosome [i] = System.Math.Round((double)(double)Random.Range (0f, 300f), 2);
					if (i % 3 == 2)
						childChromosome [i] = System.Math.Round((double)(double)Random.Range (0f, 3f), 2);
				}

			}

			Debug.Log ("Child MUTATED: " + PrintChromosome (childChromosome));
		}
		Child = new Node (childChromosome);

		PlaceChild ();
	}

	public static void TupleCrossOver(){

		int[] childChromosome = new int[3];
		string str = "";

		for (int i = 0; i < childChromosome.Length; i++) {
			Random.InitState (System.DateTime.Now.Millisecond * i);
			int p1score = (int) (TupleParent1.GetFitness () + 1) * 10;
			int p2score = (int) (TupleParent2.GetFitness () + 1) * 10;
			int totalScore = p1score + p2score;

			int randNum = Random.Range (0, totalScore);
			if (randNum <= p1score) {
				childChromosome [i] = TupleParent1.chromosome [i];
			} else {
				childChromosome [i] = TupleParent2.chromosome [i];
			}
			str += childChromosome [i] + "|";
		}

		Debug.Log ("Tuple Child        : " + PrintChromosome (childChromosome));

		Random.InitState (System.DateTime.Now.Millisecond);
		double randNum2 = Random.Range (0, 100);

		if (randNum2 > 85) {

			// Mutate
			for (int i = 0; i < childChromosome.Length; i++) {
				Random.InitState (System.DateTime.Now.Millisecond * i);
				randNum2 = Random.Range (0, 100);

				if((TupleParent1.GetFitness() + TupleParent2.GetFitness()) / 2 < GetAverageFitness())
					randNum2 *= 1.25f;

				if (randNum2 > 60) {
					childChromosome [i] = Random.Range (0, 3);
				}
			}

			Debug.Log ("Tuple Child MUTATED: " + PrintChromosome (childChromosome));
		}
		TupleChild = new Tuple (childChromosome);

		PlaceTupleChild ();
	}

	public static void PlaceChild(){
		// Choose a loser
		List<int> partIndices = new List<int>();

		int[] pIndices = new int[tournamentSize];

		for (int i = 0; i < pIndices.Length; i++)
			pIndices[i] = Random.Range (0, population.Count);

		double minScore = double.MaxValue;
		int loserIndex = 0;

		for (int i = 0; i < pIndices.Length; i++) {
			if (population [pIndices[i]].GetFitness () < minScore && !partIndices.Contains(loserIndex)) {
				minScore = population [pIndices[i]].GetFitness ();
				loserIndex = pIndices[i];
				partIndices.Add (loserIndex);
			}
		}

		population [loserIndex] = new Node (Child.chromosome);
		population [loserIndex].nodeIndex = loserIndex;

		childCount++;
		//Debug.Log ("Replacing at node at " + loserIndex);
		if (childCount < childrenNum) {
			TournamentSelection ();
		}
		else
			childCount = 0;
	}

	public static void PlaceTupleChild(){
		// Choose a loser
		List<int> partIndices = new List<int>();

		int[] pIndices = new int[tournamentSize];

		for (int i = 0; i < pIndices.Length; i++)
			pIndices[i] = Random.Range (0, tuplePopulation.Count);

		double minScore = double.MaxValue;
		int loserIndex = 0;

		for (int i = 0; i < pIndices.Length; i++) {
			if (tuplePopulation [pIndices[i]].GetFitness () < minScore && !partIndices.Contains(loserIndex)) {
				minScore = tuplePopulation [pIndices[i]].GetFitness ();
				loserIndex = pIndices[i];
				partIndices.Add (loserIndex);
			}
		}

		tuplePopulation [loserIndex] = new Tuple (TupleChild.chromosome);
		tuplePopulation [loserIndex].nodeIndex = loserIndex;

		childCount++;
		//Debug.Log ("Replacing at node at " + loserIndex);
		if (childCount < childrenNum) {
			TupleTournamentSelection ();
		}
		else
			childCount = 0;
	}

	public static string GetAverageHeuristic(){

		double h = 0.0f;
		for (int i = 0; i < population.Count; i++)
			h += population [i].GetFitness ();
		double average = h / population.Count;
		return average.ToString ("n3");
	}

	public static double GetAverageFitness(){
		double f = 0.0f;
		for (int i = 0; i < population.Count; i++)
			f += population [i].GetFitness ();
		double average = f / population.Count;
		return average;
	}

	public static string PrintChromosome(double[] chr){
		string str = "";
		for (int i = 0; i < chr.Length; i++) {
			str += chr [i].ToString ("n3") + "|";
		}
		return (str);
	}

	public static string PrintChromosome(int[] chr){
		string str = "";
		for (int i = 0; i < chr.Length; i++) {
			str += chr [i].ToString ("n3") + "|";
		}
		return (str);
	}

	public static bool IsUniqueSolution(Node solutionNode){
		for (int i = 0; i < population.Count; i++) {
			if (IsSameChromosome (population [i].chromosome, solutionNode.chromosome))
				return false;
		}

		return true;
	}

	public static bool IsUniqueTupleSolution(Tuple solutionTuple){
		for (int i = 0; i < tuplePopulation.Count; i++) {
			if (IsSameChromosome (tuplePopulation [i].chromosome, solutionTuple.chromosome))
				return false;
		}

		return true;
	}
	public static bool IsSameChromosome(double[] a, double[] b){
		for (int i = 0; i < a.Length; i++) {
			if (a [i] != b [i])
				return false;
		}
		return true;
	}

	public static bool IsSameChromosome(int[] a, int[] b){
		for (int i = 0; i < a.Length; i++) {
			if (a [i] != b [i])
				return false;
		}
		return true;
	}
}
