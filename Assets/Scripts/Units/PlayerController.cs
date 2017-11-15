using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character_Controller {

	private float playerMovementTimer = 0;	// Timer for player's movement

	// Use this for initialization
	protected override void Start () {
		base.Start();

		this.playerMovementTimer = Character_Controller.moveTimeInSec * (this.model.positionInTheMapCost()+1);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if (((Player)this.model).FreeMove) {
			handleKeyboard();
		} else if( playerMovementTimer <= 0 ){
			handleKeyboard();
			// TODO Consider edge cost
			this.playerMovementTimer = Character_Controller.moveTimeInSec * (this.model.positionInTheMapCost()+1);
		}
		else{
			this.playerMovementTimer -= Time.deltaTime;
		}
	}

	void handleKeyboard(){
		// Handling mavements: if the player is free to move, it must consider only keyDown
		if( Input.GetKeyDown(KeyCode.DownArrow) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.DownArrow)) ){
			// move player
			this.model.moveDown();
		}
		if( Input.GetKeyDown(KeyCode.UpArrow) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.UpArrow)) ){
			// move player
			this.model.moveUp();
		}
		if( Input.GetKeyDown(KeyCode.RightArrow) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.RightArrow)) ){
			// move player
			this.model.moveRight();
		}
		if( Input.GetKeyDown(KeyCode.LeftArrow) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.LeftArrow)) ){
			// move player
			this.model.moveLeft();
		}
	}

	// Mouse is over the object
	void OnMouseOver(){
		// Handling click
		if( Input.GetMouseButtonDown( 1 )){	// Right click
			((Player)this.model).changeFreeMove();
		}
	}
}
