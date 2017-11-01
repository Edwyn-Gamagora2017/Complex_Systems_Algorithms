using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	Map mapModel;			// stores the map information

	[SerializeField]
	TextAsset mapFile;		// File that describes the map

	[SerializeField]
	MapView mapView;		// MapView component

	// To be executed before the component starts
	void Awake () {
		// Create the map based on the file
		if( this.mapFile != null ){
			this.mapModel = Map.read( this.mapFile.text );
			if( this.mapView != null ){
				this.mapView.MapModel = this.mapModel;
			}
		}
		else{
			Debug.LogError( "MapView : mapFile not defined" );
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
