using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneticView : MonoBehaviour {

	MapGenetic mapModel;			// stores the map information

	Dictionary<KeyValuePair<int,int>, GameObject> tileGameObjects;	// Tile GameObjects

	// Elements to show the map
	[SerializeField]
	GameObject mapContainer;	// Element that involve the map
	[SerializeField]
	GameObject mapTilePrefab;	// Element that represents a mapTile

	[SerializeField]
	GameObject targetPrefab;	// Element that represents a target

	/* PROPERTIES */
	public MapGenetic MapModel {
		set {
			mapModel = value;
			this.drawMap();
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	private GameObject createMapTile( MapGenetic.MapTileType type, int x, int y ){
		GameObject result = null;
		if( type != MapGenetic.MapTileType.NotDefined ){
			result = GameObject.Instantiate( mapTilePrefab, mapContainer.transform );
			result.GetComponent<MapGeneticTileView>().setPosition( x, y );
			result.GetComponent<MapGeneticTileView>().setType( type );
		}
		return result;
	}

	private GameObject createTarget( float x, float y ){
		GameObject result = GameObject.Instantiate( targetPrefab, mapContainer.transform );
		result.transform.position = new Vector3(x,y,-0.1f);
		return result;
	}

	// Create map Tiles
	private void drawMap(){
		this.tileGameObjects = new Dictionary<KeyValuePair<int, int>, GameObject>();

		if( this.mapModel != null ){
			for( int y=0; y<this.mapModel.Height; y++ ){
				for( int x=0; x<this.mapModel.Width; x++ ){
					MapGenetic.MapTileType type = this.mapModel.getTileType( x,y );
					this.tileGameObjects.Add( new KeyValuePair<int, int>( y,x ), this.createMapTile( type, x, y ) );
				}
			}
			foreach( GeneticTarget t in this.mapModel.Targets ){
				this.createTarget( t.getPosX(), t.getPosY() );
			}
			// Adjust the camera
			Camera mapCamera = GameObject.FindObjectOfType<Camera>();
			if( mapCamera != null ){
				mapCamera.transform.position = new Vector3( this.mapModel.Width/2f-0.5f, this.mapModel.Height/2f-0.5f, -1 );	// 0.5 is the size of a half of the tile
				mapCamera.orthographicSize = Mathf.Max( this.mapModel.Width/2f, this.mapModel.Height/2f );
			}
		}
	}
}
