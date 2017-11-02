using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

	Vector2 position;
	Map map;

	public Character( Vector2 pos, Map map ){
		this.position = pos;
		this.map = map;
	}

	public Vector2 positionInTheMap(){
		return new Vector2( this.map.getCharacterPositionX(this), this.map.getCharacterPositionY(this) );
	}
	public List<PathVertexInfo> findPath(){
		return this.map.findPath( this );
	}

	public float getPosX(){
		return this.position.x;
	}
	public float getPosY(){
		return this.position.y;
	}
	public void setPos( Vector2 newPosition ){
		this.position = newPosition;
	}

	// Character Movements
	public void move(float newX, float newY){
		this.map.characterMove( this, newX, newY );
	}
	public void moveDown(){
		Vector2 pos = this.positionInTheMap();
		this.move( pos.x, pos.y-1 );
	}
	public void moveUp(){
		Vector2 pos = this.positionInTheMap();
		this.move( pos.x, pos.y+1 );
	}
	public void moveLeft(){
		Vector2 pos = this.positionInTheMap();
		this.move( pos.x-1, pos.y );
	}
	public void moveRight(){
		Vector2 pos = this.positionInTheMap();
		this.move( pos.x+1, pos.y );
	}
}
