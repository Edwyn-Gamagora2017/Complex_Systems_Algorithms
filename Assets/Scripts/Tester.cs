using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Testing graph functions
		// Reading
		Graph g = Graph.readFromFile( "Assets/Graphs/graph2.txt" );
		// Showing
		Debug.Log (g.toString());
		// A*
		PathVertexInfo aResult = g.aStar( 0, 3 );
		// Distance obtained by the A*
		Debug.Log( "Distance to target: "+aResult.DistanceToVertex );
		// Showing the path found by A*
		List<PathVertexInfo> path = aResult.pathTo();
		string resPath = "";
		foreach( PathVertexInfo v in path ){
			resPath += v.VertexIndex + " -> ";
		}
		Debug.Log( resPath );

		Map m = Map.readFromFile( "Assets/Maps/map1.txt" );
		Debug.Log (m.toString());
		//Debug.Log (m.getGraph.print());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
