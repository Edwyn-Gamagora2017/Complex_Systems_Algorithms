﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class that defines the basic Vertex information to the graph
public abstract class VertexInfo{
	protected int vertexIndex;		// Index of the vertex in the vertices list
	protected float vertexCost;		// Cost of visiting the vertex

	public VertexInfo( int vertexIndex, float vertexCost = 1 ){
		this.vertexIndex = vertexIndex;
		this.vertexCost = vertexCost;
	}
	public int VertexIndex {
		get {
			return vertexIndex;
		}
	}
	public float VertexCost {
		get {
			return vertexCost;
		}
	}

	public abstract string toString();
	// Heuristics used by the A* algorithm
	public abstract float distanceTo( VertexInfo vertex );
};
// Class that determines necessary Vertex information to graph algorithms to path finding, such as A* algorithm
public class PathVertexInfo{
	private VertexInfo vertex;			// Vertex associated to the information
	private PathVertexInfo previousVertex;	// the previous Vertex in the path
	private float distanceToVertex;		// Distance acumulated until arriving to the vertex
	private bool visited;				// Idicates if the vertex was processed by the algorithms

	public PathVertexInfo( VertexInfo vertex, float distanceToVertex = 0 ){
		this.vertex = vertex;
		this.previousVertex = null;
		this.distanceToVertex = distanceToVertex;
		this.visited = false;
	}

	public float DistanceToVertex {
		get {
			return distanceToVertex;
		}
		set {
			distanceToVertex = value;
		}
	}
	public bool Visited {
		get {
			return visited;
		}
		set {
			visited = value;
		}
	}
	public PathVertexInfo PreviousVertex {
		set {
			previousVertex = value;
		}
	}
	public int PreviousVertexIndex {
		get {
			return previousVertex.VertexIndex;
		}
	}
	public VertexInfo Vertex {
		get {
			return vertex;
		}
	}
	public int VertexIndex {
		get {
			return vertex.VertexIndex;
		}
	}
	public float VertexCost {
		get {
			return vertex.VertexCost;
		}
	}
	// Distance to the target predicted by the heuristics
	public float distanceToTarget( VertexInfo target ){
		return this.DistanceToVertex + this.vertex.distanceTo( target );
	}
	// Obtains the path to achieve the vertex
	public List<PathVertexInfo> pathTo(){
		List<PathVertexInfo> result = new List<PathVertexInfo>();
		PathVertexInfo current = this;

		while( current != null ){
			result.Add( current );
			current = current.previousVertex;
		}

		return result;
	}
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
	// A* algorithm : Obtains the smallest distance in the list (remove it from the list). It considers the target
	private PathVertexInfo getSmallest(List<PathVertexInfo> list, VertexInfo target){
		if( list.Count > 0 ){
			int resultIndex = -1;
			for(int i=0; i<list.Count; i++){
				if( resultIndex == -1 || list[resultIndex].distanceToTarget( target ) > list[i].distanceToTarget( target ) ){
					resultIndex = i;
				}
			}
			if( resultIndex > -1 ){
				PathVertexInfo result = list[resultIndex];
				list.RemoveAt( resultIndex );
				return result;
			}
		}
		return null;
	}
	// Check if the vertex was inserted in a list of vertexDistance
	private bool vertexAlreadyInList(int vertexIndex, List<PathVertexInfo> list){
		foreach( PathVertexInfo v in list ){
			if( v.VertexIndex == vertexIndex ){
				return true;
			}
		}
		return false;
	}
	// Calculates the smallest path between Origin and Target
	public PathVertexInfo aStar( int vertexOriginIndex, int vertexTargetIndex ){
		Debug.Log(this.toString());
		// Check if the arguments are valid vertices
		if( this.isValidVertexIndex( vertexOriginIndex ) || this.isValidVertexIndex( vertexTargetIndex ) ){
			// Create list of information necessary to the algorithm
			Dictionary<int,PathVertexInfo> info = new Dictionary<int, PathVertexInfo>();
			foreach( VertexInfo vertex in this.vertices ){
				info.Add( vertex.VertexIndex, new PathVertexInfo( vertex ) );
			}
			// Initialize the list of neighbors
			List<PathVertexInfo> open = new List<PathVertexInfo>();

			// First Element
			info[ vertexOriginIndex ].DistanceToVertex = this.vertices[ vertexOriginIndex ].VertexCost;
			open.Add( info[ vertexOriginIndex ] );

			PathVertexInfo currentVertex = this.getSmallest( open, this.vertices[ vertexTargetIndex ] );

			// Calculate the neighbors
			while( currentVertex != null && currentVertex.VertexIndex != vertexTargetIndex ){
				
				// Informing that this vertex was processed
				currentVertex.Visited = true;

				// Inserting Neighbors
				foreach( Adjacent neighbor in this.adjacency[ currentVertex.VertexIndex ] ){
					if( !info[ neighbor.index ].Visited && !vertexAlreadyInList( neighbor.index, open ) ){
						PathVertexInfo neighborVertex = info[ neighbor.index ];
						neighborVertex.DistanceToVertex = currentVertex.DistanceToVertex + neighbor.edgeWeight + neighborVertex.VertexCost;
						neighborVertex.PreviousVertex = currentVertex;
						// Add neighbor
						open.Add( neighborVertex );
					}
				}

				// Updating current
				currentVertex = this.getSmallest( open, this.vertices[ vertexTargetIndex ] );
			}

			// Result
			if( currentVertex != null && currentVertex.VertexIndex == vertexTargetIndex ){
				return info[ vertexTargetIndex ];
			}
		}
		return null;
	}

	/*
	 * ====  INPUT and OUTPUT ====
	 */
	public string toString()
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
	public class FloatVertexInfo : VertexInfo {
		public FloatVertexInfo( float vertexCost, int index ) : base( index, vertexCost ){
		}
		public override float distanceTo (VertexInfo vertex)
		{
			return ((FloatVertexInfo)vertex).VertexIndex - this.VertexIndex;
		}
		public override string toString ()
		{
			return "Vertex => index: "+this.VertexIndex.ToString()+"; cost: "+this.VertexCost.ToString();
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
