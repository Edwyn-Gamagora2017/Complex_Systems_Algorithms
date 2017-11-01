using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	GameObject map;				// Map Controller and View

	[SerializeField]
	GameObject playerPrefab;	// Prefab to instantiate the Player
	[SerializeField]
	GameObject enemyPrefab;		// Prefab to instantiate the Enemy

	List<GameObject> players;
	List<GameObject> enemies;

	// Use this for initialization
	void Start () {
		this.players = new List<GameObject>();
		this.enemies = new List<GameObject>();

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
				this.enemies.Add( enemy );
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
