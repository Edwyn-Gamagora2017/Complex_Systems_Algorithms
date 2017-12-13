using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneticController : MonoBehaviour {

	MapGenetic mapModel;			// stores the map information
	[SerializeField]
	TextAsset mapPath;

	GeneticSceneController geneticController;
	GeneticPlayer player;

	// To be executed before the component starts
	void Awake () {
		geneticController = GetComponent<GeneticSceneController>();

		setMap ();
	}

	public bool setMap(){
		// Create the map based on the file
		this.mapModel = MapGenetic.read( mapPath.text );

		player = new GeneticPlayer( new Vector2(0,0), this.mapModel, geneticController );

		// Setting map to view
		if( this.GetComponent<MapGeneticView>() != null ){
			this.GetComponent<MapGeneticView>().Player = player;
			this.GetComponent<MapGeneticView>().MapModel = this.mapModel;
			this.GetComponent<MapGeneticView>().MapController = this;
		}
			
		this.mapModel.Graph.floydWarshall();	// Calculate all paths
		// Setting graph to genetic controller
		List<SalesmanCity> cities = new List<SalesmanCity>();
		for( int i = 0; i < mapModel.Targets.Count; i++  ){
			// For each target, create a city indicating the index of the vertex
			cities.Add( new SalesmanCity( i, mapModel.getCharacterGraphPosition( mapModel.Targets[i] ) ) );
		}
		geneticController.setGraph( this.mapModel.Graph, cities );

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
	}

	// Update is called once per frame
	void Update () {
	}
}
