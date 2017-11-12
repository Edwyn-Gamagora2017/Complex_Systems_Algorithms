using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	/*
	 * Attributes
	 */
	protected bool aStar;

	/*
	 * CONSTRUCTOR
	 */
	public Enemy( Vector2 position, Map map, bool aStar ) : base( position, map ){
		this.aStar = aStar;
	}

	/*
	 * GETTERS
	 */
	public bool AStar {
		get {
			return aStar;
		}
		set {
			this.aStar = value;
		}
	}
}
