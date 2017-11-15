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
			// if some movement was executed
			if( handleKeyboard() ){
				// TODO Consider edge cost
				this.playerMovementTimer = Character_Controller.moveTimeInSec * (this.model.positionInTheMapCost()+1);
			}
		}
		else{
			this.playerMovementTimer -= Time.deltaTime;
		}
	}

	// returns true if some movement was executed
	bool handleKeyboard(){
		// Handling mavements: if the player is free to move, it must consider only keyDown
		if( Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2) || (!((Player)this.model).FreeMove && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Keypad2))) ){
			// move player
			this.model.moveDown();
			return true;
		}
		if( Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8) || (!((Player)this.model).FreeMove && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Keypad8))) ){
			// move player
			this.model.moveUp();
			return true;
		}
		if( Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6) || (!((Player)this.model).FreeMove && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6))) ){
			// move player
			this.model.moveRight();
			return true;
		}
		if( Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4) || (!((Player)this.model).FreeMove && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4))) ){
			// move player
			this.model.moveLeft();
			return true;
		}

		// Diagonal moves
		if( Input.GetKeyDown(KeyCode.Keypad7) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.Keypad7)) ){
			// move player
			this.model.moveUpLeft();
			return true;
		}
		if( Input.GetKeyDown(KeyCode.Keypad9) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.Keypad9)) ){
			// move player
			this.model.moveUpRight();
			return true;
		}
		if( Input.GetKeyDown(KeyCode.Keypad1) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.Keypad1)) ){
			// move player
			this.model.moveDownLeft();
			return true;
		}
		if( Input.GetKeyDown(KeyCode.Keypad3) || (!((Player)this.model).FreeMove && Input.GetKey(KeyCode.Keypad3)) ){
			// move player
			this.model.moveDownRight();
			return true;
		}
		return false;
	}

	// Mouse is over the object
	void OnMouseOver(){
		// Handling click
		if( Input.GetMouseButtonDown( 1 )){	// Right click
			((Player)this.model).changeFreeMove();
		}
	}
}
