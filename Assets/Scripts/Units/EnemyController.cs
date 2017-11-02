using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character_Controller {

	// Game Settings
	[SerializeField]
	int enemyMoveTimeInSec = 10;	// Interval in which enemy's movement is executed
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
	}
	
	// Update is called once per frame
	protected override void Update (){
		base.Update();

		if( enemyMovementTimer <= 0 && enemyMoveEnabled ){
			enemyMovementTimer = enemyMoveTimeInSec;
			executeMovement();
		}
		else{
			enemyMovementTimer -= Time.deltaTime;
		}
	}

	void executeMovement(){
		// Find Path
		Debug.Log("Find Path");
		List<PathVertexInfo> path = this.model.findPath();
		// Move enemy
		if( path != null && path.Count > 1){
			this.model.move( ((Map.TileInfo)path[ path.Count-2 ].Vertex).x, ((Map.TileInfo)path[ path.Count-2 ].Vertex).y );
		}
		// Showing the path found
		if( this.showPathFlag ){
			this.map.includePath( path, this.model );
		}
	}
}
