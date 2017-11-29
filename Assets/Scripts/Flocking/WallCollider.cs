using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour {

	[SerializeField]
	Camera camera = null;
	[SerializeField]
	bool invertDirection = false;

	private float width;
	private float height;

	private BoidBehaviour boid;

	public void setCamera( Camera newCamera ){
		this.camera = newCamera;
	}

	// Use this for initialization
	void Start () {
		if( camera == null ){
			camera = Camera.main;
		}

		height = this.camera.orthographicSize*2;
		width = height*this.camera.aspect;

		boid = GetComponent<BoidBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		checkBounds();
	}

	void checkBounds(){
		if( invertDirection ){
			// Invert movement
			Vector3 oldVelocity = boid.getVelocity();
			Vector3 normal = new Vector3();
			bool collision = false;

			if( transform.position.x > width/2f ){
				// Right Bound
				normal = Vector3.left;
				collision = true;
			} else if( transform.position.x < -width/2f ){
				// left Bound
				normal = Vector3.right;
				collision = true;
			} else if( transform.position.y > height/2f ){
				// Top Bound
				normal = Vector3.down;
				collision = true;
			} else if( transform.position.y < -height/2f ){
				// Bottom Bound
				normal = Vector3.up;
				collision = true;
			}

			if( collision ){
				float scalaire = Vector3.Dot( oldVelocity.normalized, normal.normalized );

				Vector3 newVelocity = (-2)*scalaire*normal + oldVelocity.normalized;
				newVelocity *= oldVelocity.magnitude;
				boid.setVelocity( newVelocity );
			}
		}
		else{
			// Move to opposite bound
			if( transform.position.x > width/2f ){
				transform.Translate( new Vector3( -width,0,0 ) );
			}
			else if( transform.position.x < -width/2f ){
				transform.Translate( new Vector3( width,0,0 ) );
			}
			else if( transform.position.y > height/2f ){
				transform.Translate( new Vector3( 0,-height,0 ) );
			}
			else if( transform.position.y < -height/2f ){
				transform.Translate( new Vector3( 0,height,0 ) );
			}
		}
	}
}
