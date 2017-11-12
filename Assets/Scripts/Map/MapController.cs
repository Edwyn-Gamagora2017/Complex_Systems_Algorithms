using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	Map mapModel;			// stores the map information

	Dictionary< Character, List<KeyValuePair<List<PathVertexInfo>,Color>> > paths;	// Paths set by the characters
	bool refreshPaths;					// Indicates if the list of paths must be refreshed on view

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
		this.paths = new Dictionary<Character,List<KeyValuePair<List<PathVertexInfo>,Color>>>();
		this.refreshPaths = true;
	}

	public List<Character> getPlayers(){
		return this.mapModel.Players;
	}
	public List<Enemy> getEnemies(){
		return this.mapModel.Enemies;
	}
	public float getCharacterPositionCost( Character c ){
		return this.mapModel.getCharacterPositionCost( c );
	}

	public void FindPathAstar( Character enemy ){
		this.mapModel.findPathAstar( enemy );
	}
	public void FindPathDijkstra( Character enemy ){
		this.mapModel.findPathDijkstra( enemy );
	}
	public void includePaths( List<KeyValuePair<List<PathVertexInfo>,Color>> paths, Character c ){
		if( !this.paths.ContainsKey(c) ){
			this.paths.Add( c, paths );
		}
		else{
			this.paths[c] = paths;
		}
		this.refreshPaths = true;
	}
	private void setViewPaths(){
		if( this.GetComponent<MapView>() != null ){
			List<KeyValuePair<List<PathVertexInfo>,Color>> pathsList = new List<KeyValuePair<List<PathVertexInfo>,Color>>();
			foreach( List<KeyValuePair<List<PathVertexInfo>,Color>> ps in this.paths.Values ){
				foreach( KeyValuePair<List<PathVertexInfo>,Color> p in ps ){
					pathsList.Add(p);
				}
			}
			this.GetComponent<MapView>().setPathsToRender( pathsList );
		}
		this.refreshPaths = false;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if( this.refreshPaths ){
			this.setViewPaths();
		}
	}
}
