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
		// Showing the path found
		if( this.showPathFlag ){
			this.map.showPath( path, new Color(1,0,0) );
		}
	}
}
