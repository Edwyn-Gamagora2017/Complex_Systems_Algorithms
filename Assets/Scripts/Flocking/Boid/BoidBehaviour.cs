using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

	[SerializeField]
	private FlockingCollider repulsionCollider;
	[SerializeField]
	private FlockingCollider alignmentCollider;
	[SerializeField]
	private FlockingCollider attractionCollider;
	[SerializeField]
	private ObstacleCollider obstacleCollider;

	private Vector3 velocity; // orientation and module of velocity
	private float maxSpeed = 0.4f;	// the maximum value for the module of the velocity vector
	private float maxRotationAngleStepDegrees = 15;	// The boid is not allowed to rotate more than this value

	[SerializeField]
	GameObject target;			// target to be tracked by the boid

	[SerializeField]
	float angleFieldView = 90;	// Angle of the Field View (degrees)

	List<BoidBehaviour> selectFieldView( List<BoidBehaviour> insideRadius ){
		return insideRadius;
		List<BoidBehaviour> result = new List<BoidBehaviour>();
		foreach( BoidBehaviour b in insideRadius ){
			Vector3 directionToBoid = b.gameObject.transform.position - this.transform.position;
			float angle = Vector3.Angle( this.velocity, directionToBoid );
			if( angle >= -angleFieldView/2f && angle <= angleFieldView/2f ){
				result.Add( b );
			}
		}
		return result;
	}

	/**
	 * The boid will move close to the ones that are near
	 * 		It obtains the average of positions
	 */
	Vector3 moveCloser(){
		Vector3 averagePosition = new Vector3();

		List<BoidBehaviour> boids = selectFieldView( attractionCollider.getCollidingBoids() );
		foreach( BoidBehaviour boid in boids ){
			// average of vector from the boid to the close one
			averagePosition += boid.transform.position - this.transform.position;
		}
		if (boids.Count > 0) {
			averagePosition /= boids.Count;
		}

		return averagePosition / 40;
	}

	/**
	 * The boid will move with the same velocity of the ones that are near
	 * 		It obtains the average of velocities
	 */
	Vector3 moveWith(){
		Vector3 averageVelocity = new Vector3();

		List<BoidBehaviour> boids = selectFieldView( alignmentCollider.getCollidingBoids() );
		foreach( BoidBehaviour boid in boids ){
			// average of differences between the velocities
			averageVelocity += boid.getVelocity() - this.velocity;
		}
		if (boids.Count > 0) {
			averageVelocity /= boids.Count;
		}

		return averageVelocity / 5;
	}

	/**
	 * The boid will move away from the the ones that are too close
	 * 		It obtains the average of positions
	 */
	Vector3 moveAway(){
		Vector3 averagePosition = new Vector3();

		List<BoidBehaviour> boids = selectFieldView( repulsionCollider.getCollidingBoids() );
		foreach( BoidBehaviour boid in boids ){
			averagePosition += boid.transform.position - this.transform.position;
		}
		if (boids.Count > 0) {
			averagePosition /= boids.Count;
		}

		return -( averagePosition / 10 );
	}

	/**
	 * The boid will move towards the target
	 * 		based on its position
	 */
	Vector3 moveTarget(){
		Vector3 targetDirection = target.gameObject.transform.position - transform.position;

		return targetDirection.normalized / 100;
	}

	/**
	 * The boid will avoid the obstacles
	 * 		based on their positions and distance
	 */
	Vector3 avoidObstacles(){
		Vector3 averageDistance = new Vector3();

		List<ObstacleBoid> obstacles = obstacleCollider.getCollidingObstacles();
		float colliderDistance = obstacleCollider.getRadius();
		foreach( ObstacleBoid obst in obstacles ){
			// The closer to the object, the bigger the distance added to the average
			Vector3 dirToObst = obst.transform.position - transform.position;
			/*float distancefromObstacleToCollider = colliderDistance - dirToObst.magnitude;
			averageDistance += dirToObst.normalized*distancefromObstacleToCollider;*/
			averageDistance += dirToObst;
		}
		if (obstacles.Count > 0) {
			averageDistance /= obstacles.Count;
		}

		//return -( averageDistance / (10*colliderDistance) );
		return -( averageDistance / 185 );
	}

	/**
	 * The boid will move
	 * 		It executes the functions to control the boid
	 */
	void move(){
		Vector3 partialVelocity = this.moveCloser ();
		partialVelocity 		+= this.moveWith ();
		partialVelocity 		+= this.moveAway ();
		partialVelocity 		+= this.moveTarget();
		partialVelocity 		+= this.avoidObstacles();
			
		// Move boid
		this.setVelocity( this.velocity+partialVelocity, true );
	}

	public Vector3 getVelocity(){
		return this.velocity;
	}

	public void setVelocity( Vector3 newVelocity, bool smoothRotation ){
		// Avoiding high angle rotation
		if( smoothRotation && Vector3.Angle( this.velocity.normalized, newVelocity.normalized ) > maxRotationAngleStepDegrees ){
			//newVelocity = Quaternion.Euler( 0,0,maxRotationAngleStepDegrees )*this.velocity;
			Vector3 rotationVelocity = Vector3.RotateTowards( this.velocity.normalized, newVelocity.normalized, maxRotationAngleStepDegrees*Mathf.Deg2Rad, 0f );
			rotationVelocity *= newVelocity.magnitude;
			rotationVelocity.Scale( new Vector3(1,1,0) );
			newVelocity = rotationVelocity;
		}
		this.velocity = newVelocity;

		// Check velocity module
		float currentSpeed = this.velocity.magnitude;
		if( currentSpeed > this.maxSpeed ){
			float factor = this.maxSpeed / currentSpeed;
			this.velocity.Scale ( new Vector3( factor, factor, factor ) );
		}

		this.transform.rotation = Quaternion.FromToRotation( Vector3.right, this.velocity );

		setPosition( this.transform.position + this.velocity );
	}

	// Check if the obstacles does not block the movement
	public void setPosition( Vector3 newPosition ){
		foreach( ObstacleBoid obst in obstacleCollider.getCollidingObstacles() ){
			if( obst.collisionPosition( newPosition ) ){
				return;
			}
		}
		this.transform.position = newPosition;
	}

	public void setTarget( GameObject newTarget ){
		this.target = newTarget;
	}

	// Use this for initialization
	void Awake () {
		//this.velocity = new Vector3 (0.5f,0.1f,0);
		this.velocity = new Vector3 (1/(float)Random.Range(1,10),1/(float)Random.Range(1,10),0);
		this.transform.position = new Vector3 (Random.Range(1,10),Random.Range(1,10),0);
	}

	void Start () {
		//InvokeRepeating ( "move", 0, 0.05f );
	}
	
	// Update is called once per frame
	void Update () {
		move ();
	}
}
