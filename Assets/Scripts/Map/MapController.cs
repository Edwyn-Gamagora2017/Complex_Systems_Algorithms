using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	Map mapModel;			// stores the map information

	[SerializeField]
	TextAsset mapFile;		// File that describes the map

	[SerializeField]
	MapView mapView;		// MapView component

	[SerializeField]
	GameObject[] players;		// Players	// TODO remove serialize
	[SerializeField]
	GameObject[] enemies;		// Enemies	// TODO remove serialize

	private List<Character> getPlayers(){
		List<Character> result = new List<Character>();
		foreach( GameObject p in players ){
			Character pCharacter = p.GetComponent<Character>();
			if( pCharacter != null ){
				result.Add( pCharacter );
			}
		}
		return result;
	}
	private List<Character> getEnemies(){
		List<Character> result = new List<Character>();
		foreach( GameObject e in enemies ){
			Character eCharacter = e.GetComponent<Character>();
			if( eCharacter != null ){
				result.Add( eCharacter );
			}
		}
		return result;
	}

	// To be executed before the component starts
	void Awake () {
		// Create the map based on the file
		if( this.mapFile != null ){
			this.mapModel = Map.read( this.mapFile.text );
			// Setting players and enemies // TODO it must be included in the map
			this.mapModel.Players = this.getPlayers();
			this.mapModel.Enemies = this.getEnemies();
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
