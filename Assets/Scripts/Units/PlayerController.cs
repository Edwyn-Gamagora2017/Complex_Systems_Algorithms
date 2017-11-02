using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character_Controller {
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		// Handling mavements
		if( Input.GetKeyDown(KeyCode.DownArrow) ){
			// move player
			this.model.moveDown();
		}
		if( Input.GetKeyDown(KeyCode.UpArrow) ){
			// move player
			this.model.moveUp();
		}
		if( Input.GetKeyDown(KeyCode.RightArrow) ){
			// move player
			this.model.moveRight();
		}
		if( Input.GetKeyDown(KeyCode.LeftArrow) ){
			// move player
			this.model.moveLeft();
		}
	}
}
