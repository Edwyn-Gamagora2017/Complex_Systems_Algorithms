using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticController : MonoBehaviour {

	int amountSubjects = 10;
	int amountSelectSubjects = 3;

	/*
	 * Genetic Loop
	 */
	void Init(){
		List<ChromosomeSalesman> population;

		// Create Population
		population = this.createPopulation();

		// Evaluation


		// Finished?


	}

	/*
	 * Creation
	 */
	public List<ChromosomeSalesman> createPopulation(){
		List<ChromosomeSalesman> population = new List<ChromosomeSalesman>();

		for(int i=0; i<this.amountSubjects; i++){
			population.Add ( new ChromosomeSalesman() );
		}

		return population;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
