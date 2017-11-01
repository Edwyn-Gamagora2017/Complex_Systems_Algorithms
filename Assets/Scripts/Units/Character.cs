using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
	Vector2 pos;

	public Character( Vector2 pos ){
		this.pos = pos;
	}

	public Vector2 Pos {
		get {
			return pos;
		}
		set {
			pos = value;
		}
	}
}
