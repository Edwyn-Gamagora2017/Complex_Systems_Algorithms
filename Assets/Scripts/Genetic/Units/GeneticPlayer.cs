using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticPlayer : GeneticCharacter {
	
	ChromosomeSalesman solution;
	bool bestSolution;
	float worstFitness;		// used to calculate the time for moving the players (based on the worst fitness)

	public GeneticPlayer( Vector2 position, MapGenetic map, ChromosomeSalesman solution, bool bestSolution, float worstFitness ):base( position, map ){
		this.solution = solution;
		this.bestSolution = bestSolution;
		this.worstFitness = worstFitness;
	}

	public ChromosomeSalesman Solution {
		get {
			return solution;
		}
	}

	public bool BestSolution {
		get {
			return bestSolution;
		}
	}

	public float WorstFitness {
		get {
			return worstFitness;
		}
	}
}
