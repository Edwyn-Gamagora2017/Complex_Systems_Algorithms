﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour {

	Map mapModel;			// stores the map information

	Dictionary<KeyValuePair<int,int>, GameObject> tileGameObjects;	// Tile GameObjects

	List<KeyValuePair<List<PathVertexInfo>,Color>> pathsToRender;	// List of paths to be shown

	bool refreshPaths = true;	// Flag that indicates that a refresh of path drawing is necessary

	// Elements to show the map
	[SerializeField]
	GameObject mapContainer;	// Element that involve the map
	[SerializeField]
	GameObject mapTilePrefab;	// Element that represents a mapTile				

	/* PROPERTIES */
	public Map MapModel {
		set {
			mapModel = value;
			this.drawMap();
		}
	}

	// Use this for initialization
	void Start () {
		this.pathsToRender = new List<KeyValuePair<List<PathVertexInfo>,Color>>();
	}
	
	// Update is called once per frame
	void Update () {
		if( this.refreshPaths ){
			this.renderPaths();
		}
	}

	private GameObject createMapTile( Map.MapTileType type, int x, int y ){
		GameObject result = null;
		if( type != Map.MapTileType.NotDefined ){
			result = GameObject.Instantiate( mapTilePrefab, mapContainer.transform );
			result.GetComponent<MapTileView>().setPosition( x, y );
			result.GetComponent<MapTileView>().setType( type );
		}
		return result;
	}

	// Create map Tiles
	private void drawMap(){
		this.tileGameObjects = new Dictionary<KeyValuePair<int, int>, GameObject>();

		if( this.mapModel != null ){
			for( int y=0; y<this.mapModel.Height; y++ ){
				for( int x=0; x<this.mapModel.Width; x++ ){
					Map.MapTileType type = this.mapModel.getTileType( x,y );
					this.tileGameObjects.Add( new KeyValuePair<int, int>( y,x ), this.createMapTile( type, x, y ) );
				}
			}
			// Adjust the camera
			Camera mapCamera = GameObject.FindObjectOfType<Camera>();
			if( mapCamera != null ){
				mapCamera.transform.position = new Vector3( this.mapModel.Width/2f-0.5f, this.mapModel.Height/2f-0.5f, -1 );	// 0.5 is the size of a half of the tile
				mapCamera.orthographicSize = Mathf.Max( this.mapModel.Width/2f, this.mapModel.Height/2f );
			}
		}
	}

	/*
	 * DRAW PATHS
	 */
	public void setPathsToRender( List<KeyValuePair<List<PathVertexInfo>,Color>> paths ){
		this.pathsToRender = paths;
		this.refreshPaths = true;
	}
	private void renderPaths(){
		this.clearPaths();
		foreach( KeyValuePair<List<PathVertexInfo>,Color> path in this.pathsToRender ){
			this.showPath( path.Key, path.Value );
		}
		this.refreshPaths = false;
	}
	private void showPath( List<PathVertexInfo> path, Color color ){
		string resPath = "";

		for(int i=0; i < path.Count-1; i++ ){
			Map.TileInfo vertexInfo = (Map.TileInfo)path[i].Vertex;
			resPath += vertexInfo.VertexIndex+": (x:"+vertexInfo.x+","+vertexInfo.y+")\n";
			GameObject tile = this.tileGameObjects[ new KeyValuePair<int,int>( vertexInfo.y, vertexInfo.x ) ];
			if( tile != null ){
				tile.GetComponent<MapTileView>().showPath( color );
			}
		}

		//Debug.Log( resPath );
	}
	public void clearPaths(){
		if( this.mapModel != null ){
			for( int y=0; y<this.mapModel.Height; y++ ){
				for( int x=0; x<this.mapModel.Width; x++ ){
					GameObject tile = this.tileGameObjects[ new KeyValuePair<int,int>( y, x ) ];
					if( tile != null ){
						tile.GetComponent<MapTileView>().hidePath();
					}
				}
			}
		}
	}
}
