using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Graph g = Graph.readFromFile( "Assets/Graphs/graph2.txt" );
		Debug.Log (g.toString());
		PathVertexInfo aResult = g.aStar( 0, 3 );
		Debug.Log( "Distance to target: "+aResult.DistanceToVertex );

		Map m = Map.readFromFile( "Assets/Maps/map1.txt" );
		Debug.Log (m.toString());
		//Debug.Log (m.getGraph.print());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
