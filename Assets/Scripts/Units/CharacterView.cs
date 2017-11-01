using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour {

	Character characterModel;	// Character Model

	[SerializeField]
	Sprite spriteCharacter;			// Sprite that presents the character

	public Character CharacterModel {
		set {
			characterModel = value;
		}
	}

	// Use this for initialization
	protected void Start () {
		GetComponent<SpriteRenderer>().sprite = spriteCharacter;
	}
	
	// Update is called once per frame
	protected void Update () {
		// updating position
		Vector2 pos = characterModel.positionInTheMap();
		transform.position = new Vector3( pos.x, pos.y, 0 );
	}
}
