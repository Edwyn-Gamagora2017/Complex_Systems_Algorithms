using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	Map mapModel;			// stores the map information

	Dictionary< Character, List<KeyValuePair<List<PathVertexInfo>,Color>> > paths;	// Paths set by the characters
	bool refreshPaths;					// Indicates if the list of paths must be refreshed on view

	// To be executed before the component starts
	void Awake () {
		this.paths = new Dictionary<Character,List<KeyValuePair<List<PathVertexInfo>,Color>>>();
		this.refreshPaths = true;
	}

	public bool setMap( string path ){
		// Create the map based on the file
		this.mapModel = Map.readFromFile( path );
		if( this.GetComponent<MapView>() != null ){
			this.GetComponent<MapView>().MapModel = this.mapModel;
		}

		return this.mapModel != null;
	}

	/*
	 * GETTERS
	 */
	public List<Player> getPlayers(){
		if( this.mapModel != null ){
			return this.mapModel.Players;
		}else{
			return new List<Player>();
		}
	}
	public List<Enemy> getEnemies(){
		if( this.mapModel != null ){
			return this.mapModel.Enemies;
		}else{
			return new List<Enemy>();
		}
	}
	public float getCharacterPositionCost( Character c ){
		if( this.mapModel != null ){
			return this.mapModel.getCharacterPositionCost( c );
		}else{
			return float.MaxValue;
		}
	}

	/*
	 * PATH FINDING
	 */
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
