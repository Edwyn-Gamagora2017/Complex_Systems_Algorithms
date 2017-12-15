using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticController<T> where T : Chromosome, new() {

	// Subjects
	int amountInitialSubjects;
	int amountSelectSubjects;

	// Generations
	int maxGenerations;
	int currentGeneration;

	// Mutation
	int mutationPropabiblity;	// indicates D, since Probability = 1/D

	public GeneticController() : this( 10, 3, 10, 100 ){}

	public GeneticController( int amountInitialSubjects, int amountSelectSubjects, int maxGenerations, int mutationPropabiblity ){
		this.amountInitialSubjects = amountInitialSubjects;
		this.amountSelectSubjects = amountSelectSubjects;
		this.maxGenerations = maxGenerations;
		this.mutationPropabiblity = mutationPropabiblity;
		this.currentGeneration = 0;
	}

	/*
	 * Genetic Loop
	 */
	public T CalculateSolution(){
		List<T> population;

		// Create Population
		population = this.createPopulation();

		// Evaluation
		population.Sort();

/*
foreach (T c in population) {
	Debug.Log ( "Initial = "+c.toString() );
	Debug.Log ( "Cost = "+c.fitness() );
}
/*/

		T partialSolution = population [0];

//*		// Finished?
		while( !partialSolution.isFinalSolution() && currentGeneration < maxGenerations ){
			// Selection
			List<T> selectedPopulation = selectionPopulation( population );

			// Crossing
			population = crossPopulation( selectedPopulation );

			// Mutation in crossing result
			mutationPopulation( population );

			// Validate Population TEST
			//if( !validatePopulation( population ) ){ throw new UnityEngine.UnityException( "Genetic : Population is not valid generation "+currentGeneration ); } else{ Debug.Log( "Genetic : Population is valid at generation "+currentGeneration ); }

			// include selected ones among the new Population
			foreach( T t in selectedPopulation ){
				population.Add ( t );
			}

			// Evaluation
			population.Sort();

			if( population[0].isBetterThan( partialSolution ) ){
				partialSolution = population [0];
			}

			currentGeneration++;
		}
//*/
		return partialSolution;
	}

	/*
	 * Creation
	 */
	protected List<T> createPopulation(){
		List<T> population = new List<T>();

		for(int i=0; i<this.amountInitialSubjects; i++){
			population.Add ( new T() );
			population [i].setRandomValue ();
		}

		return population;
	}

	/*
	 * Selection
	 */
	protected List<T> selectionPopulation( List<T> sortedPopulation ){
		List<T> selected = new List<T>();

		// Selection
		for(int i=0; i<this.amountSelectSubjects; i++){
			selected.Add ( sortedPopulation[i] );
		}

		return selected;
	}

	/*
	 * Crossing
	 */
	protected List<T> crossPopulation( List<T> selectedSubjects ){
		List<T> population = new List<T>();

		// Cross
		for(int i=0; i<selectedSubjects.Count; i++){
			population.Add ( (T) selectedSubjects[i].crossing( (Chromosome) selectedSubjects[(i+1)%selectedSubjects.Count] ) );
			population.Add ( (T) selectedSubjects[(i+1)%selectedSubjects.Count].crossing( (Chromosome) selectedSubjects[i] ) );
		}

		return population;
	}

	/*
	 * Mutation
	 */
	protected void mutationPopulation( List<T> population ){
		// Selection
		foreach( T chromosome in population ){
			chromosome.mutation ( this.mutationPropabiblity );
		}
	}

	/*
	 * Validate Population
	 */
	protected bool validatePopulation( List<T> population ){
		foreach( T chromosome in population ){
			if (!chromosome.isValid ()) {
				return false;
			}
		}
		return true;
	}
}
