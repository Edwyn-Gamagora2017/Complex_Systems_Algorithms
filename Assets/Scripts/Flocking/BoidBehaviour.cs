using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

	[SerializeField]
	private FlockingCollider closeCollider;
	[SerializeField]
	private FlockingCollider moveCollider;

	private Vector3 velocity; // orientation and module of velocity
	private float maxSpeed = 2;	// the maximum value for the module of the velocity vector

	/**
	 * The boid will move close to the ones that are near
	 * 		It obtains the average of positions
	 */
	/*void moveCloser(){
		Vector3 averagePosition = new Vector3();

		List<BoidBehaviour> boids = moveCollider.getCollidingBoids();
		foreach( BoidBehaviour boid in boids ){
			// average of vector from the boid to the close one
			averagePosition += boid.transform.position - this.transform.position;
		}
		if (boids.Count > 0) {
			averagePosition /= boids.Count;
		}

		this.velocity += averagePosition / 100;
	}*/

	/**
	 * The boid will move with the same velocity of the ones that are near
	 * 		It obtains the average of velocities
	 */
	/*void moveWith(){
		Vector3 averageVelocity = new Vector3();

		List<BoidBehaviour> boids = moveCollider.getCollidingBoids();
		foreach( BoidBehaviour boid in boids ){
			// average of differences between the velocities
			averageVelocity += boid.getVelocity() - this.velocity;
		}
		if (boids.Count > 0) {
			averageVelocity /= boids.Count;
		}

		this.velocity += averageVelocity / 10;
	}*/

	/**
	 * The boid will move away from the the ones that are too close
	 * 		It obtains the average of positions
	 */
	/*void moveAway(){
		Vector3 averagePosition = new Vector3();

		List<BoidBehaviour> boids = closeCollider.getCollidingBoids();
		foreach( BoidBehaviour boid in boids ){
			averagePosition += boid.transform.position - this.transform.position;
		}
		if (boids.Count > 0) {
			averagePosition /= boids.Count;
		}

		this.velocity += averagePosition / 10;
	}*/

	/**
	 * The boid will move
	 * 		It executes the functions to control the boid
	 */
	/*void move(){
		this.moveCloser ();
		this.moveWith ();
		this.moveAway ();

		// Check velocity module
		float currentSpeed = this.velocity.magnitude;
		if( currentSpeed > this.maxSpeed ){
			float factor = this.maxSpeed / currentSpeed;
			this.velocity.Scale ( new Vector3( factor, factor, factor ) );
		}

		Debug.Log (this.velocity);
		// Move boid
		this.transform.Translate( this.velocity );
	}*/

	public Vector3 getVelocity(){
		return this.velocity;
	}

	// Use this for initialization
	void Awake () {
		this.velocity = new Vector3 (0,0,0);
	}

	void Start () {
		//InvokeRepeating ( "move", 0, 0.3f );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter( Collider col ){
		Debug.Log ("dsdq");
	}
}
