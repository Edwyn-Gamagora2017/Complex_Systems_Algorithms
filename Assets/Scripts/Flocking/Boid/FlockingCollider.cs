using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingCollider : MonoBehaviour {

	List<BoidBehaviour> collidingBoids ;

	void Awake () {
		this.collidingBoids = new List<BoidBehaviour>();
	}

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public List<BoidBehaviour> getCollidingBoids(){
		return this.collidingBoids;
	}
	public float getRadius(){
		return this.gameObject.GetComponent< CircleCollider2D >().radius;
	}

	// Some boids entered the zone
	void OnTriggerEnter2D( Collider2D col ){
		// Check if it a boid
		BoidBehaviour boid = col.gameObject.GetComponent<BoidBehaviour>();
		if( boid != null ){
			collidingBoids.Add( boid );
		}
	}

	// Some boids exited the zone
	void OnTriggerExit2D( Collider2D col ){
		// Check if it a boid
		BoidBehaviour boid = col.gameObject.GetComponent<BoidBehaviour>();
		if( boid != null ){
			collidingBoids.Remove( boid );
		}
	}
}
