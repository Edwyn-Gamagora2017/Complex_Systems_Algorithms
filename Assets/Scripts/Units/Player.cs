using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	/*
	 * Attributes
	 */
	private bool freeMove = false;	// player can move without restrictions of time

	/*
	 * CONSTRUCTOR
	 */
	public Player( Vector2 position, Map map ) : base( position, map ){
		this.freeMove = true;
	}

	/*
	 * GETTERS AND SETTERS
	 */
	public bool FreeMove {
		get {
			return freeMove;
		}
	}
	public void changeFreeMove(){
		this.freeMove = !this.freeMove;
	}
}
