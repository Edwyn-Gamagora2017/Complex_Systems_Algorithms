using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour {

	protected Character model; // Stores the Character Model values

	public Character Model {
		set {
			model = value;
			GetComponent<CharacterView>().CharacterModel = value;
		}
	}

	// Use this for initialization
	protected virtual void Start () {

	}

	// Update is called once per frame
	protected virtual void Update () {

	}
}
