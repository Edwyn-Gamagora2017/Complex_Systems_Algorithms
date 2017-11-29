using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollider : MonoBehaviour {

	List<ObstacleBoid> collidingObstacles ;

	void Awake () {
		this.collidingObstacles = new List<ObstacleBoid>();
	}

	// Use this for initialization
	void Start () {}

	// Update is called once per frame
	void Update () {}

	public List<ObstacleBoid> getCollidingObstacles(){
		return this.collidingObstacles;
	}
	public float getRadius(){
		return this.gameObject.GetComponent< CircleCollider2D >().radius;
	}

	// Some obstacles entered the zone
	void OnTriggerEnter2D( Collider2D col ){
		// Check if it is an obstacle
		ObstacleBoid obstacle = col.gameObject.GetComponent<ObstacleBoid>();
		if( obstacle != null ){
			collidingObstacles.Add( obstacle );
		}
	}

	// Some obstacles exited the zone
	void OnTriggerExit2D( Collider2D col ){
		// Check if it is an obstacle
		ObstacleBoid obstacle = col.gameObject.GetComponent<ObstacleBoid>();
		if( obstacle != null ){
			collidingObstacles.Remove( obstacle );
		}
	}
}
