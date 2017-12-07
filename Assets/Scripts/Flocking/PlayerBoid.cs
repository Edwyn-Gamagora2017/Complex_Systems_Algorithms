using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoid : MonoBehaviour {

	private float moveStep = 0.5f;

	[SerializeField]
	Camera camera = null;

	[SerializeField]
	GameObject borders;

	private float width;
	private float height;

	[SerializeField]
	private ObstacleCollider obstacleCollider;

	// Use this for initialization
	void Start () {
		height = borders.transform.localScale.y;
		width = borders.transform.localScale.x;

		if( camera == null ){
			camera = Camera.main;
			camera.orthographicSize = height/2;
			camera.aspect = width/height;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 oldPosition = transform.position;
		if( Input.GetKey( KeyCode.UpArrow ) ){
			UpdatePosition( new Vector3( oldPosition.x, oldPosition.y + moveStep, oldPosition.z ) );
		}
		if( Input.GetKey( KeyCode.DownArrow ) ){
			UpdatePosition( new Vector3( oldPosition.x, oldPosition.y - moveStep, oldPosition.z ) );
		}
		if( Input.GetKey( KeyCode.RightArrow ) ){
			UpdatePosition( new Vector3( oldPosition.x + moveStep, oldPosition.y, oldPosition.z ) );
		}
		if( Input.GetKey( KeyCode.LeftArrow ) ){
			UpdatePosition( new Vector3( oldPosition.x - moveStep, oldPosition.y, oldPosition.z ) );
		}
	}

	void UpdatePosition( Vector3 newPosition ){
		if( newPosition.x > -width/2f && newPosition.x < width/2f && newPosition.y > -height/2f && newPosition.y < height/2f ){
			foreach( ObstacleBoid obst in obstacleCollider.getCollidingObstacles() ){
				if( obst.collisionPosition( newPosition ) ){
					return;
				}
			}
			this.transform.position = newPosition;
		}
	}
}
