using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Graph g = Graph.readFromFile( "graph1.txt" );
		Debug.Log (g.print());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
