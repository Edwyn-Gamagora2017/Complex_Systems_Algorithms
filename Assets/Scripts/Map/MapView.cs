using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour {

	Map mapModel;			// stores the map information

	// Elements to show the map
	[SerializeField]
	GameObject mapContainer;	// Element that involve the map
	[SerializeField]
	Sprite[] tileSprites;		// Sprites that are going to be used to show the tiles of the map. The index of the tileSprite matchs the Enum TileType
	// TODO create a custom Unity inspector

	/* PROPERTIES */
	public Map MapModel {
		set {
			mapModel = value;
			this.updateMap();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Create map Tiles
	private void updateMap(){
		
	}
}
