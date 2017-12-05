using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour {

	public GameObject boidPrefab;

	[SerializeField]
	private int amountBoids = 10;

	[SerializeField]
	private GameObject player;

	[SerializeField]
	private GameObject border;

	private List<GameObject> allBoids;
	private List<GameObject> allObstacles;

	public List<GameObject> AllBoids {
		get {
			return allBoids;
		}
	}
	public List<GameObject> AllObstacles {
		get {
			return allObstacles;
		}
	}

	// Use this for initialization
	void Start () {
		allBoids = new List<GameObject>();
		allObstacles = new List<GameObject>();

		for( int i = 0; i < amountBoids; i++ ){
			GameObject boid = Instantiate( boidPrefab );
			boid.GetComponent<BoidBehaviour>().setTarget( player );
			boid.GetComponent<WallCollider>().setBorders( border );
			boid.GetComponent<BoidBehaviour>().manager = this;
			allBoids.Add(boid);
		}

		foreach( GameObject obst in GameObject.FindGameObjectsWithTag( "BoidObstacle" )){
			allObstacles.Add( obst );
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(KeyCode.Mouse0) ){
			BoidBehaviour.considerAngleOfView = !BoidBehaviour.considerAngleOfView;
		}
	}
}
