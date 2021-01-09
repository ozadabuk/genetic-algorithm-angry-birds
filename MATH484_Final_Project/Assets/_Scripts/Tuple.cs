using System.Collections;
using System.Collections.Generic;
using System;

public class Tuple {

	public int nodeIndex = 0;
	public int[] chromosome = new int[3];
	public double fitness = 0.0f;
	System.Random randGen;
	public bool solution = false;

	public Tuple(){

		randGen = new System.Random(GetHashCode());
		fitness = 0.0f;
		chromosome = new int[3];

		for (int i = 0; i < chromosome.Length; i++) {
			chromosome [i] = randGen.Next (GeneticAlgorithm.populationSize);
		}
	}

	public Tuple(int[] chr){

		fitness = 0.0f;
		chromosome = new int[3];

		for (int i = 0; i < chromosome.Length; i++) {
			chromosome [i] = chr [i];
		}
	}

	public string GetChromosome(){
		string str = "";
		for (int i = 0; i < chromosome.Length; i++) {
			str += chromosome [i].ToString () + "|";
		}
		str += " Fitness: " + GetFitness ().ToString();
		if (solution)
			str += " solution";
		return str;
	}

	public void SetFitness(double f, bool s){
		solution = s;
		fitness = f;
	}

	public double GetFitness(){
		return Math.Round(fitness, 2);
	}
}
