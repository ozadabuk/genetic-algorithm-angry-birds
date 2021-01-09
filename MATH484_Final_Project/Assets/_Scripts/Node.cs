using System.Collections;
using System.Collections.Generic;
using System;

public class Node {

	public int nodeIndex = 0;
	public double[] chromosome = new double[3];
	public double fitness = 0.0f;
	System.Random randGen;
	public bool solution = false;

	public Node(){

		randGen = new System.Random(GetHashCode());
		fitness = 0.0f;
		chromosome = new double[3];

		for (int i = 0; i < chromosome.Length; i++) {
			if (i % 3 == 0)
				chromosome [i] = GetAngle ();
			if (i % 3 == 1)
				chromosome [i] = GetForce ();
			if(i % 3 == 2)
				chromosome [i] = GetTime ();
		}
	}

	public Node(double[] chr){

		fitness = 0.0f;
		chromosome = new double[3];

		for (int i = 0; i < chromosome.Length; i++) {
			chromosome [i] = chr [i];
		}
	}

	public double GetAngle(){
		return Math.Round(randGen.NextDouble () * 90f, 2);
	}

	public double GetForce(){
		return Math.Round( randGen.NextDouble () * 300f, 2);
	}

	public double GetTime(){
		return Math.Round(randGen.NextDouble () * 3, 2);
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
