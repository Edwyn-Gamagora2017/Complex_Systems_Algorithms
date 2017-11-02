using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class that determines necessary Vertex functions to graph A* algorithm
public abstract class VertexInfo{
	private int vertexIndex;		// Index of the vertex in the vertices list
	public VertexInfo( int vertexIndex ){
		this.vertexIndex = vertexIndex;
	}
	public int VertexIndex {
		get {
			return vertexIndex;
		}
	}
	// Heuristics used by the A* algorithm
	public abstract float distanceTo( VertexInfo vertex );
	public abstract string toString();
};

public class Graph {
	
	// Defines a element of the list of adjacency
	public class Adjacent{
		public int index;			// Index of the adjacent vertex in the vertices list
		public float edgeWeight;	// Weight of the edge that stablishes the adjacency

		public Adjacent(int index, float edgeWeight)
		{
			this.index = index;
			this.edgeWeight = edgeWeight;
		}
	};

	bool bidirectional;			// Indicates if the edge is bidirectional
	bool multipleEdge;			// Indicates if the graph accepts multiple edges connecting the same pair of vertices
	VertexInfo[] vertices;		// List of information about the vertices
	List<Adjacent>[] adjacency;	// List of adjacency

	public Graph( int nVertices, bool bidirectional, bool multipleEdge )
	{
		this.bidirectional = bidirectional;
		this.multipleEdge = multipleEdge;
		// Setting Vertices
		this.vertices = new VertexInfo[nVertices];
		// Setting Vertices Adjacency
		this.adjacency = new List<Adjacent>[nVertices];
		for (int i = 0; i < nVertices; i++) {
			this.adjacency[i] = new List<Adjacent>();
		}
	}

	// Indicates if a index is inside the range of existing vertex indexes
	private bool isValidVertexIndex( int index ){
		return index >= 0 && index < this.vertices.Length;
	}
	// returns the information about the edge connecting A to B (null is they are not connected)
	public Adjacent isAdjacent(int indexA, int indexB)
	{
		if( this.isValidVertexIndex( indexA ) ){
			foreach( Adjacent adj in this.adjacency[indexA] )
			{
				if (adj.index == indexB) {
					return adj;
				}
			}
		}
		return null;
	}
	// Inserts the information related to a vertex
	public void setVertex( int index, VertexInfo value )
	{
		if( this.isValidVertexIndex( index ) ){
			this.vertices [index] = value;
		}
	}
	// Inserts information related to the edge that connects A to B
	public void setAdjacency( int indexA, int indexB, float edgeWeight )
	{
		if ( (this.isAdjacent (indexA, indexB) == null || this.multipleEdge) && this.isValidVertexIndex(indexA) && this.isValidVertexIndex(indexB) ) {
			this.adjacency [indexA].Add (new Adjacent( indexB, edgeWeight ));
			if (this.bidirectional) {
				this.adjacency [indexB].Add (new Adjacent( indexA, edgeWeight ));
			}
		}
	}

	/*
	 * PATH FINDING
	*/

