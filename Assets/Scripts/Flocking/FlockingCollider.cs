using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingCollider : MonoBehaviour {

	List<BoidBehaviour> collidingBoids ;

	// Use this for initialization
	void Start () {
		this.collidingBoids = new List<BoidBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<BoidBehaviour> getCollidingBoids(){
		return this.collidingBoids;
	}

	// Some boid entered the zone
	void OnTriggerEnter( Collider col ){
		// Check if it a boid
		BoidBehaviour boid = col.gameObject.GetComponent<BoidBehaviour>();
		if( boid != null ){
			collidingBoids.Add( boid );
		}
	}

	// Some boid exited the zone
	void OnTriggerExit( Collider col ){
		// Check if it a boid
		BoidBehaviour boid = col.gameObject.GetComponent<BoidBehaviour>();
		if( boid != null ){
			collidingBoids.Remove( boid );
		}
	}
}
