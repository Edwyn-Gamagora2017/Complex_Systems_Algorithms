using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBoid : MonoBehaviour {

	public bool collisionPosition( Vector3 pos ){
		return pos.x >= this.transform.position.x - this.transform.localScale.x/2 && pos.x <= this.transform.position.x + this.transform.localScale.x/2 &&
			pos.y >= this.transform.position.y - this.transform.localScale.y/2 && pos.y <= this.transform.position.y + this.transform.localScale.y/2;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
