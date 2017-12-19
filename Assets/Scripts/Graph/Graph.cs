using System.Collections;
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
	private PathVertexInfo previousVertex;		// the previous Vertex in the path
	//private float costFromPrevious;				// the cost of the edge from the previous Vertex in the path
	private float distanceToVertex;		// Distance acumulated until arriving to the vertex
	private bool visited;				// Indicates if the vertex was processed by the algorithms

	public PathVertexInfo( VertexInfo vertex, float distanceToVertex = 0 ){
		this.vertex = vertex;
		this.previousVertex = null;
		//this.costFromPrevious = 0;
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
		get {
			return previousVertex;
		}
		set {
			previousVertex = value;
		}
	}
	public int PreviousVertexIndex {
		get {
			return previousVertex.VertexIndex;
		}
	}
	/*public float CostFromPrevious {
		get {
			return costFromPrevious;
		}
		set {
			costFromPrevious = value;
		}
	}*/
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
	// Obtains the string that represents the path
	public static string pathToString( List<PathVertexInfo> path ){
		string resPath = "";

		foreach( PathVertexInfo v in path ){
			resPath += v.VertexIndex + " -> ";
		}

		return resPath;
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
	PathVertexInfo[,] allDistances;	// all distances - calculated by the algorithm of Floyd Warshall

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

	/*
	 * GETTERS and SETTERS
	 */
	public int getAmountVertices(){
		return this.vertices.Length;
	}
	public float getVertexCost( int index ){
		return this.vertices[index].VertexCost;
	}
	public VertexInfo getVertex( int index ){
		return this.vertices[index];
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
	// Check if the vertex was inserted in a list of vertexDistance
	private bool vertexAlreadyInList(int vertexIndex, List<PathVertexInfo> list){
		foreach( PathVertexInfo v in list ){
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
	// Calculates the smallest path between Origin and Target
	public PathVertexInfo aStar( int vertexOriginIndex, int vertexTargetIndex ){
		// Debug.Log(this.toString());
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
						//neighborVertex.CostFromPrevious = neighbor.edgeWeight;
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

	// Dijkstra : Obtains the smallest distance in the list (remove it from the list)
	private PathVertexInfo getSmallest(List<PathVertexInfo> list ){
		if( list.Count > 0 ){
			int resultIndex = -1;
			for(int i=0; i<list.Count; i++){
				if( resultIndex == -1 || list[resultIndex].DistanceToVertex > list[i].DistanceToVertex ){
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
	// Calculates the smallest path between Origin and Target
	public PathVertexInfo dijkstra( int vertexOriginIndex, int vertexTargetIndex ){
		//Debug.Log(this.toString());
		// Check if the arguments are valid vertices
		if( this.isValidVertexIndex( vertexOriginIndex ) || this.isValidVertexIndex( vertexTargetIndex ) ){
			// Create list of information necessary to the algorithm
			Dictionary<int,PathVertexInfo> info = new Dictionary<int, PathVertexInfo>();
			foreach( VertexInfo vertex in this.vertices ){
				info.Add( vertex.VertexIndex, new PathVertexInfo( vertex, float.MaxValue ) );
			}
			// Initialize the list of neighbors
			List<PathVertexInfo> open = new List<PathVertexInfo>();

			// First Element
			info[ vertexOriginIndex ].DistanceToVertex = this.vertices[ vertexOriginIndex ].VertexCost;
			open.Add( info[ vertexOriginIndex ] );

			PathVertexInfo currentVertex = this.getSmallest( open );

			// Calculate the neighbors
			while( currentVertex != null && currentVertex.VertexIndex != vertexTargetIndex ){

				// Informing that this vertex was processed
				currentVertex.Visited = true;

				// Inserting Neighbors
				foreach( Adjacent neighbor in this.adjacency[ currentVertex.VertexIndex ] ){
					if( !info[ neighbor.index ].Visited ){
						PathVertexInfo neighborVertex = info[ neighbor.index ];
						float newDistance = currentVertex.DistanceToVertex + neighbor.edgeWeight + neighborVertex.VertexCost;
						if( neighborVertex.DistanceToVertex > newDistance ){
							neighborVertex.DistanceToVertex = newDistance;
							neighborVertex.PreviousVertex = currentVertex;
							//neighborVertex.CostFromPrevious = neighbor.edgeWeight;
						}
						// Add neighbor
						if( !vertexAlreadyInList( neighbor.index, open ) ){
							open.Add( neighborVertex );
						}
					}
				}

				// Updating current
				currentVertex = this.getSmallest( open );
			}

			// Result
			if( currentVertex != null && currentVertex.VertexIndex == vertexTargetIndex ){
				return info[ vertexTargetIndex ];
			}
		}
		return null;
	}

	// Calculates the smallest path among all vertices
	public void floydWarshall(){

		this.allDistances = new PathVertexInfo[ this.vertices.Length, this.vertices.Length ];

		// initializing distances
		for (int i = 0; i < this.vertices.Length; i++) {
			this.allDistances [i, i] = new PathVertexInfo (this.vertices [i], 0);

			for (int j = 0; j < this.vertices.Length; j++) {
				if (i != j) {
					Adjacent adj = this.isAdjacent (i, j);
					if (adj != null) {
						this.allDistances [i, j] = new PathVertexInfo (this.vertices [j],this.vertices [j].VertexCost + adj.edgeWeight + this.allDistances[i,i].DistanceToVertex);
						this.allDistances [i, j].PreviousVertex = this.allDistances[i,i];
					} else {
						this.allDistances [i, j] = new PathVertexInfo (this.vertices [j], float.MaxValue);
					}
				}
			}
		}

		for (int k = 0; k < this.vertices.Length; k++) {
			for (int i = 0; i < this.vertices.Length; i++) {
				for (int j = 0; j < this.vertices.Length; j++) {
					if (allDistances [i, j].DistanceToVertex > allDistances [i, k].DistanceToVertex + allDistances [k, j].DistanceToVertex
						&& allDistances [i, k].DistanceToVertex + allDistances [k, j].DistanceToVertex >= 0) {	// Check overflow
						allDistances [i, j].DistanceToVertex = allDistances [i, k].DistanceToVertex + allDistances [k, j].DistanceToVertex;
						allDistances [i, j].PreviousVertex = allDistances [k, j].PreviousVertex;
					}
				}
			}
		}

		// add the cost of start vertex to the path
		for (int i = 0; i < this.vertices.Length; i++) {
			for (int j = 0; j < this.vertices.Length; j++) {
				float distanceWithStart = allDistances [i, j].DistanceToVertex + this.vertices [i].VertexCost;
				// not overflow
				if (distanceWithStart > 0) {
					allDistances [i, j].DistanceToVertex = distanceWithStart;
				}
			}
		}
	}
	// Obtains the path to achieve the vertex (based on FloydWarshall)
	public List<PathVertexInfo> pathToFloydWarshall( int indexStart, int indexEnd ){
		List<PathVertexInfo> result = new List<PathVertexInfo>();
		if( this.allDistances != null ){
			PathVertexInfo current = this.allDistances [indexStart, indexEnd];

			while( current != null ){
				result.Add( current );
				if (current.PreviousVertex != null) {
					current = allDistances [indexStart, current.PreviousVertex.Vertex.VertexIndex];
				} else {
					current = null;
				}
			}
		}

		return result;
	}
	public float getFloydWarshallDistance( int indexStart, int indexEnd ){
		if (this.allDistances != null) {
			return this.allDistances [indexStart, indexEnd].DistanceToVertex;
		}
		return float.MaxValue;
	}

	/*
	 * ==== INPUT and OUTPUT ====
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
