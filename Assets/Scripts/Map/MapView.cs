using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour {

	Map mapModel;			// stores the map information

	// Elements to show the map
	[SerializeField]
	GameObject mapContainer;	// Element that involve the map
	[SerializeField]
	GameObject mapTilePrefab;	// Element that represents a mapTile
	[SerializeField]
	Sprite[] tileSprites;		// Sprites that are going to be used to show the tiles of the map. The index of the tileSprite matchs the Enum TileType
	// TODO create a custom Unity inspector for the tile sprite

	/* PROPERTIES */
	public Map MapModel {
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

	private GameObject createMapTile( Map.MapTileType type, int x, int y ){
		GameObject result = null;
		if( type != Map.MapTileType.NotDefined ){
			result = GameObject.Instantiate( mapTilePrefab, mapContainer.transform );
			result.GetComponent<SpriteRenderer>().sprite = tileSprites[ Map.typeToTypeIndex( type ) ];
			result.transform.position = new Vector3(x,y);
		}
		return result;
	}

	// Create map Tiles
	private void drawMap(){
		for( int y=0; y<this.mapModel.Height; y++ ){
			for( int x=0; x<this.mapModel.Width; x++ ){
				Map.MapTileType type = this.mapModel.getTileType( x,y );
				this.createMapTile( type, x, y );
			}
		}
	}
}
