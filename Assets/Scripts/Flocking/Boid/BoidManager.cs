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

	// Use this for initialization
	void Start () {
		for( int i = 0; i < amountBoids; i++ ){
			GameObject boid = Instantiate( boidPrefab );
			boid.GetComponent<BoidBehaviour>().setTarget( player );
			boid.GetComponent<WallCollider>().setBorders( border );
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
