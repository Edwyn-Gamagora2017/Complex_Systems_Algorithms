using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticSceneController : MonoBehaviour {

	GeneticController<ChromosomeSalesman> geneticController;

	void Awake(){
		geneticController = new GeneticController<ChromosomeSalesman> ();

		// Setting basic info to Chromosome
		ChromosomeSalesman.cities = new List<SalesmanCity>();
		ChromosomeSalesman.cities.Add (new SalesmanCity (0));
		ChromosomeSalesman.cities.Add (new SalesmanCity (1));
		ChromosomeSalesman.cities.Add (new SalesmanCity (2));

		ChromosomeSalesman solution = geneticController.Calculate ();
		Debug.Log ( solution.toString() );
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
