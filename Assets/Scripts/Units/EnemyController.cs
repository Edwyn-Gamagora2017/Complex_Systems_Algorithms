using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character_Controller {

	// Game Settings
	[SerializeField]
	int enemyMoveTimeInSec = 1;	// Interval in which enemy's movement is executed
	[SerializeField]
	bool enemyMoveEnabled = true;	// Indicates if the enemy's movement is enabled
	[SerializeField]
	bool moveAstar = true;			// Indicates if the enemy's movement is based on Astar algorithm

	private float enemyMovementTimer = 0;	// Timer for enemy's movement

	private MapController map;		// Controller to interact to the Map
	public MapController Map {
		set {
			map = value;
		}
	}

	// Settings
	private bool showPathFlag = true;				// Indicates if the map shows some path

	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.enemyMovementTimer = ( this.enemyMoveEnabled ? enemyMoveTimeInSec*(this.map.getCharacterPositionCost( this.model )+1) : 0 );
	}
	
	// Update is called once per frame
	protected override void Update (){
		base.Update();

		if( enemyMovementTimer <= 0 ){
			executeMovement();
			// TODO Consider edge cost
			this.enemyMovementTimer = enemyMoveTimeInSec* ( this.enemyMoveEnabled ? (this.map.getCharacterPositionCost( this.model )+1) : 1 );
		}
		else{
			enemyMovementTimer -= Time.deltaTime;
		}
	}

	// Mouse is over the object
	void OnMouseOver(){
		// Handling click
		if( Input.GetMouseButtonDown( 0 ) ){	// Left click
			this.changeMovementEnabled();
		}
	}

	void executeMovement(){
		// Find Path
		Debug.Log("Find Path");
		List<PathVertexInfo> pathAstar = this.model.findPathAstar();
		List<PathVertexInfo> pathDijkstra = this.model.findPathDijkstra();
		// Move enemy
		List<PathVertexInfo> path = (this.moveAstar?pathAstar:pathDijkstra);
		if( path != null && path.Count > 1 && enemyMoveEnabled ){
			Map.TileInfo v = ((Map.TileInfo)path[ path.Count-2 ].Vertex);
			this.model.move( v.x, v.y );
		}
		// Showing the path found
		if( this.showPathFlag ){
			List<KeyValuePair<List<PathVertexInfo>,Color>> paths = new List<KeyValuePair<List<PathVertexInfo>,Color>>();
			paths.Add( new KeyValuePair<List<PathVertexInfo>, Color>( pathAstar, new Color(1,0,0)) );
			paths.Add( new KeyValuePair<List<PathVertexInfo>, Color>( pathDijkstra, new Color(0,0,1)) );
			this.map.includePaths( paths, this.model );
		}
	}

	public void changeMovementEnabled(){
		this.enemyMoveEnabled = !this.enemyMoveEnabled;
	}
}
