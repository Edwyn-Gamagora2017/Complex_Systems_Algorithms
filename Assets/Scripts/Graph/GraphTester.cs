using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Testing graph functions
		// Reading
		Graph g = Graph.readFromFile( "Assets/Graphs/graph-salesman.txt" );
		// Showing
		Debug.Log (g.toString());

		/*
		// Dijkstra
		PathVertexInfo dijResult = g.dijkstra( 0, 3 );
		// Distance obtained
		Debug.Log( "Distance to target: "+dijResult.DistanceToVertex );
		// Showing the path found
		List<PathVertexInfo> path = dijResult.pathTo();
		*/

		// Floyd Warshall
		g.floydWarshall();
		float floResult = g.getFloydWarshallDistance(2,3);
		List<PathVertexInfo> floPathResult = g.pathToFloydWarshall(2,3);
		// Distance obtained
		Debug.Log( "Distance to target: "+floResult );
		// Showing the path found
		Debug.Log( PathVertexInfo.pathToString( floPathResult ) );
	}

	// Update is called once per frame
	void Update () {

	}
}
