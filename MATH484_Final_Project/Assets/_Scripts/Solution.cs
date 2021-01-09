using System.Collections;
using System.Collections.Generic;

public class Solution {

	public double[] solutionChromosome = new double[9];

	public Solution(double[] chr){

		solutionChromosome = new double[9];

		for (int i = 0; i < 9; i++) {
			solutionChromosome [i] = chr [i];
		}
	}
}
