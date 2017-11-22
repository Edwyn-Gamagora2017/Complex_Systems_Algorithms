using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

	[SerializeField]
	FlockingCollider closeCollider;
	[SerializeField]
	FlockingCollider moveCollider;

	/**
	 * The boid will move close to the ones that are near
	 * 		It obtains the average of positions
	 */
	void moveCloser(){
		Vector3 average = new Vector3();

		List<BoidBehaviour> boids = closeCollider.getCollidingBoids();
		foreach( BoidBehaviour boid in boids ){
			average += boid.transform.position - this.transform.position;
		}
		average /= boids.Count;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
