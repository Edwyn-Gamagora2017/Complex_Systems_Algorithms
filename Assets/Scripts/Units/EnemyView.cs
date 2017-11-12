using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : CharacterView {

	/*
	 * Attributes
	 */
	[SerializeField]
	Color aStar;		// Color that represents the Astar algorithm
	[SerializeField]
	Color dijkstra;		// Color that represents the Dijkstra algorithm

	/*
	 * GETTERS
	 */
	public Color AStar {
		get {
			return aStar;
		}
	}
	public Color Dijkstra {
		get {
			return dijkstra;
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		// update color
		this.gameObject.GetComponent<SpriteRenderer>().color = ((Enemy)this.characterModel).AStar ? aStar : dijkstra;
	}
}
