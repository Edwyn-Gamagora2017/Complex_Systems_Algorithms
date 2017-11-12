using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {

	private GameObject map;		// Map Controller and View
	List<GameObject> players;
	List<GameObject> enemies;

	[SerializeField]
	GameObject mapPrefab;		// Map to be instantiated when a new map is selected

	[SerializeField]
	GameObject playerPrefab;	// Prefab to instantiate the Player
	[SerializeField]
	GameObject enemyPrefab;		// Prefab to instantiate the Enemy

	// Executing before Start
	void Awake(){
		this.players = new List<GameObject>();
		this.enemies = new List<GameObject>();
	}

	// Use this for initialization
	void Start () {}

	void loadCharacters(){
		destroyCharacters();	// Cleaning

		if( this.map != null ){
			// Create the GameObjects for the Characters
			foreach( Character c in this.map.GetComponent<MapController>().getPlayers() ){
				GameObject player = GameObject.Instantiate( playerPrefab, this.transform );
				player.GetComponent<PlayerController>().Model = c;
				this.players.Add( player );
			}
			foreach( Character c in this.map.GetComponent<MapController>().getEnemies() ){
				GameObject enemy = GameObject.Instantiate( enemyPrefab, this.transform );
				enemy.GetComponent<EnemyController>().Model = c;
				enemy.GetComponent<EnemyController>().Map = this.map.GetComponent<MapController>();
				this.enemies.Add( enemy );
			}
		}
	}

	void destroyCharacters(){
		foreach( GameObject g in this.enemies ){
			Destroy( g );
		}
		foreach( GameObject g in this.players ){
			Destroy( g );
		}

		this.players = new List<GameObject>();
		this.enemies = new List<GameObject>();
	}

	public bool setMapPath( string path ){
		if( this.map != null ){ Destroy( this.map ); }

		this.map = Instantiate( this.mapPrefab, this.transform );
		bool result = this.map.GetComponent<MapController>().setMap( path );

		this.loadCharacters();

		return result;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
