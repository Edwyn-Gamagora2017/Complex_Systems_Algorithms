using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	Map mapModel;			// stores the map information

	[SerializeField]
	TextAsset mapFile;		// File that describes the map

	// To be executed before the component starts
	void Awake () {
		// Create the map based on the file
		if( this.mapFile != null ){
			this.mapModel = Map.read( this.mapFile.text );
			if( this.GetComponent<MapView>() != null ){
				this.GetComponent<MapView>().MapModel = this.mapModel;
			}
		}
		else{
			Debug.LogError( "MapView : mapFile not defined" );
		}
	}

	public List<Character> getPlayers(){
		return this.mapModel.Players;
	}
	public List<Character> getEnemies(){
		return this.mapModel.Enemies;
	}

	public void FindPath( Character enemy ){
		this.mapModel.findPath( enemy );
	}
	public void showPath( List<PathVertexInfo> path, Color color ){
		if( this.GetComponent<MapView>() != null ){
			this.GetComponent<MapView>().showPath( path, color );
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
