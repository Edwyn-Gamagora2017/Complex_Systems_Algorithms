using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character_Controller {

	// Game Settings
	[SerializeField]
	bool enemyMoveEnabled = true;	// Indicates if the enemy's movement is enabled

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
		this.enemyMovementTimer = ( this.enemyMoveEnabled ? Character_Controller.moveTimeInSec * (this.map.getCharacterPositionCost( this.model )+1) : 0 );
	}
	
	// Update is called once per frame
	protected override void Update (){
		base.Update();

		if( enemyMovementTimer <= 0 ){
			executeMovement();
			// TODO Consider edge cost
			this.enemyMovementTimer = Character_Controller.moveTimeInSec * ( this.enemyMoveEnabled ? (this.map.getCharacterPositionCost( this.model )+1) : 1 );
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
		if( Input.GetMouseButtonDown( 1 )){	// Right click
			this.changeFindingPath();
		}
	}

	void executeMovement(){
		// Find Path
		List<PathVertexInfo> pathAstar = this.model.findPathAstar();
		List<PathVertexInfo> pathDijkstra = this.model.findPathDijkstra();
		// Move enemy
		List<PathVertexInfo> path = (((Enemy)this.model).AStar?pathAstar:pathDijkstra);
		if( path != null && path.Count > 1 && enemyMoveEnabled ){
			Map.TileInfo v = ((Map.TileInfo)path[ path.Count-2 ].Vertex);
			this.model.move( v.x, v.y );
			// Removing the last vertex of the path because it was used by the character
			pathAstar.RemoveAt( pathAstar.Count-1 );
			pathDijkstra.RemoveAt( pathDijkstra.Count-1 );
		}
		// Showing the path found
		if( this.showPathFlag ){
			List<KeyValuePair<List<PathVertexInfo>,Color>> paths = new List<KeyValuePair<List<PathVertexInfo>,Color>>();
			paths.Add( new KeyValuePair<List<PathVertexInfo>, Color>( pathAstar, this.GetComponent< EnemyView >().AStar ) );
			paths.Add( new KeyValuePair<List<PathVertexInfo>, Color>( pathDijkstra, this.GetComponent< EnemyView >().Dijkstra ) );
			this.map.includePaths( paths, this.model );
		}
	}

	public void changeMovementEnabled(){
		this.enemyMoveEnabled = !this.enemyMoveEnabled;
	}
	public void changeFindingPath(){
		((Enemy)this.model).AStar = !((Enemy)this.model).AStar;
	}
}
