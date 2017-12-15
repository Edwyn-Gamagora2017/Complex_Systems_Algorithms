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

	// To be executed before the component starts
	void Awake () {}

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
		this.selectShowSolutionMode( showBestSolution );

		return this.mapModel != null;
	}

	public float getCharacterPositionCost( GeneticCharacter c ){
		if( this.mapModel != null ){
			return this.mapModel.getCharacterPositionCost( c );
		}else{
			return float.MaxValue;
		}
	}
		
	// Use this for initialization
	void Start () {
		/*ChromosomeSalesman solution = geneticController.getSolution();
		for( int i = 0; i < mapModel.Targets.Count; i++  ){
			Debug.Log( "Target "+i+" : "+mapModel.Targets[i].getPosX()+" "+mapModel.Targets[i].getPosY() );
		}
		Debug.Log( solution.toString() );*/
		setMap();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			// Replay
			this.createPlayers( true );
		}
		if(Input.GetKeyDown(KeyCode.KeypadEnter)){
			// Replay
			this.createPlayers( false );
		}
	}

	void createPlayers( bool replay ){
		if( showBestSolution ){
			if( !replay ){
				solution = geneticController.getSolution();
			}

			players = new List<GeneticPlayer>();
			players.Add( new GeneticPlayer( new Vector2(0,0), this.mapModel, solution, false, solution.fitness() ));
			this.GetComponent<MapGeneticView>().setPlayers( players );
		}
		else{
			if( !replay ){
				List<ChromosomeSalesman> currentSolutions = geneticController.getPartialSolution();
				if( currentSolutions != null ){
					solutions = currentSolutions;
				}
			}

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

	void selectShowSolutionMode( bool showBestSolution ){
		this.showBestSolution = showBestSolution;

		geneticController = GetComponent<GeneticSceneController>();
		geneticController.setGraph( this.mapModel.Graph, cities );
		this.createPlayers( false );
	}
}
