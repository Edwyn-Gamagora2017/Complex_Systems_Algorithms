using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
	public class Adjacent{
		public int index = -1;
		public float weight = 0;

		public Adjacent(int index, float weight)
		{
			this.index = index;
			this.weight = weight;
		}
	};

	bool bidirectional;
	float[] vertices;
	List<Adjacent>[] adjacency;	// List of adjacency

	public Graph( int nVertices, bool bidirectional )
	{
		this.bidirectional = bidirectional;
		// Setting Vertices
		this.vertices = new float[nVertices];
		for(int i=0; i<nVertices; i++) {
			this.vertices[i] = 0;
		}
		// Setting Vertices Adjacency
		this.adjacency = new List<Adjacent>[nVertices];
		for (int i = 0; i < nVertices; i++) {
			this.adjacency[i] = new List<Adjacent>();
		}
	}

	public Adjacent isAdjacent(int indexA, int indexB)
	{
		foreach( Adjacent adj in this.adjacency[indexA] )
		{
			if (adj.index == indexB) {
				return adj;
			}
		}
		return null;
	}
	public void setVertex( int index, float value )
	{
		this.vertices [index] = value;
	}
	public void setAdjacency( int indexA, int indexB, float value )
	{
		if ( this.isAdjacent (indexA, indexB) == null ) {
			this.adjacency [indexA].Add (new Adjacent( indexB, value ));
			if (this.bidirectional) {
				this.adjacency [indexB].Add (new Adjacent( indexA, value ));
			}
		}
	}

	public string print()
	{
		string res = "Graph : {\nnVertices = "+this.vertices.Length.ToString ()+"\n";
		for( int i=0; i<this.adjacency.Length; i++ )
		{
			res += i.ToString()+"("+this.vertices[i].ToString()+") => ";
			foreach( Adjacent adj in this.adjacency[i] )
			{
				res += adj.index.ToString()+"("+adj.weight.ToString()+") ";
			}
			res += "\n";
		}
		res += "}\n";

		return res;
	}

	public static Graph readFromFile( string fileName )
	{
		/*
		 * File format :
		 * nVertices nEdges 0|1(bidirectional)
		 * nVertices * value
		 * nEdges * ( v1, v2, weight)
		*/
		string path = "Assets/Graphs/" + fileName;

		// Starting Reader
		System.IO.StreamReader reader;
		try{
			reader = new System.IO.StreamReader( path );
		}
		catch( System.Exception ex ){
			Debug.LogError ( "Error while opening the Graph file" );
			return null;
		}

		// Reading Information
		try{
			// initial info
			string[] line = reader.ReadLine().Split(' ');
			int nV = int.Parse(line [0]);
			int nE = int.Parse(line [1]);
			bool bidirectional = int.Parse(line [2]) == 1;
			Graph g = new Graph (nV, bidirectional);

			// Vertices
			for (int i = 0; i < nV; i++) {
				g.setVertex( i, int.Parse( reader.ReadLine () ) );
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
			return null;
		}
	}
}
