using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Graph g = Graph.readFromFile( "Assets/Graphs/graph1.txt" );
		Debug.Log (g.print());

		Map m = Map.readFromFile( "Assets/Maps/map1.txt" );
		Debug.Log (m.toString());
		//Debug.Log (m.getGraph.print());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
