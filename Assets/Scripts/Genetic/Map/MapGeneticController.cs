using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneticController : MonoBehaviour {

	MapGenetic mapModel;			// stores the map information
	[SerializeField]
	TextAsset mapPath;

	bool showBestSolution = true;

	GeneticSceneController geneticController;
	List<ChromosomeSalesman> solutions;
	ChromosomeSalesman solution;
	List<GeneticPlayer> players;
	List<SalesmanCity> cities;

	public bool setMap(){
		// Create the map based on the file
		this.mapModel = MapGenetic.read( mapPath.text );

		// Setting map to view
		if( this.GetComponent<MapGeneticView>() != null ){
			this.GetComponent<MapGeneticView>().MapModel = this.mapModel;
			this.GetComponent<MapGeneticView>().MapController = this;
		}
			
		this.mapModel.Graph.floydWarshall();	// Calculate all paths
		// Setting graph to genetic controller
		cities = new List<SalesmanCity>();
		for( int i = 0; i < mapModel.Targets.Count; i++  ){
			// For each target, create a city indicating the index of the vertex
			cities.Add( new SalesmanCity( i, mapModel.getCharacterGraphPosition( mapModel.Targets[i] ) ) );
		}
		geneticController = GetComponent<GeneticSceneController>();
		geneticController.setGraph( this.mapModel.Graph, cities );
		//this.selectShowSolutionMode( showBestSolution );

		return this.mapModel != null;
	}

	public float getCharacterPositionCost( GeneticCharacter c ){
		if( this.mapModel != null ){
			return this.mapModel.getCharacterPositionCost( c );
		}else{
			return float.MaxValue;
		}
	}

	void Awake(){
		setMap();
	}

	// Use this for initialization
	void Start () {
		/*ChromosomeSalesman solution = geneticController.getSolution();
		for( int i = 0; i < mapModel.Targets.Count; i++  ){
			Debug.Log( "Target "+i+" : "+mapModel.Targets[i].getPosX()+" "+mapModel.Targets[i].getPosY() );
		}
		Debug.Log( solution.toString() );*/
	}

	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.Space)){
			// Replay
			this.createPlayers( true );
		}
		if(Input.GetKeyDown(KeyCode.KeypadEnter)){
			// Replay
			this.createPlayers( false );
		}*/
	}

	public void nextGeneration(){ 	// or next solution
		this.createPlayers( false );
	}
	public void replayGeneration(){ // or solution
		this.createPlayers( true );
	}
	public int getCurrentGeneration(){
		return geneticController.getCurrentGeneration();
	}
	public int getMaxGeneration(){
		return geneticController.getMaxGeneration();
	}
	public float getSolutionFitness(){
		if( showBestSolution && solution != null ){
			return solution.fitness();
		}else if( !showBestSolution && solutions != null && solutions.Count > 0 ){
			return solutions[0].fitness();
		}
		return -1;
	}

	void createPlayers( bool replay ){
		if( showBestSolution ){
			if( !replay ){
				solution = geneticController.getSolution();
			}

			if( solution != null ){
				players = new List<GeneticPlayer>();
				players.Add( new GeneticPlayer( new Vector2(0,0), this.mapModel, solution, false, solution.fitness() ));
				this.GetComponent<MapGeneticView>().setPlayers( players );
			}
		}
		else{
			if( !replay ){
				List<ChromosomeSalesman> currentSolutions = geneticController.getPartialSolution();
				if( currentSolutions != null ){
					solutions = currentSolutions;
				}
			}

			if( solutions != null ){
				float worstFitness = 0;
				foreach( ChromosomeSalesman solution in solutions ){
					float fitness = solution.fitness();
					if( fitness > worstFitness ){
						worstFitness = fitness;
					}
				}

				players = new List<GeneticPlayer>();
				// create players
				for(int i=0; i<solutions.Count; i++){
					players.Add( new GeneticPlayer( new Vector2(0,0), this.mapModel, solutions[i], i==0, worstFitness ));
				}
				this.GetComponent<MapGeneticView>().setPlayers( players );
			}
		}
	}

	public void selectShowSolutionMode( bool showBestSolution ){
		this.showBestSolution = showBestSolution;

		//this.createPlayers( false );
	}
}
