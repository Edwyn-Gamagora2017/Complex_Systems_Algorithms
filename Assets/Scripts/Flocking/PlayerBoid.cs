using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoid : MonoBehaviour {

	private float moveStep = 0.3f;

	[SerializeField]
	Camera camera = null;
	private float width;
	private float height;

	[SerializeField]
	private ObstacleCollider obstacleCollider;

	// Use this for initialization
	void Start () {
		if( camera == null ){
			camera = Camera.main;
		}

		height = this.camera.orthographicSize*2;
		width = height*this.camera.aspect;
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
