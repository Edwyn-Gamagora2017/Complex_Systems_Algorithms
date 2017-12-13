using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromosomeSalesman : Chromosome, System.IComparable<ChromosomeSalesman> {
	public static Graph graph;
	public static List< SalesmanCity > cities;

	private List< int > path;	// path including the index of each city the Salesman must visit

	public ChromosomeSalesman():base(){
		this.path = new List<int> ();
	}

	/*
	 * GETTERS and SETTERS
	 */
	public List<int> Path {
		get {
			return path;
		}
		set {
			path = value;
		}
	}

	#region implemented abstract members of Chromosome
	public override void setRandomValue ()
	{
		List<SalesmanCity> availableCities = new List<SalesmanCity> (ChromosomeSalesman.cities);

		while (availableCities.Count > 0) {
			int chosenCity = Random.Range (0, availableCities.Count);
			this.path.Add (availableCities[chosenCity].IndexCity);
			availableCities.RemoveAt (chosenCity);
		}
	}
	public override float fitness ()
	{
		// Path cost
		//return ChromosomeSalesman.graph.getFloydWarshallDistance(  );
		float distance = 0;
		for( int i=0; i<path.Count; i++ ){
			float partialDistance = ChromosomeSalesman.graph.getFloydWarshallDistance( cities[path[i]].IndexVertex, cities[path[(i+1)%path.Count]].IndexVertex );
			// the distance includes ending vertex cost. it must be ignored because it is the cost of start vertex of the next path
			partialDistance -= ChromosomeSalesman.graph.getVertexCost( cities[path[(i+1)%path.Count]].IndexVertex );
			distance+=partialDistance;
		}
		return distance;
	}
	public override Chromosome crossing (Chromosome c)
	{
		// The first half is a copy from the first chromosome
		List<int> resultPath = this.path.GetRange(0,this.path.Count/2);
		// Cities from the second chromosome
		foreach( int city in ((ChromosomeSalesman)c).Path ){
			if (!resultPath.Contains (city)) {
				resultPath.Add ( city );
			}
		}

		ChromosomeSalesman result = new ChromosomeSalesman ();
		result.Path = resultPath;

		return result;
	}
	public override void mutation ( int mutationPropabiblity )
	{
		// Change the position of cities
		for( int i = 0; i < path.Count; i++ ){
			bool mutate = Random.Range (0, mutationPropabiblity) == 0;
			if (mutate) {
				Debug.Log ( "++++++++++++++++++++++ mutation" );
				//Debug.Log ( "old: "+ this.toString ());
				// Choose a city to change (count-1 because the current city is not included)
				int index = Random.Range ( 0, path.Count-1 );
				// If index is the current city, choose the next one
				if (index == i) {
					index++;
				}
				// change values between cities
				int cityIndex = path[index];
				path [index] = path [i];
				path [i] = cityIndex;
				//Debug.Log ("new: "+this.toString ());
			}
		}
	}
	public override bool isValid ()
	{
		bool[] visitedCities = new bool[cities.Count];
		foreach( int city in path ){
			if (visitedCities[city]) {
				return false;
			} else {
				visitedCities [city] = true;
			}
		}
		return true;
	}

	public override string toString ()
	{
		string path = "";
		foreach (int cityIndex in this.path) {
			path += cityIndex + " - ";
		}
		return path;
	}

	public override bool isBetterThan (Chromosome c)
	{
		return ((System.IComparable<ChromosomeSalesman>)this).CompareTo( (ChromosomeSalesman)c ) <= 0;
	}

	public override bool isFinalSolution ()
	{
		// it is not possible to define a final solution before the iteration
		// Unless the cost of the path is equals to the number of cities. Not consider here because it is not worth
		return false;
	}
	#endregion

	#region IComparable implementation

	/* 1 : this is better than other
	 * -1 : other is better than this
	 * 0 : both are equal
	 */
	int System.IComparable<ChromosomeSalesman>.CompareTo (ChromosomeSalesman other)
	{
		float thisFit = this.fitness ();
		float otherFit = other.fitness ();
		return ( thisFit < otherFit ? -1 : ( thisFit > otherFit ? 1 : 0 ) );
	}

	#endregion
}
