using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

	Vector2 pos;
	Map map;

	public Character( Vector2 pos, Map map ){
		this.pos = pos;
		this.map = map;
	}

	public Vector2 positionInTheMap(){
		return this.map.getCharacterPosition(this);
	}

	public Vector2 Pos {
		set {
			pos = value;
		}
	}
	public float getPosX(){
		return this.pos.x;
	}
	public float getPosY(){
		return this.pos.y;
	}
}
