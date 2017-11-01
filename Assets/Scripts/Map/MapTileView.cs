﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileView : MonoBehaviour {

	Map.MapTileType type;	// Type of the tyle

	[SerializeField]
	Sprite[] tileSprites;		// Sprites that are going to be used to show the tiles of the map. The index of the tileSprite matchs the Enum TileType
	// TODO create a custom Unity inspector for the tile sprite

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setType( Map.MapTileType type ){
		this.type = type;
		GetComponent<SpriteRenderer>().sprite = tileSprites[ Map.typeToTypeIndex( type ) ];
	}
}
