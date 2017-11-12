using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

	[SerializeField]
	EnemyView enemyViewPrefab;	// Prefab that defines the colors that represents the algorithms
	[SerializeField]
	UnityEngine.UI.Image aStarIcon;
	[SerializeField]
	UnityEngine.UI.Image dijkstraIcon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		aStarIcon.color = enemyViewPrefab.AStar;
		dijkstraIcon.color = enemyViewPrefab.Dijkstra;
	}
}
