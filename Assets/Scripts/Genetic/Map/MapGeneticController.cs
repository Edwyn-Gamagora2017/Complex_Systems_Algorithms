using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneticController : MonoBehaviour {

	MapGenetic mapModel;			// stores the map information
	[SerializeField]
	TextAsset mapPath;

	// To be executed before the component starts
	void Awake () {
	}

	public bool setMap(){
		// Create the map based on the file
		this.mapModel = MapGenetic.read( mapPath.text );
		if( this.GetComponent<MapGeneticView>() != null ){
			this.GetComponent<MapGeneticView>().MapModel = this.mapModel;
		}

		return this.mapModel != null;
	}

	// Use this for initialization
	void Start () {
		setMap ();
	}

	// Update is called once per frame
	void Update () {
	}
}
