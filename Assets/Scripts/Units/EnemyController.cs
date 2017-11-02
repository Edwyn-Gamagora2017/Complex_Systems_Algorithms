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

	private MapController map;				// Controller to interact to the Map

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
		Debug.Log("Find Path");
		//this.model.findPath();
	}
}
