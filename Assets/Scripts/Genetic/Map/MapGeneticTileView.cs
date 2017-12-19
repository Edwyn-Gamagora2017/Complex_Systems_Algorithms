using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneticTileView : MonoBehaviour {

	MapGenetic.MapTileType type;	// Type of the tyle

	[SerializeField]
	Sprite[] tileSprites;		// Sprites that are going to be used to show the tiles of the map. The index of the tileSprite matchs the Enum TileType
	// TODO create a custom Unity inspector for the tile sprite
	[SerializeField]
	UnityEngine.UI.Image tileImage;	// Image used to show the tile sprite
	[SerializeField]
	UnityEngine.UI.Image isPathImage;	// Image used to show that the tile is part of the path

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}

	public void setType( MapGenetic.MapTileType type ){
		this.type = type;
		if( this.type != MapGenetic.MapTileType.NotDefined ){
			tileImage.sprite = tileSprites[ MapGenetic.typeToTypeIndex( this.type ) ];
		}
	}
	public void setPosition( float x, float y ){
		transform.position = new Vector3(x,y,0);
	}
	public void hidePath(){
		if( isPathImage != null ){
			isPathImage.gameObject.SetActive( false );
		}
	}
	public void showPath( Color color ){
		if( isPathImage != null ){
			isPathImage.gameObject.SetActive( true );
			isPathImage.color = color;
		}
	}
}