	// Check if a vertex is present in the list of targets
	private bool isTarget( int vertexIndex, List<VertexInfo> targets ){
		foreach( VertexInfo v in targets ){
			if( v.VertexIndex == vertexIndex ){
				return true;
			}
		}
		return false;
	}
	// Class used in the A* algorithm in order to store the distance cumulated while calculating the distance between a pair of vertices A and B, passing through the vertex
	private class VertexDistance{
		public int vertexIndex;		// vertex that possibly connects A and B
		public float distance;		// cumulated distance
		public VertexDistance( int vertexIndex, float distance ){
			this.vertexIndex = vertexIndex;
			this.distance = distance;
		}
	}
	// A* algorithm : Obtains the smallest distance in the list (remove it from the list)
	private VertexDistance getSmallest(List<VertexDistance> list){
		if( list.Count > 0 ){
			int resultIndex = -1;
			for(int i=0; i<list.Count; i++){
				if( resultIndex == -1 || list[resultIndex].distance < list[i].distance ){
					resultIndex = i;
				}
			}
			if( resultIndex > -1 ){
				VertexDistance result = list[resultIndex];
				list.RemoveAt( resultIndex );
				return result;
			}
		}
		return null;
	}
	// Check if the vertex was inserted in a list of vertexDistance
	private bool vertexAlreadyInList(int vertexIndex, List<VertexDistance> list){
		foreach( VertexDistance v in list ){
			if( v.vertexIndex == vertexIndex ){
				return true;
			}
		}
		return false;
	}
	// Calculates the smallest path between Origin and Target
	public VertexInfo aStar( int vertexOriginIndex, int vertexTargetIndex ){
		// Check if the arguments are valid vertices
		if( this.isValidVertexIndex( vertexOriginIndex ) || this.isValidVertexIndex( vertexTargetIndex ) ){
			// Create visit list
			List<bool> visited = new List<bool>();
			foreach( VertexInfo vertex in this.vertices ){
				visited.Add( false );
			}
			// Initialize the list of neighbors
			List<VertexDistance> open = new List<VertexDistance>();

			// First Element
			open.Add( new VertexDistance( vertexOriginIndex, 0 ) );

			// Calculate the neighbors
			VertexDistance currentVertex = this.getSmallest( open );
			visited[currentVertex.vertexIndex] = true;
			while( currentVertex.vertexIndex != vertexTargetIndex || currentVertex != null ){
				// Inserting Neighbors
				foreach( Adjacent neighbor in this.adjacency[ currentVertex.vertexIndex ] ){
					if( !visited[ neighbor.index ] && !vertexAlreadyInList( neighbor.index, open ) ){
						// Add neighbor
						float dist = currentVertex.distance + this.vertices[neighbor.index].distanceTo( this.vertices[vertexTargetIndex] );
						open.Add( new VertexDistance( neighbor.index, dist ) );
						// Updating vertex distance (if vertex has type TileInfo)
						if(this.vertices[neighbor.index].GetType() == typeof(Map.TileInfo)){
							((Map.TileInfo)this.vertices[neighbor.index]).distance = dist;
						}
					}
				}

				// Updating current
				currentVertex = this.getSmallest( open );
				// Visiting Current
				visited[currentVertex.vertexIndex] = true;
			}

			// Result
			if( currentVertex.vertexIndex == vertexTargetIndex ){
				return this.vertices[ vertexTargetIndex ];
			}
		}
		return null;
	}

	/*
	 * ====  INPUT and OUTPUT ====
	 */
	public string print()
	{
		string res = "Graph : {\nnVertices = "+this.vertices.Length.ToString ()+"\n";
		for( int i=0; i<this.adjacency.Length; i++ )
		{
			res += i.ToString()+"("+this.vertices[i].toString()+") => ";
			foreach( Adjacent adj in this.adjacency[i] )
			{
				res += adj.index.ToString()+"("+adj.edgeWeight.ToString()+") ";
			}
			res += "\n";
		}
		res += "}\n";

		return res;
	}

	// Class to be used to store a vertex in the graph (in the case of a graph containing floats as vertices weight)
	public class FloatVertexInfo:VertexInfo{
		public float vertexWeight;		// vertex weight
		public FloatVertexInfo( float vertexWeight, int index ) : base( index ){
			this.vertexWeight = vertexWeight;
		}
		public override float distanceTo (VertexInfo vertex)
		{
			return ((FloatVertexInfo)vertex).vertexWeight - this.vertexWeight;
		}
		public override string toString ()
		{
			return this.vertexWeight.ToString();
		}
	};
	// Reading file containing a graph presenting floats as vertices weight
	public static Graph readFromFile( string fileName )
	{
		/*
		 * File format :
		 * nVertices nEdges 0|1(bidirectional) 0|1(multipleEdge)
		 * nVertices * value
		 * nEdges * ( v1, v2, weight)
		*/
		string path = fileName;

		// Starting Reader
		System.IO.StreamReader reader;
		try{
			reader = new System.IO.StreamReader( path );
		}
		catch( System.Exception ex ){
			Debug.LogError ( "Error while opening the Graph file" );
			Debug.LogError( ex.Message );
			return null;
		}

		// Reading Information
		try{
			// initial info
			string[] line = reader.ReadLine().Split(' ');
			int nV = int.Parse(line [0]);
			int nE = int.Parse(line [1]);
			bool bidirectional = int.Parse(line [2]) == 1;
			bool multipleEdge = int.Parse(line [3]) == 1;
			Graph g = new Graph (nV, bidirectional, multipleEdge);

			// Vertices
			for (int i = 0; i < nV; i++) {
				g.setVertex( i, new FloatVertexInfo( int.Parse( reader.ReadLine () ), i ) );
			}
			// Edges
			for (int i = 0; i < nE; i++) {
				string[] lineEdge = reader.ReadLine().Split(' ');
				g.setAdjacency( int.Parse(lineEdge [0]), int.Parse(lineEdge [1]), float.Parse(lineEdge [2]) );
			}

			reader.Close ();

			return g;
		}catch( System.Exception ex ){
			Debug.LogError ( "Error while opening the Graph file : wrong format" );
			Debug.LogError( ex.Message );
			return null;
		}
	}
}
