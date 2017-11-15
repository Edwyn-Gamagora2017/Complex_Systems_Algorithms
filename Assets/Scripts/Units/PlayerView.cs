using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : CharacterView {

	/*
	 * Attributes
	 */
	[SerializeField]
	Color freeMoveColor;		// Color that represents that the player is free to move

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		// Updating color
		this.gameObject.GetComponent<SpriteRenderer>().color = ((Player)this.characterModel).FreeMove ? freeMoveColor : Color.white;
	}
}
