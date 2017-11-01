using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

	private Character model; // Stores the Character Model values

	public Character Model {
		set {
			model = value;
			GetComponent<CharacterView>().CharacterModel = value;
		}
	}

	// Use this for initialization
	protected void Start () {

	}

	// Update is called once per frame
	protected void Update () {

	}
}
