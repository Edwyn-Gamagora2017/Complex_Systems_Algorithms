using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	Map mapModel;			// stores the map information

	Dictionary< Character, List<PathVertexInfo> > paths;	// Paths set by the characters
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
		this.paths = new Dictionary<Character,List<PathVertexInfo>>();
		this.refreshPaths = true;
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
	public void includePath( List<PathVertexInfo> path, Character c ){
		if( !this.paths.ContainsKey(c) ){
			this.paths.Add( c, path );
		}
		else{
			this.paths[c] = path;
		}
		this.refreshPaths = true;
	}
	private void setViewPaths(){
		if( this.GetComponent<MapView>() != null ){
			List<List<PathVertexInfo>> pathsList = new List<List<PathVertexInfo>>();
			foreach( List<PathVertexInfo> p in this.paths.Values ){
				pathsList.Add( p );
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
