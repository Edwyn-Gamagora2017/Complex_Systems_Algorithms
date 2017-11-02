using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileView : MonoBehaviour {

	Map.MapTileType type;	// Type of the tyle

	[SerializeField]
	Sprite[] tileSprites;		// Sprites that are going to be used to show the tiles of the map. The index of the tileSprite matchs the Enum TileType
	// TODO create a custom Unity inspector for the tile sprite

	[SerializeField]
	UnityEngine.UI.Text tileInfo;	// UI element to display information related to the tile

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		tileInfo.text = "1";
	}

	public void setType( Map.MapTileType type ){
		this.type = type;
		GetComponent<SpriteRenderer>().sprite = tileSprites[ Map.typeToTypeIndex( this.type ) ];
	}
}
