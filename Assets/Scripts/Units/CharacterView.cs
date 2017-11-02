using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour {

	protected Character characterModel;	// Character Model

	[SerializeField]
	Sprite spriteCharacter;			// Sprite that presents the character

	public Character CharacterModel {
		set {
			characterModel = value;
		}
	}

	// Use this for initialization
	protected virtual void Start () {
		GetComponent<SpriteRenderer>().sprite = spriteCharacter;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		// updating position
		transform.position = new Vector3( this.characterModel.getPosX(), this.characterModel.getPosY(), -0.1f );
	}
}
